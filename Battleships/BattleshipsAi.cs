using Battleships.Interfaces;
using Battleships.objects;

namespace Battleships;

public class BattleshipsAi
{
    private int arenaXSize;
    private int arenaYSize;
    
    private bool ShipTargeted { get; set; }
    private HashSet<int> shipHits = new HashSet<int>();

    public BattleshipsAi(int arenaXSize, int arenaYSize)
    {
        this.arenaXSize = arenaXSize;
        this.arenaYSize = arenaYSize;
    }
    
    // TODO: Probably not needed now that I already get the hitResult here.
    public void HandleShipSunkEvent(object? o, EventArgs e)
    {
        ShipTargeted = false;
    }
    
    
    
    public async Task<HitResult> PlayTurn(IPlayer targetPlayer, CancellationToken cancellationToken)
    {
        if (ShipTargeted)
        {
            
        }
        else
        {
            return FireAtRandomTile(targetPlayer, cancellationToken);
        }
    }

    private HitResult FireAtRandomTile(IPlayer targetPlayer, CancellationToken cancellationToken)
    {
        while (true)
        {
            var targetLocation = GetRandomCoordinate();
            var targetTile = targetPlayer.KnownArenaTiles[targetLocation.X, targetLocation.Y];
            if (targetTile.Hit)
                continue;
            
            if ()
        }
    }


    private TargetCoordinates GetRandomCoordinate()
    {
        return new TargetCoordinates(Random.Shared.Next(arenaXSize), Random.Shared.Next(arenaYSize));
    }
    
    private bool AdjacentToShip()
}