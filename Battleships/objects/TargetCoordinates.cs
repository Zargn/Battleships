namespace Battleships.objects;

public struct TargetCoordinates
{
    public int XCoordinate;
    public int YCoordinate;

    public TargetCoordinates(int xCoordinate, int yCoordinate)
    {
        XCoordinate = xCoordinate;
        YCoordinate = yCoordinate;
    }

    public static TargetCoordinates North => new TargetCoordinates(0, 1);
    public static TargetCoordinates East => new TargetCoordinates(1, 0);
    public static TargetCoordinates South => new TargetCoordinates(0, -1);
    public static TargetCoordinates West => new TargetCoordinates(-1, 0);
}