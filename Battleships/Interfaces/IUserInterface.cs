using System.Net;

namespace Battleships.Interfaces;

public interface IUserInterface
{
    public IPlayer GetPlayer1();
    public IPlayer GetPlayer2();

    public void DrawArenas(IPlayer player1);
}