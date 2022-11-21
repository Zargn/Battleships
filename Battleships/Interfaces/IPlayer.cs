using Battleships.EventArguments;
using Battleships.objects;
using Battleships.objects.Enums;

namespace Battleships.Interfaces;

public interface IPlayer
{
    public Task InitializePlayer(CancellationToken cancellationToken);

    public StartingPlayer PlayerStartPriority { get; }
    
    public Tile[,] KnownArenaTiles { get; }
    
    public Task<TurnResult> PlayTurnAsync(IPlayer target, CancellationToken cancellationToken);
    
    // public Task<FiringTarget> GetFiringTargetAsync();

    public Task<bool> HitTile(TargetCoordinates targetCoordinates);

    public Task UnloadPlayer(EndOfGameStatistics endOfGameStatistics);

    public event EventHandler<PlayerDefeatedEventArgs>? PlayerDefeatedDEPRECATED;

    public event EventHandler<ShipSunkEventArgs>? ShipSunkDEPRECATED;
}