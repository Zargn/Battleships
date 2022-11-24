using Battleships.EventArguments;
using Battleships.objects;
using Battleships.objects.Enums;

namespace Battleships.Interfaces;

public interface IPlayer
{
    public Task InitializePlayer(int[] shipLengths, int xSize, int ySize, CancellationToken cancellationToken);

    public StartingPlayer PlayerStartPriority { get; }
    public string UserName { get; }
    
    public Tile[,] KnownArenaTiles { get; }
    public int ShipsLeft { get; }
    public bool PlayerDefeated => ShipsLeft <= 0;
    
    public Task<TurnResult> PlayTurnAsync(IPlayer target, CancellationToken cancellationToken);
    
    // public Task<FiringTarget> GetFiringTargetAsync();

    public Task<HitResult> HitTile(TargetCoordinates targetCoordinates);

    public Task UnloadPlayer(EndOfGameStatistics endOfGameStatistics);

    public event EventHandler<PlayerDefeatedEventArgs>? PlayerDefeatedDEPRECATED;

    public event EventHandler<ShipSunkEventArgs>? ShipSunkDEPRECATED;
}