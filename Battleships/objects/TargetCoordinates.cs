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

    public static TargetCoordinates North => new(0, 1);
    public static TargetCoordinates East => new(1, 0);
    public static TargetCoordinates South => new(0, -1);
    public static TargetCoordinates West => new(-1, 0);
    public static readonly TargetCoordinates[] Directions = {North, East, South, West};

    public static TargetCoordinates operator +(TargetCoordinates a, TargetCoordinates b) => new(a.X + b.X, a.Y + b.Y);
    public static TargetCoordinates operator -(TargetCoordinates a, TargetCoordinates b) => new(a.X - b.X, a.Y - b.Y);
    public static TargetCoordinates operator *(TargetCoordinates a, int i) => new(a.X * i, a.Y * i);
    public static bool operator ==(TargetCoordinates a, TargetCoordinates b) => a.X == b.X && a.Y == b.Y;
    public static bool operator !=(TargetCoordinates a, TargetCoordinates b) => !(a == b);

    public override string ToString()
    {
        return $"(x: {X}, y: {Y})";
    }

    /// <summary>
    /// Get a TargetCoordinates from the supplied string. String should be in format: "number number"
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    /// <exception cref="FormatException"></exception>
    public static TargetCoordinates FromString(string s)
    {
        if (s == null)
            throw new FormatException();
        
        string? xString = null;
        string? yString = null;

        var startIndex = 0;
        
        // No need to check the first letter for a space since in a valid input the first letter will always be a number.
        for (var i = 1; i < s.Length; i++)
        {
            if (s[i] != ' ' || xString != null) continue;
            
            xString = s.Substring(startIndex, i);
            startIndex = i + 1;
        }

        yString = s.Substring(startIndex, s.Length - startIndex);

        if (xString == null)
            throw new FormatException();

        var x = int.Parse(xString);
        var y = int.Parse(yString);

        return new TargetCoordinates(x, y);
    }
}