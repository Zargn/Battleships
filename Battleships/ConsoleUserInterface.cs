using Battleships.Interfaces;
using Battleships.objects;

namespace Battleships;

public class ConsoleUserInterface : IUserInterface
{
    public IPlayer GetPlayer1()
    {
        throw new NotImplementedException();
    }

    public IPlayer GetPlayer2()
    {
        throw new NotImplementedException();
    }

    public void DrawTiles(Tile[,] tiles)
    {
        throw new NotImplementedException();
    }

    public string GetUsername()
    {
        throw new NotImplementedException();
    }

    public bool GetYesNoAnswer(string question)
    {
        throw new NotImplementedException();
    }

    public ShipPlacementInformation GetShipPlacementInformation(int shipLength)
    {
        throw new NotImplementedException();
    }

    public void DisplayError(string message)
    {
        throw new NotImplementedException();
    }
}