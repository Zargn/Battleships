namespace BattleshipsCore.objects;

public class TurnResult
{
    public TurnResult(bool shipHit, bool shipSunk, bool targetPlayerDefeated, Ship? ship)
    {
        ShipHit = shipHit;
        ShipSunk = shipSunk;
        TargetPlayerDefeated = targetPlayerDefeated;
        Ship = ship;
    }

    public bool ShipHit { get; }
    public bool ShipSunk { get; }
    public Ship? Ship { get; }
    public bool TargetPlayerDefeated { get; }
}