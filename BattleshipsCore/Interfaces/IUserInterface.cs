using System.Net;
using BattleshipsCore.objects;

namespace BattleshipsCore.Interfaces;

public interface IUserInterface
{
    public Task<IPlayer> GetPlayer1();
    public Task<IPlayer> GetPlayer2();

    // public void DrawArenas(IPlayer player1);
    public void DrawTiles(Tile[,] tiles, string username);

    public Task<string> GetUsername();

    public Task<bool> GetYesNoAnswer(string question, CancellationToken cancellationToken);

    public Task<ShipPlacementInformation> GetShipPlacementInformation(int shipLength, CancellationToken cancellationToken);
    public void DisplayError(string message);
    public void DisplayMessage(string message);

    public Task<TargetCoordinates> GetTargetCoordinates(CancellationToken cancellationToken);

    public Task<IPAddress> GetIpAddress(CancellationToken cancellationToken);
    public Task<string> GetTargetGroupCode(CancellationToken cancellationToken);
}