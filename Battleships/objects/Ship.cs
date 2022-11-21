namespace Battleships.objects;

public class Ship
{
    public Ship(int length, string name, string type)
    {
        Length = length;
        Name = name;
        Type = type;
        Health = length;
        CoordinatesArray = new TargetCoordinates[length];
    }

    public int Length { get; }
    public string Name { get; }
    public string Type { get; }
    public int Health { get; set; }
    public bool ShipSunk => Health <= 0;
    public TargetCoordinates[] CoordinatesArray { get; }
}