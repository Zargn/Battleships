using Battleships.EventArguments;
using Battleships.Interfaces;
using Battleships.objects;
using Battleships.objects.Enums;
using Battleships.objects.networking;
using ForwardingClient;
using ForwardingServer;
using ForwardingServer.Resources;
using ForwardingServer.Resources.InformationPackages;
using Unnamed_Networking_Plugin;

namespace Battleships.Players;

public class RemotePlayer : IPlayer
{
    private IUserInterface userInterface;
    private FwClient netClient;
    private string groupCode;
    private IPlayer opponent;
    private string remoteGroupCode;
    private CancellationTokenSource gameCancelSource;
    
    
    private SemaphoreSlim userNameReceived = new SemaphoreSlim(0, 1);
    private SemaphoreSlim userInGroup = new SemaphoreSlim(0, 1);
    private SemaphoreSlim errorReceived = new SemaphoreSlim(0, 1);
    private SemaphoreSlim warningReceived = new SemaphoreSlim(0, 1);


    private TargetCoordinates hitTileCoordinatesCache;
    private SemaphoreSlim hitTileCoordinatesReceived = new SemaphoreSlim(0, 1);
    
    private HitResult? hitResultCache;
    private SemaphoreSlim hitResultReceived = new SemaphoreSlim(0, 1);

    public StartingPlayer PlayerStartPriority { get; private set; }
    public string UserName { get; private set; }
    public Tile[,] KnownArenaTiles { get; set; }
    public int ShipsLeft { get; private set; }
    
    
    
    public RemotePlayer(IUserInterface userInterface, IPlayer opponent)
    {
        this.userInterface = userInterface;
        this.opponent = opponent;
    }
    
    
    
    public async Task InitializePlayer(int[] shipLengths, int xSize, int ySize, CancellationToken cancellationToken, CancellationTokenSource gameCancelSource)
    {
        this.gameCancelSource = gameCancelSource;
        
        KnownArenaTiles = new Tile[xSize, ySize];
        ShipsLeft = shipLengths.Length;
        
        netClient = await ConnectToServer(cancellationToken);
        ConfigureSubscribers();
        // await ListGroups();

        if (userInterface.GetYesNoAnswer("Do you want to join a existing group?"))
        {
            await JoinMode(cancellationToken);
            PlayerStartPriority = StartingPlayer.Yes;
        }
        else
        {
            await CreateMode(cancellationToken);
            PlayerStartPriority = StartingPlayer.No;
        }
        
        
    }

    private async Task<FwClient> ConnectToServer(CancellationToken cancellationToken)
    {
        FwClient? client = null;
        bool success = false;
        while (!success)
        {
            if (cancellationToken.IsCancellationRequested)
                throw new OperationCanceledException();
            
            var allowedLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var letterArray = new char[5];
            for (int i = 0; i < letterArray.Length; i++)
                letterArray[i] = allowedLetters[Random.Shared.Next(allowedLetters.Length)];
            groupCode = new string(letterArray);
            var identification = new OnlineUserIdentification(groupCode);
            var identificationPackage = new OnlineUserIdentificationPackage(identification);

            client = new FwClient(new ConsoleLogger(), new JsonSerializerAdapter(), identificationPackage);

            var ip = userInterface.GetIpAddress();

            success = await client.ConnectAsync(ip, 25564);
        }
        
        return client;

        // Try to connect using other players username as identification.
        // If user already exists then add 1 to the end of username and try again. Repeat if needed with 2, 3, 4, etc.
        // throw new NotImplementedException();
    }

    private void ConfigureSubscribers()
    {
        netClient.PackageBroker.SubscribeToPackage<GroupsListPackage>(HandleGroupsListPackage);
        netClient.PackageBroker.SubscribeToPackage<WarningPackage>(HandleWarningPackage);
        netClient.PackageBroker.SubscribeToPackage<ErrorPackage>(HandleErrorPackage);
        netClient.PackageBroker.SubscribeToPackage<UserNamePackage>(HandleUserNamePackage);
        netClient.PackageBroker.SubscribeToPackage<ClientJoinedGroupPackage<OnlineUserIdentification>>(HandleJoinedGroupPackage);
        netClient.PackageBroker.SubscribeToPackage<HitTilePackage>(HandleHitTilePackage);
        netClient.PackageBroker.SubscribeToPackage<HitResultPackage>(HandleHitResultPackage);
        netClient.PackageBroker.SubscribeToPackage<ShipSunkPackage>(HandleShipSunkPackage);
    }
    
    private async Task ListGroups()
    {
        await netClient.SendListGroupsRequest();
    }
    
    
    
    private async Task JoinMode(CancellationToken cancellationToken)
    {
        // TODO: It is probably better to throw instead of stopping the loop when canceled.
        while (!cancellationToken.IsCancellationRequested)
        {
            var targetCode = userInterface.GetTargetGroupCode();

            await netClient.SendJoinGroupRequest(new GroupSettings(0, targetCode, ""));

            // await userInGroup.WaitAsync(cancellationToken);
            var success = await SuccessfullyJoined();
            if (!success)
                continue;
            
            await netClient.SendPackageToAllGroupMembers(new UserNamePackage(opponent.UserName));

            await userNameReceived.WaitAsync(cancellationToken);
            
            return;
        }
    }

    private async Task<bool> SuccessfullyJoined()
    {
        var waitForUserInGroup = userInGroup.WaitAsync();
        await Task.WhenAny(waitForUserInGroup, errorReceived.WaitAsync(), warningReceived.WaitAsync());
        return waitForUserInGroup.IsCompleted;
    }

    private async Task WaitForErrorReceived()
    {
        await errorReceived.WaitAsync();
    }



    private async Task CreateMode(CancellationToken cancellationToken)
    {
        await CreateGroup();
        
        userInterface.DisplayMessage($"Group created. Join code is: [{groupCode}]");
        userInterface.DisplayMessage($"Waiting for remote player to connect...");
        
        await userNameReceived.WaitAsync(cancellationToken);
        await netClient.SendPackageToAllGroupMembers(new UserNamePackage(opponent.UserName));

        userInterface.DisplayMessage($"{UserName} joined the game");
    }

    private async Task CreateGroup()
    {
        var groupSettings = new GroupSettings(2, groupCode, "This should be the local players username or something");

        await netClient.SendCreateGroupRequest(groupSettings);
    }



    private void HandleGroupsListPackage(object? o, PackageReceivedEventArgs args)
    {
        var groupList = (args.ReceivedPackage as GroupsListPackage).GroupInformation;
        foreach (var groupInformation in groupList)
        {
            Console.WriteLine(groupInformation);
        }
    }

    private void HandleWarningPackage(object? o, PackageReceivedEventArgs args)
    {
        var warningPackage = args.ReceivedPackage as WarningPackage;
        userInterface.DisplayError($"[{warningPackage.WarningType}] {warningPackage.WarningMessage}");
        warningReceived.Release();
    }

    private void HandleErrorPackage(object? o, PackageReceivedEventArgs args)
    {
        var errorPackage = args.ReceivedPackage as ErrorPackage;
        userInterface.DisplayError($"Server: {errorPackage.ErrorMessage} | {errorPackage.Exception}");
        errorReceived.Release();
    }

    private void HandleUserNamePackage(object? o, PackageReceivedEventArgs args)
    {
        var package = args.ReceivedPackage as UserNamePackage;
        UserName = package.UserName;
        userNameReceived.Release();
    }

    private void HandleJoinedGroupPackage(object? o, PackageReceivedEventArgs args)
    {
        var package = args.ReceivedPackage as ClientJoinedGroupPackage<OnlineUserIdentification>;
        if (package.ClientInformation.UserName == groupCode)
            userInGroup.Release();
    }

    private void HandleHitTilePackage(object? o, PackageReceivedEventArgs args)
    {
        var package = args.ReceivedPackage as HitTilePackage;

        hitTileCoordinatesCache = package.TargetCoordinates;
        hitTileCoordinatesReceived.Release();
    }

    private void HandleHitResultPackage(object? o, PackageReceivedEventArgs args)
    {
        var package = args.ReceivedPackage as HitResultPackage;
        hitResultCache = package.HitResult;
        hitResultReceived.Release();
    }

    private void HandleShipSunkPackage(object? o, PackageReceivedEventArgs args)
    {
        ShipsLeft--;
    }
    


    public async Task<TurnResult> PlayTurnAsync(IPlayer target, CancellationToken cancellationToken)
    {
        await hitTileCoordinatesReceived.WaitAsync(cancellationToken);

        var targetCoordinates = hitTileCoordinatesCache;

        var hitResult = await target.HitTile(targetCoordinates, cancellationToken);

        var turnResult = new TurnResult(hitResult.shipHit, hitResult.Ship?.Health <= 0, target.PlayerDefeated);
        
        if (turnResult.ShipHitDEPRECATED)
            userInterface.DisplayMessage($"{UserName} hit one of your ships!");
        if (turnResult.ShipSunkDEPRECATED)
            userInterface.DisplayMessage($"{UserName} sunk one of your ships!");
        if (turnResult.TargetPlayerDefeated)
            userInterface.DisplayMessage($"{UserName} sunk all of your ships!");

        userInterface.DrawTiles(target.KnownArenaTiles);

        if (hitResult.Ship?.ShipSunk == true)
            await netClient.SendPackageToAllGroupMembers(new ShipSunkPackage());

        await netClient.SendPackageToAllGroupMembers(new HitResultPackage(hitResult));

        return turnResult;
    }

    
    
    public async Task<HitResult> HitTile(TargetCoordinates targetCoordinates, CancellationToken cancellationToken)
    {
        await netClient.SendPackageToAllGroupMembers(new HitTilePackage(targetCoordinates));

        await hitResultReceived.WaitAsync(cancellationToken);

        var cached = hitResultCache;
        hitResultCache = null;

        KnownArenaTiles[targetCoordinates.X, targetCoordinates.Y].Hit = true;
        KnownArenaTiles[targetCoordinates.X, targetCoordinates.Y].OccupiedByShip = cached.shipHit;
        
        return cached;
    }

    
    
    public Task UnloadPlayer(EndOfGameStatistics endOfGameStatistics)
    {
        throw new NotImplementedException();
    }

    
    
    public event EventHandler<PlayerDefeatedEventArgs>? PlayerDefeatedDEPRECATED;
    public event EventHandler<ShipSunkEventArgs>? ShipSunkDEPRECATED;
}