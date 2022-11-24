using Battleships.EventArguments;
using Battleships.Interfaces;
using Battleships.objects;
using Battleships.objects.Enums;

namespace Battleships.Players;

public class RemotePlayer : IPlayer
{
    private IUserInterface userInterface;
    
    

    public StartingPlayer PlayerStartPriority { get; }
    public string UserName { get; }
    public Tile[,] KnownArenaTiles { get; }
    public int ShipsLeft { get; }
    
    
    
    public RemotePlayer(IUserInterface userInterface)
    {
        this.userInterface = userInterface;
    }
    
    
    
    public Task InitializePlayer(int[] shipLengths, int xSize, int ySize, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
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