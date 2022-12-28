using Unnamed_Networking_Plugin.Resources;

namespace BattleshipsCore.objects.networking;

public class HitTilePackage : Package
{
    public TargetCoordinates TargetCoordinates { get; init; }

    public HitTilePackage(TargetCoordinates targetCoordinates)
    {
        TargetCoordinates = targetCoordinates;
    }
}