﻿using Unnamed_Networking_Plugin.Resources;

namespace BattleshipsCore.objects.networking;

public class ShipLocationsPackage : Package
{
    public TargetCoordinates[] Locations { get; init; }

    public ShipLocationsPackage(TargetCoordinates[] locations)
    {
        this.Locations = locations;
    }
}