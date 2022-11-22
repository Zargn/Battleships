using Battleships.EventArguments;
using Battleships.Interfaces;
using Battleships.objects;
using Battleships.objects.Enums;

namespace Battleships.Players;

public class LocalPlayer : IPlayer
{
    private Arena arena;
    private StartingPlayer playerStartPriority;

    public Task InitializePlayer(CancellationToken cancellationToken)
    {
        
        
        this.arena = arena;
        throw new NotImplementedException();
    }

    public StartingPlayer PlayerStartPriority { get; private set; }
    public string UserName { get; private set; }

    public Tile[,] KnownArenaTiles => arena.CurrentView;

    public Task<TurnResult> PlayTurnAsync(IPlayer target, CancellationToken cancellationToken)
    {
        var firingTarget = GetFiringTarget();

        var fireAtPlayerTask = FireAtOtherPlayer(firingTarget);
        
        throw new NotImplementedException();
    }

    public Task<HitResult> HitTile(TargetCoordinates targetCoordinates)
    {
        return Task.FromResult(arena.FireAtTile(targetCoordinates));
        
        throw new NotImplementedException();
    }

    public Task UnloadPlayer(EndOfGameStatistics endOfGameStatistics)
    {
        throw new NotImplementedException();
    }

    public event EventHandler<PlayerDefeatedEventArgs>? PlayerDefeatedDEPRECATED;
    public event EventHandler<ShipSunkEventArgs>? ShipSunkDEPRECATED;

    
    
    private TargetCoordinates GetFiringTarget()
    {
        throw new NotImplementedException();
    }

    private Task FireAtOtherPlayer(TargetCoordinates targetCoordinates)
    {
        throw new NotImplementedException();
    }
}