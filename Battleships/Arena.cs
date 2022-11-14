using Battleships.objects;

namespace Battleships;

public class Arena
{
    private Tile[,] tiles;

    public int XSize => tiles.GetLength(0);
    public int YSize => tiles.GetLength(1);
    
    public Arena(int xSize, int ySize)
    {
        tiles = new Tile[xSize, ySize];
    }

    // Fires at the target x and y coordinates.
    public bool FireAtTile(int x, int y)
    {
        tiles[x, y].Hit = true;
        return tiles[x, y].OccupiedByShip;
    }
}