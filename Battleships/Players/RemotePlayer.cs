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
    private readonly IUserInterface userInterface;
    private FwClient netClient;
    private string groupCode;
    private readonly IPlayer opponent;


    private readonly SemaphoreSlim userNameReceived = new(0, 1);
    private readonly SemaphoreSlim userInGroup = new(0, 1);
    private readonly SemaphoreSlim errorReceived = new(0, 1);
    private readonly SemaphoreSlim warningReceived = new(0, 1);


    private TargetCoordinates hitTileCoordinatesCache;
    private readonly SemaphoreSlim hitTileCoordinatesReceived = new(0, 1);
    
    private HitResult? hitResultCache;
    private readonly SemaphoreSlim hitResultReceived = new(0, 1);

    private Tile[,]? allArenaTilesCache;
    private readonly SemaphoreSlim allArenaTilesReceived = new(0, 1);

    public StartingPlayer PlayerStartPriority { get; private set; }
    public string UserName { get; private set; }
    public Tile[,] KnownArenaTiles { get; set; }
    public int ShipsLeft { get; private set; }
    
    Task<Tile[,]?> IPlayer.AllArenaTiles()
    {
        return RequestEndOfGameTiles();
    }



    public RemotePlayer(IUserInterface userInterface, IPlayer opponent)
    {
        this.userInterface = userInterface;
        this.opponent = opponent;
    }



    private async Task<Tile[,]?> RequestEndOfGameTiles()
    {
        await netClient.SendPackageToAllGroupMembers(new RequestEndOfGameTilesPackage());
        if (!await ReceivedWithOutErrors(allArenaTilesReceived))
            return null;
        var cache = allArenaTilesCache;
        allArenaTilesCache = null;
        return cache;
    }
    
    
    
    #region Initialization
    
    public async Task InitializePlayer(int[] shipLengths, int xSize, int ySize, CancellationToken cancellationToken)
    {
        KnownArenaTiles = new Tile[xSize, ySize];
        ShipsLeft = shipLengths.Length;
        
        netClient = await ConnectToServer(cancellationToken);
        
        ConfigurePackageHandlers();

        if (userInterface.GetYesNoAnswer("Do you want to join a existing group?", cancellationToken))
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

            var ip = userInterface.GetIpAddress(cancellationToken);

            success = await client.ConnectAsync(ip, 25564);
        }
        
        return client;
    }

    private void ConfigurePackageHandlers()
    {
        netClient.PackageBroker.SubscribeToPackage<WarningPackage>(HandleWarningPackage);
        netClient.PackageBroker.SubscribeToPackage<ErrorPackage>(HandleErrorPackage);
        netClient.PackageBroker.SubscribeToPackage<BattleshipsWarningPackage>(HandleBattleshipsWarningPackage);
        netClient.PackageBroker.SubscribeToPackage<UserNamePackage>(HandleUserNamePackage);
        netClient.PackageBroker.SubscribeToPackage<ClientJoinedGroupPackage<OnlineUserIdentification>>(HandleJoinedGroupPackage);
        netClient.PackageBroker.SubscribeToPackage<HitTilePackage>(HandleHitTilePackage);
        netClient.PackageBroker.SubscribeToPackage<HitResultPackage>(HandleHitResultPackage);
        netClient.PackageBroker.SubscribeToPackage<ShipSunkPackage>(HandleShipSunkPackage);
        netClient.PackageBroker.SubscribeToPackage<ClientLeftGroupPackage<OnlineUserIdentification>>(HandleClientLeftGroupPackage);
        netClient.PackageBroker.SubscribeToPackage<RequestEndOfGameTilesPackage>(HandleRequestEndOfGameTilesPackage);
        netClient.PackageBroker.SubscribeToPackage<ShipLocationsPackage>(HandleShipLocationsPackage);

        netClient.ClientDisconnected += HandleClientDisconnected;
    }



    private async Task JoinMode(CancellationToken cancellationToken)
    {
        while (true)
        {
            var targetCode = userInterface.GetTargetGroupCode(cancellationToken);

            if (cancellationToken.IsCancellationRequested)
                throw new OperationCanceledException();

            await netClient.SendJoinGroupRequest(new GroupSettings(0, targetCode.ToUpper(), ""));

            var success = await ReceivedWithOutErrors(userInGroup);
            if (!success)
                continue;
            
            await netClient.SendPackageToAllGroupMembers(new UserNamePackage(opponent.UserName));

            await userNameReceived.WaitAsync(cancellationToken);
            
            return;
        }
    }
    
    
    
    private async Task<bool> ReceivedWithOutErrors(SemaphoreSlim packageSignal)
    {
        var waitForSignal = packageSignal.WaitAsync();
        await Task.WhenAny(waitForSignal, errorReceived.WaitAsync(), warningReceived.WaitAsync());
        return waitForSignal.IsCompleted;
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
        var groupSettings = new GroupSettings(2, groupCode, "");

        await netClient.SendCreateGroupRequest(groupSettings);
    }
    
    #endregion
    
    

    public async Task<TurnResult> PlayTurnAsync(IPlayer target, CancellationToken cancellationToken)
    {
        await hitTileCoordinatesReceived.WaitAsync(cancellationToken);

        var targetCoordinates = hitTileCoordinatesCache;

        var hitResult = await target.HitTile(targetCoordinates, cancellationToken);

        var turnResult = new TurnResult(hitResult.ShipHit, hitResult.Ship?.Health <= 0, target.PlayerDefeated, hitResult.Ship);
        
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
        KnownArenaTiles[targetCoordinates.X, targetCoordinates.Y].OccupiedByShip = cached.ShipHit;
        
        return cached;
    }

    
    
    public Task UnloadPlayer(EndOfGameStatistics endOfGameStatistics)
    {
        throw new NotImplementedException();
    }

    public event EventHandler<PlayerUnavailableEventArgs>? PlayerUnavailable;
    public event EventHandler<PlayerDefeatedEventArgs>? PlayerDefeatedDEPRECATED;
    public event EventHandler<ShipSunkEventArgs>? ShipSunkDEPRECATED;
    
    
    #region PackageHandlers
    
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

    private void HandleBattleshipsWarningPackage(object? o, PackageReceivedEventArgs args)
    {
        var package = args.ReceivedPackage as BattleshipsWarningPackage;
        
        // Todo: Maybe add more detailed warning support with type or similar?
        
        userInterface.DisplayError($"[Unknown] {package.Message}");
        warningReceived.Release();
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

    private void HandleClientLeftGroupPackage(object? o, PackageReceivedEventArgs args)
    {
        var reason = $"{UserName} left the game";
        var detailedReason = $"{UserName} unexpectedly left the game.";
        PlayerUnavailable?.Invoke(this, new PlayerUnavailableEventArgs(reason, detailedReason));
    }

    private void HandleClientDisconnected(object? o, EventArgs args)
    {
        var reason = $"Lost connection to game server";
        var detailedReason = $"Something caused the connection to the game server to be lost. Please check your internet connection or contact the developer.";
        PlayerUnavailable?.Invoke(this, new PlayerUnavailableEventArgs(reason, detailedReason));
    }

    private async void HandleRequestEndOfGameTilesPackage(object? o, PackageReceivedEventArgs args)
    {
        var tiles = await opponent.GetEndOfGameTiles(this);
        if (tiles == null)
        {
            var message =
                "Player did not accept request for complete view of their arena. They do not recognise your defeat.";
            await netClient.SendPackageToAllGroupMembers(new BattleshipsWarningPackage(message));
            return;
        }

        List<TargetCoordinates> locations = new();
        
        for (int y = 0; y < tiles.GetLength(1); y++)
        {
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                if (tiles[x, y].OccupiedByShip)
                    locations.Add(new TargetCoordinates(x, y));
            }
        }

        await netClient.SendPackageToAllGroupMembers(new ShipLocationsPackage(locations.ToArray()));
    }
    
    private void HandleShipLocationsPackage(object? o, PackageReceivedEventArgs args)
    {
        var package = args.ReceivedPackage as ShipLocationsPackage;
        var tiles = KnownArenaTiles;

        foreach (var shipLocation in package.Locations)
        {
            tiles[shipLocation.X, shipLocation.Y].OccupiedByShip = true;
        }

        allArenaTilesCache = tiles;
        allArenaTilesReceived.Release();
    }
    
    #endregion
}