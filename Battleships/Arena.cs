using System.Numerics;
using System.Xml.Xsl;
using Battleships.objects;
using Battleships.objects.Exceptions;

namespace Battleships;

public class Arena
{
    private Tile[,] tiles;
    private Tile[,] outsideViewTiles;

    public int XSize => tiles.GetLength(0);
    public int YSize => tiles.GetLength(1);

    public Tile[,] CurrentView => outsideViewTiles;

    public Arena(int xSize, int ySize)
    {
        tiles = new Tile[xSize, ySize];
    }

    /// <summary>
    /// Attempt to place the provided ship at the provided coordinates and direction.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="LocationUnavailableException">Location provided does not have enough space for provided ship.</exception>
    public Tile[,] PlaceShip(Ship ship, TargetCoordinates targetCoordinates, TargetCoordinates direction)
    {
        if (!IsInArray(targetCoordinates))
            throw new LocationUnavailableException("Provided coordinates was outside the map");
        
        if (!IsInArray(targetCoordinates + direction*ship.Length))
            throw new LocationUnavailableException("Provided direction and ship size resulted in ship being unable to fit inside the map.");

        if (!SurroundingAreaContainsShips(ship.Length, targetCoordinates, direction))
            throw new LocationUnavailableException("Selected location had ships in too close proximity.");
        
        
        
        var sideDir = new TargetCoordinates(direction.Y, direction.X);
        
        
        
        return tiles;
    }

    private bool IsInArray(TargetCoordinates c)
    {
        return (0 <= c.X && c.X < XSize && 0 <= c.Y && c.Y < YSize);
    }

    private bool SurroundingAreaContainsShips(int length, TargetCoordinates startCoordinates, TargetCoordinates direction)
    {
        // This might seem weird since it will not give the same side of the direction, but it doesn't really matter in this case.
        TargetCoordinates sideDirection = new TargetCoordinates(direction.Y, direction.X);

        TargetCoordinates searchCords = startCoordinates - direction - sideDirection;

        for (int shortSide = 0; shortSide < 3; shortSide++)
        {
            for (int longSide = 0; longSide < length + 2; longSide++)
            {
                if (IsInArray(searchCords))
                {
                    if (tiles[searchCords.X, searchCords.Y].OccupiedByShip)
                        return false;
                }

                searchCords += direction;
            }

            searchCords += sideDirection;
        }

        return true;
    }
    
    // Fires at the target x and y coordinates.
    public bool FireAtTile(int x, int y)
    {
        tiles[x, y].Hit = true;
        outsideViewTiles[x, y].Hit = true;
        if (tiles[x, y].OccupiedByShip)
            outsideViewTiles[x, y].OccupiedByShip = true;
        
        return tiles[x, y].OccupiedByShip;
    }

    public bool FireAtTile(TargetCoordinates targetCoordinates)
    {
        return FireAtTile(targetCoordinates.X, targetCoordinates.Y);
    }
}