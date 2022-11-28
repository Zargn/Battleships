namespace Battleships.objects.networking;

public class HitTilePackage
{
    public TargetCoordinates TargetCoordinates { get; init; }

    public HitTilePackage(TargetCoordinates targetCoordinates)
    {
        TargetCoordinates = targetCoordinates;
    }
}