using System.Net;
using System.Threading;
using BattleshipsCore.Interfaces;
using BattleshipsCore.objects;

namespace BattleshipsGUI.ViewModels;

public class MainWindowViewModel : IUserInterface
{
    public IPlayer GetPlayer1()
    {
        throw new System.NotImplementedException();
    }

    public IPlayer GetPlayer2()
    {
        throw new System.NotImplementedException();
    }

    public void DrawTiles(Tile[,] tiles, string username)
    {
        throw new System.NotImplementedException();
    }

    public string GetUsername()
    {
        throw new System.NotImplementedException();
    }

    public bool GetYesNoAnswer(string question, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }

    public ShipPlacementInformation GetShipPlacementInformation(int shipLength, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }

    public void DisplayError(string message)
    {
        throw new System.NotImplementedException();
    }

    public void DisplayMessage(string message)
    {
        throw new System.NotImplementedException();
    }

    public TargetCoordinates GetTargetCoordinates(CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }

    public IPAddress GetIpAddress(CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }

    public string GetTargetGroupCode(CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }
}