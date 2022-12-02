using Battleships.EventArguments;
using Battleships.Interfaces;
using Battleships.objects;
using Battleships.objects.Enums;

namespace Battleships.Players;

public class BotPlayer : IPlayer
{
    private Arena? arena;
    
    
    public Task InitializePlayer(int[] shipLengths, int xSize, int ySize, CancellationToken cancellationToken)
    {
        arena = new Arena(xSize, ySize);
        arena.RandomiseShipLocations(shipLengths);
        
        return Task.CompletedTask;
    }

    public StartingPlayer PlayerStartPriority => StartingPlayer.Maybe;
    public string UserName => "Ai";
    public Tile[,] KnownArenaTiles { get; }
    public int ShipsLeft { get; }
    Task<Tile[,]?> IPlayer.AllArenaTiles()
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

    public event EventHandler<PlayerUnavailableEventArgs>? PlayerUnavailable;
    public event EventHandler<PlayerDefeatedEventArgs>? PlayerDefeatedDEPRECATED;
    public event EventHandler<ShipSunkEventArgs>? ShipSunkDEPRECATED;
}