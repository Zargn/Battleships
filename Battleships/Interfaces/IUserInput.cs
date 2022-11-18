using System.Net;

namespace Battleships.Interfaces;

public interface IUserInput
{
    public IPlayer GetPlayer1();
    public IPlayer GetPlayer2();
    
}