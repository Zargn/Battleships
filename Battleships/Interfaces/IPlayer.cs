using Battleships.EventArguments;
using Battleships.objects;

namespace Battleships.Interfaces;

public interface IPlayer
{
    public Task InitializePlayer(Arena arena, CancellationToken cancellationToken);

    public Task PlayTurn(CancellationToken cancellationToken);
    
    // public Task<FiringTarget> GetFiringTargetAsync();

    public Task<bool> HitTile(TargetCoordinates targetCoordinates);

    public Task UnloadPlayer(EndOfGameStatistics endOfGameStatistics);

    public event EventHandler<PlayerDefeatedEventArgs> PlayerDefeated;
}