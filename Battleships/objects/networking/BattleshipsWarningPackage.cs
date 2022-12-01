using Unnamed_Networking_Plugin.Resources;

namespace Battleships.objects.networking;

public class BattleshipsWarningPackage : Package
{
    public string Message { get; }

    public BattleshipsWarningPackage(string message)
    {
        Message = message;
    }
}