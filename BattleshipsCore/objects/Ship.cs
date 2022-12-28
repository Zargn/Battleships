using BattleshipsCore.objects.Enums;

namespace BattleshipsCore.objects;

public class Ship
{
    public Ship(int length, string name)
    {
        Length = length;
        Name = name;
        Type = ((ShipType) length).ToString();
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