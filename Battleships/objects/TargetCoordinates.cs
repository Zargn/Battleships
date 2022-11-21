namespace Battleships.objects;

public struct TargetCoordinates
{
    public int X;
    public int Y;

    public TargetCoordinates(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static TargetCoordinates North => new TargetCoordinates(0, 1);
    public static TargetCoordinates East => new TargetCoordinates(1, 0);
    public static TargetCoordinates South => new TargetCoordinates(0, -1);
    public static TargetCoordinates West => new TargetCoordinates(-1, 0);

    public static TargetCoordinates operator +(TargetCoordinates a, TargetCoordinates b) =>
        new TargetCoordinates(a.X + b.X, a.Y + b.Y);
    public static TargetCoordinates operator -(TargetCoordinates a, TargetCoordinates b) =>
        new TargetCoordinates(a.X - b.X, a.Y - b.Y);
    
    public static TargetCoordinates operator *(TargetCoordinates a, int i) =>
        new TargetCoordinates(a.X * i, a.Y * i);
}