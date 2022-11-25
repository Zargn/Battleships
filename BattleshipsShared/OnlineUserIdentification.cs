using Unnamed_Networking_Plugin.Interfaces;
using Unnamed_Networking_Plugin.Resources;

namespace Battleships.objects.networking;

public class OnlineUserIdentificationPackage : IdentificationPackage
{
    public OnlineUserIdentification OnlineUserIdentification { get; }

    public OnlineUserIdentificationPackage(OnlineUserIdentification onlineUserIdentification)
    {
        OnlineUserIdentification = onlineUserIdentification;
    }
    
    public override IConnectionInformation ExtractConnectionInformation()
    {
        return OnlineUserIdentification;
    }
}

public class OnlineUserIdentification : IConnectionInformation
{ 
    public string UserName { get; init; }
    
    public OnlineUserIdentification(string userName)
    {
        UserName = userName;
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is OnlineUserIdentification)
            return (obj as OnlineUserIdentification).UserName == UserName;
        return false;
    }
    
    public override string ToString()
    {
        return UserName;
    }
    
    public override int GetHashCode()
    {
        return UserName.GetHashCode();
    }
}