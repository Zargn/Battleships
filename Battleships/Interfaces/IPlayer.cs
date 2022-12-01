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
    protected Tile[,] AllArenaTiles { get; }

    /// <summary>
    /// Get tiles including ship positions.
    /// Returns null if both players are not defeated.
    /// </summary>
    /// <param name="otherPlayer">opponent player</param>
    /// <returns>arena tiles including alive ship positions.</returns>
    public Tile[,]? GetEndOfGameTiles(IPlayer otherPlayer)
    {
        if (otherPlayer.PlayerDefeated)
            return AllArenaTiles;
        if (PlayerDefeated)
            return KnownArenaTiles;
        return null;
    }

    public Task<TurnResult> PlayTurnAsync(IPlayer target, CancellationToken cancellationToken);

    public Task<HitResult> HitTile(TargetCoordinates targetCoordinates, CancellationToken cancellationToken);

    public Task UnloadPlayer(EndOfGameStatistics endOfGameStatistics);

    public event EventHandler<PlayerUnavailableEventArgs>? PlayerUnavailable;
    public event EventHandler<PlayerDefeatedEventArgs>? PlayerDefeatedDEPRECATED;

    public event EventHandler<ShipSunkEventArgs>? ShipSunkDEPRECATED;
}