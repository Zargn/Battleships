namespace Battleships.objects;

public class TurnResult
{
    public TurnResult(bool shipHit, bool shipSunk, bool targetPlayerDefeated)
    {
        ShipHit = shipHit;
        ShipSunk = shipSunk;
        TargetPlayerDefeated = targetPlayerDefeated;
    }

    public bool ShipHit { get; }
    public bool ShipSunk { get; }
    public bool TargetPlayerDefeated { get; }
}