using Battleships.EventArguments;
using Battleships.Interfaces;
using Battleships.objects;
using Battleships.objects.Enums;
using Battleships.objects.networking;
using ForwardingClient;

namespace Battleships.Players;

public class RemotePlayer : IPlayer
{
    private IUserInterface userInterface;
    private FwClient netClient;


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
        await ListGroups();
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
            string groupCode = new string(letterArray);
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

    private async Task ListGroups()
    {
        
    }



    public Task<TurnResult> PlayTurnAsync(IPlayer target, CancellationToken cancellationToken)
    {
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