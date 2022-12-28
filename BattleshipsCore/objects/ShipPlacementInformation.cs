namespace Battleships.objects;

public class ShipPlacementInformation
{
    public TargetCoordinates Coordinates { get; }
    public TargetCoordinates Direction { get; }
    public string Name { get; }

    public ShipPlacementInformation(TargetCoordinates coordinates, TargetCoordinates direction, string name)
    {
        Coordinates = coordinates;
        Direction = direction;
        Name = name;
    }
}