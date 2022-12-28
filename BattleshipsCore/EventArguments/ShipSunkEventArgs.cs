namespace Battleships.EventArguments;

public class ShipSunkEventArgs
{
    public int ShipSize { get; }
    
    public ShipSunkEventArgs(int shipSize)
    {
        ShipSize = shipSize;
    }
}