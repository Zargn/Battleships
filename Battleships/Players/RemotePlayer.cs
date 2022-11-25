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


    public StartingPlayer PlayerStartPriority { get; }
    public string UserName { get; }
    public Tile[,] KnownArenaTiles { get; }
    public int ShipsLeft { get; }
    
    
    
    public RemotePlayer(IUserInterface userInterface)
    {
        this.userInterface = userInterface;
    }
    
    
    
    public async Task InitializePlayer(int[] shipLengths, int xSize, int ySize, CancellationToken cancellationToken)
    {
        netClient = await ConnectToServer(cancellationToken);
        ConfigureSubscribers();
        await ListGroups();
        await CreateGroup();
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
        netClient.PackageBroker.SubscribeToPackage<ErrorPackage>(HandleErrorPackage);
    }
    
    private async Task ListGroups()
    {
        await netClient.SendListGroupsRequest();
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

    private void HandleErrorPackage(object? o, PackageReceivedEventArgs args)
    {
        var errorPackage = args.ReceivedPackage as ErrorPackage;
        userInterface.DisplayError($"{errorPackage.Type}: {errorPackage.ErrorMessage} | {errorPackage.Exception}");
    }




    public Task<TurnResult> PlayTurnAsync(IPlayer target, CancellationToken cancellationToken)
    {
        Console.ReadLine();
        throw new NotImplementedException();
    }

    
    
    public Task<HitResult> HitTile(TargetCoordinates targetCoordinates, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    
    
    public Task UnloadPlayer(EndOfGameStatistics endOfGameStatistics)
    {
        throw new NotImplementedException();
    }

    
    
    public event EventHandler<PlayerDefeatedEventArgs>? PlayerDefeatedDEPRECATED;
    public event EventHandler<ShipSunkEventArgs>? ShipSunkDEPRECATED;
}