using Battleships.EventArguments;
using Battleships.Interfaces;
using Battleships.objects;

namespace Battleships.Players;

public class LocalPlayer : IPlayer
{
    private Arena arena;
    
    public Task InitializePlayer(Arena arena, CancellationToken cancellationToken)
    {
        this.arena = arena;
        throw new NotImplementedException();
    }

    public Task PlayTurn()
    {
        throw new NotImplementedException();
    }

    public Task<Tile> HitTile(FiringTarget firingTarget)
    {
        throw new NotImplementedException();
    }

    public event EventHandler<PlayerDefeatedEventArgs>? PlayerDefeated;
}