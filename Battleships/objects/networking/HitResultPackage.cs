using Unnamed_Networking_Plugin.Resources;

namespace Battleships.objects.networking;

public class HitResultPackage : Package
{
    public HitResult HitResult { get; init; }

    public HitResultPackage(HitResult hitResult)
    {
        HitResult = hitResult;
    }
}