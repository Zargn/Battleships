namespace BattleshipsCore.objects;

public class HitResult
{
    public bool ShipHit { get; }
    public Ship? Ship { get; }

    public HitResult(bool shipHit, Ship? ship)
    {
        ShipHit = shipHit;
        Ship = ship;
    }
}