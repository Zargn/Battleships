using Unnamed_Networking_Plugin.Resources;

namespace BattleshipsCore.objects.networking;

public class UserNamePackage : Package
{
    public string UserName { get; init; }

    public UserNamePackage(string userName)
    {
        UserName = userName;
    }
}