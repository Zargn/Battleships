using Battleships.EventArguments;
using Battleships.objects;
using Battleships.objects.Enums;

namespace Battleships.Interfaces;

public interface IPlayer
{
    public Task InitializePlayer(Arena arena, CancellationToken cancellationToken);

    public StartingPlayer GetPlayerStartPriority();
    
    public Task PlayTurn(CancellationToken cancellationToken);
    
    // public Task<FiringTarget> GetFiringTargetAsync();

    public Task<bool> HitTile(TargetCoordinates targetCoordinates);

    public Task UnloadPlayer(EndOfGameStatistics endOfGameStatistics);

    public event EventHandler<PlayerDefeatedEventArgs>? PlayerDefeated;

    public event EventHandler<ShipSunkEventArgs>? ShipSunk;
}