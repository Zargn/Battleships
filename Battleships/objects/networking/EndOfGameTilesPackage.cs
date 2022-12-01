using Unnamed_Networking_Plugin.Resources;

namespace Battleships.objects.networking;

public class EndOfGameTilesPackage : Package
{
    public Tile[,] Tiles { get; init; }

    public EndOfGameTilesPackage(Tile[,] tiles)
    {
        Tiles = tiles;
    }
}