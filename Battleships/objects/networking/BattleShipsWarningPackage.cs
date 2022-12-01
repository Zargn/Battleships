using Unnamed_Networking_Plugin.Resources;

namespace Battleships.objects.networking;

public class BattleShipsWarningPackage : Package
{
    public string Message { get; }

    public BattleShipsWarningPackage(string message)
    {
        Message = message;
    }
}