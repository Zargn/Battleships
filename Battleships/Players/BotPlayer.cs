using Battleships.EventArguments;
using Battleships.Interfaces;
using Battleships.objects;
using Battleships.objects.Enums;

namespace Battleships.Players;

public class BotPlayer : IPlayer
{
    private Arena? arena;
    private BattleshipsAi battleshipsAi;

    public BotPlayer()
    {
        battleshipsAi = new BattleshipsAi();
    }
    
    public Task InitializePlayer(int[] shipLengths, int xSize, int ySize, CancellationToken cancellationToken)
    {
        arena = new Arena(xSize, ySize);
        arena.RandomiseShipLocations(shipLengths);

        ShipsLeft = shipLengths.Length;
        arena.ShipSunk += HandleShipSunkEvent;
        
        return Task.CompletedTask;
    }

    public StartingPlayer PlayerStartPriority => StartingPlayer.Maybe;
    public string UserName => "Ai";
    public Tile[,] KnownArenaTiles => arena.CurrentView;
    public int ShipsLeft { get; private set; }
    Task<Tile[,]?> IPlayer.AllArenaTiles()
    {
        return Task.FromResult(arena.CompleteView);
    }

    public async Task<TurnResult> PlayTurnAsync(IPlayer target, CancellationToken cancellationToken)
    {
        var targetCoordinates = battleshipsAi.CalculateNextShot();
        var hitResult = await target.HitTile(targetCoordinates, cancellationToken);
        var turnResult = new TurnResult(hitResult.ShipHit, hitResult.Ship?.Health <= 0, target.PlayerDefeated, hitResult.Ship);
        return turnResult;
    }

    public Task<HitResult> HitTile(TargetCoordinates targetCoordinates, CancellationToken cancellationToken)
    {
        return Task.FromResult(arena.FireAtTile(targetCoordinates));
    }

    public Task UnloadPlayer(EndOfGameStatistics endOfGameStatistics)
    {
        throw new NotImplementedException();
    }

    public event EventHandler<PlayerUnavailableEventArgs>? PlayerUnavailable;
    public event EventHandler<PlayerDefeatedEventArgs>? PlayerDefeatedDEPRECATED;
    public event EventHandler<ShipSunkEventArgs>? ShipSunkDEPRECATED;
    
    private void HandleShipSunkEvent(object? o, EventArgs e)
    {
        ShipsLeft--;
    }
}