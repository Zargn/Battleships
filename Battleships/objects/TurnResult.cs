namespace Battleships.objects;

public class TurnResult
{
    public TurnResult(bool shipHit, bool shipSunk, bool playerDefeated)
    {
        ShipHit = shipHit;
        ShipSunk = shipSunk;
        PlayerDefeated = playerDefeated;
    }

    public bool ShipHit { get; }
    public bool ShipSunk { get; }
    public bool PlayerDefeated { get; }
}