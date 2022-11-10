using Battleships.objects;

namespace Battleships;

public class Arena
{
    private Tile[,] tiles;

    public Arena(int xSize, int ySize)
    {
        tiles = new Tile[xSize, ySize];
    }
}