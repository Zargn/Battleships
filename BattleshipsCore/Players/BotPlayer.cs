using Battleships.EventArguments;
using Battleships.Interfaces;
using Battleships.objects;
using Battleships.objects.Enums;

namespace Battleships.Players;

public class BotPlayer : IPlayer
{
    private Arena? arena;
    private BattleshipsAi? battleshipsAi;

    private static readonly string[] aiNames =
    {
        "Bob", "Karl", "Felix", "James", "Mary", "Robert", "Patricia", "John", "Jennifer", "Michael",
        "Linda", "David", "Elizabeth", "William", "Barbara", "Richard", "Susan", "Joseph", "Jessica"
    };
    
    
    public Task InitializePlayer(int[] shipLengths, int xSize, int ySize, CancellationToken cancellationToken)
    {
        arena = new Arena(xSize, ySize);
        arena.RandomiseShipLocations(shipLengths);

        battleshipsAi = new BattleshipsAi(xSize, ySize);
        
        ShipsLeft = shipLengths.Length;
        arena.ShipSunk += HandleShipSunkEvent;

        UserName = $"Ai {aiNames[Random.Shared.Next(aiNames.Length)]}";

        return Task.CompletedTask;
    }

    public StartingPlayer PlayerStartPriority => StartingPlayer.Maybe;
    public string UserName { get; private set; }
    public Tile[,] KnownArenaTiles => arena.CurrentView;
    public int ShipsLeft { get; private set; }
    Task<Tile[,]?> IPlayer.AllArenaTiles()
    {
        return Task.FromResult(arena.CompleteView);
    }

    public async Task<TurnResult> PlayTurnAsync(IPlayer target, CancellationToken cancellationToken)
    {
        var hitResult = await battleshipsAi.PlayTurn(target, cancellationToken);
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