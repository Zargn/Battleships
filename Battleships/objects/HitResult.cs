namespace Battleships.objects;

public class HitResult
{
    public bool shipHit { get; }
    public Ship? Ship { get; }

    public HitResult(bool shipHit, Ship? ship)
    {
        this.shipHit = shipHit;
        Ship = ship;
    }
}