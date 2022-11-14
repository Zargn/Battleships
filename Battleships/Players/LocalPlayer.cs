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
        var firingTarget = GetFiringTarget();

        var fireAtPlayerTask = FireAtOtherPlayer(firingTarget);
        
        throw new NotImplementedException();
    }

    public Task<Tile> HitTile(FiringTarget firingTarget)
    {
        throw new NotImplementedException();
    }

    public event EventHandler<PlayerDefeatedEventArgs>? PlayerDefeated;

    private FiringTarget GetFiringTarget()
    {
        throw new NotImplementedException();
    }

    private Task FireAtOtherPlayer(FiringTarget firingTarget)
    {
        throw new NotImplementedException();
    }
}