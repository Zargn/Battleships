namespace Battleships.objects.networking;

public class HitResultPackage
{
    public HitResult HitResult { get; init; }

    public HitResultPackage(HitResult hitResult)
    {
        HitResult = hitResult;
    }
}