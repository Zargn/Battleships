using Battleships.objects;

namespace Battleships;

public class BattleshipsAi
{
    private bool ShipTargeted { get; set; }
    
    public TargetCoordinates CalculateNextShot()
    {
        throw new NotImplementedException();
    }
    
    public void HandleShipSunkEvent(object? o, EventArgs e)
    {
        ShipTargeted = false;
    }
}