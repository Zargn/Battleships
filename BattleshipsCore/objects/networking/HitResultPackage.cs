using Unnamed_Networking_Plugin.Resources;

namespace BattleshipsCore.objects.networking;

public class HitResultPackage : Package
{
    public HitResult HitResult { get; init; }

    public HitResultPackage(HitResult hitResult)
    {
        HitResult = hitResult;
    }
}