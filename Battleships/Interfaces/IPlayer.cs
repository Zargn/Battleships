using Battleships.EventArguments;
using Battleships.objects;

namespace Battleships.Interfaces;

public interface IPlayer
{
    public Task InitializePlayer(Arena arena, CancellationToken cancellationToken);

    public Task PlayTurn();
    
    // public Task<FiringTarget> GetFiringTargetAsync();

    public Task<Tile> HitTile(TargetCoordinates targetCoordinates);

    public Task UnloadPlayer(EndOfGameStatistics endOfGameStatistics);

    public event EventHandler<PlayerDefeatedEventArgs> PlayerDefeated;
}