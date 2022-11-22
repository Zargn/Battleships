using System.Net;
using Battleships.objects;

namespace Battleships.Interfaces;

public interface IUserInterface
{
    public IPlayer GetPlayer1();
    public IPlayer GetPlayer2();

    public void DrawArenas(IPlayer player1);

    public string GetUsername();

    public bool GetYesNoAnswer(string question);

    public ShipPlacementInformation GetShipPlacementInformation(int shipLength);
}