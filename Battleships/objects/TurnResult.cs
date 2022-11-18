namespace Battleships.objects;

public class TurnResult
{
    public TurnResult(bool shipHit, bool shipSunk, bool targetPlayerDefeated)
    {
        ShipHitDEPRECATED = shipHit;
        ShipSunkDEPRECATED = shipSunk;
        TargetPlayerDefeated = targetPlayerDefeated;
    }

    public bool ShipHitDEPRECATED { get; }
    public bool ShipSunkDEPRECATED { get; }
    
    // TODO: This class could potentially be completely replaced by a normal bool.
    public bool TargetPlayerDefeated { get; }
}