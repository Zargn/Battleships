namespace Battleships.objects;

public class Ship
{
    public Ship(int length, string name, string type)
    {
        Length = length;
        Name = name;
        Type = type;
        CoordinatesArray = new TargetCoordinates[length];
    }

    public int Length { get; }
    public string Name { get; }
    public string Type { get; }
    public TargetCoordinates[] CoordinatesArray { get; }
}