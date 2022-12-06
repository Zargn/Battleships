using Battleships.Interfaces;
using Battleships.objects;

namespace Battleships;

public class BattleshipsAi
{
    private int arenaXSize;
    private int arenaYSize;

    // This is far from needed but I felt like experimenting to learn more about the subject.
    private int xMult = 1;
    private int yMult = 1;
    
    private bool ShipTargeted { get; set; }
    private HashSet<int> shipHits;

    public BattleshipsAi(int arenaXSize, int arenaYSize)
    {
        this.arenaXSize = arenaXSize;
        this.arenaYSize = arenaYSize;
        
        if (arenaXSize > arenaYSize)
        {
            xMult = arenaYSize;
        }
        else
        {
            yMult = arenaXSize;
        }
        
        shipHits = new HashSet<int>(arenaXSize * arenaYSize);
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

            if (AdjacentToShip(targetLocation, targetPlayer.KnownArenaTiles))
                continue;
            
            
        }
    }


    private TargetCoordinates GetRandomCoordinate()
    {
        return new TargetCoordinates(Random.Shared.Next(arenaXSize), Random.Shared.Next(arenaYSize));
    }

    private bool AdjacentToShip(TargetCoordinates location, Tile[,] knownTiles)
    {
        var searchCoordinate = location - new TargetCoordinates(1, 1);

        for (var y = 0; y < 3; y++)
        {
            for (var x = 0; x < 3; x++)
            {
                searchCoordinate.X++;
                if (!knownTiles[searchCoordinate.X, searchCoordinate.Y].OccupiedByShip) 
                    continue;
                
                if (!shipHits.Contains(GetCoordinateHash(searchCoordinate)))
                    return true;
            }
        }

        return false;
    }

    private int GetCoordinateHash(TargetCoordinates value)
    {
        return value.X * xMult + value.Y * yMult;
    }
}