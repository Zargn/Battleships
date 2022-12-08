﻿using Battleships.Interfaces;
using Battleships.objects;

namespace Battleships;

public class BattleshipsAi
{
    private int arenaXSize;
    private int arenaYSize;

    // This is far from needed but I felt like experimenting to learn more about the subject.
    private int xMult = 1;
    private int yMult = 1;
    
    private bool ShipTargeted { get; set; }
    private TargetCoordinates? ShipHitCoordinate { get; set; }
    private TargetCoordinates? ShipDirection { get; set; }
    private HashSet<int> shipHits;

    public BattleshipsAi(int arenaXSize, int arenaYSize)
    {
        this.arenaXSize = arenaXSize;
        this.arenaYSize = arenaYSize;
        
        if (arenaXSize > arenaYSize)
        {
            xMult = arenaYSize;
        }
        else
        {
            yMult = arenaXSize;
        }
        
        shipHits = new HashSet<int>(arenaXSize * arenaYSize);
    }
    
    // TODO: Probably not needed now that I already get the hitResult here.
    public void HandleShipSunkEvent(object? o, EventArgs e)
    {
        ShipTargeted = false;
    }
    
    
    
    public async Task<HitResult> PlayTurn(IPlayer targetPlayer, CancellationToken cancellationToken)
    {
        if (ShipTargeted)
        {
            return await FireTowardsShipArea(targetPlayer, cancellationToken);
        }
        else
        {
            return await FireAtRandomTile(targetPlayer, cancellationToken);
        }
    }
    
    

    private async Task<HitResult> FireAtRandomTile(IPlayer targetPlayer, CancellationToken cancellationToken)
    {
        while (true)
        {
            var targetLocation = GetRandomCoordinate();
            var targetTile = targetPlayer.KnownArenaTiles[targetLocation.X, targetLocation.Y];
            if (targetTile.Hit)
                continue;

            if (AdjacentToShip(targetLocation, targetPlayer.KnownArenaTiles))
                continue;

            var hitResult = await ShootAtTarget(targetPlayer, targetLocation, cancellationToken);

            if (hitResult.ShipHit)
            {
                ShipHitCoordinate = targetLocation;
            }

            return hitResult;
        }
    }


    private TargetCoordinates GetRandomCoordinate()
    {
        return new TargetCoordinates(Random.Shared.Next(arenaXSize), Random.Shared.Next(arenaYSize));
    }

    private bool AdjacentToShip(TargetCoordinates location, Tile[,] knownTiles)
    {
        var searchCoordinate = location - new TargetCoordinates(1, 1);

        for (var y = 0; y < 3; y++)
        {
            for (var x = 0; x < 3; x++)
            {
                if (!IsInArray(searchCoordinate))
                {
                    searchCoordinate.X++;
                    continue;
                }
                
                if (!knownTiles[searchCoordinate.X, searchCoordinate.Y].OccupiedByShip)
                {
                    searchCoordinate.X++;
                    continue;
                }
                
                if (!shipHits.Contains(GetCoordinateHash(searchCoordinate)))
                    return true;
                
                searchCoordinate.X++;
            }

            searchCoordinate.X -= 3;
            searchCoordinate.Y++;
        }

        return false;
    }

    private int GetCoordinateHash(TargetCoordinates value)
    {
        return value.X * xMult + value.Y * yMult;
    }

    private async Task<HitResult> ShootAtTarget(IPlayer targetPlayer, TargetCoordinates coordinates,
        CancellationToken cancellationToken)
    {
        return await targetPlayer.HitTile(coordinates, cancellationToken);
    }



    private async Task<HitResult> FireTowardsShipArea(IPlayer targetPlayer, CancellationToken cancellationToken)
    {
        if (ShipDirection == null)
        {
            return await FireToFindDirection(targetPlayer, cancellationToken);
        }
        
        return await FireBasedOnDirection(targetPlayer, cancellationToken);
    }

    private async Task<HitResult> FireToFindDirection(IPlayer targetPlayer, CancellationToken cancellationToken)
    {
        var directionIndex = Random.Shared.Next(4);
        
        for (var i = 0; i < 4; i++)
        {
            if (ShipHitCoordinate == null)
                throw new NullReferenceException("BattleshipsAi.ShipHitCoordinate was null but the ai still tried to use it.");
            
            var searchCoordinate = (TargetCoordinates) (ShipHitCoordinate + TargetCoordinates.Directions[directionIndex]);

            if (!IsInArray(searchCoordinate))
            {
                directionIndex = NextDirectionIndex(directionIndex);
                continue;
            }

            if (targetPlayer.KnownArenaTiles[searchCoordinate.X, searchCoordinate.Y].Hit)
            {
                directionIndex = NextDirectionIndex(directionIndex);
                continue;
            }

            if (AdjacentToShip(searchCoordinate, targetPlayer.KnownArenaTiles))
            { 
                directionIndex = NextDirectionIndex(directionIndex); 
                continue;
            }

            var hitResult = await targetPlayer.HitTile(searchCoordinate, cancellationToken);

            if (hitResult.ShipHit)
            {
                ShipDirection = TargetCoordinates.Directions[directionIndex];
            }

            return hitResult;
        }

        throw new Exception(
            "BattleshipsAi.FireToFindDirection: No available target was found around the last ship hit.");
    }

    private static int NextDirectionIndex(int directionIndex)
    {
        directionIndex++;
        if (directionIndex >= 4)
            directionIndex = 0;
        return directionIndex;
    }

    private async Task<HitResult> FireBasedOnDirection(IPlayer targetPlayer, CancellationToken cancellationToken)
    {
        
    }

    private TargetCoordinates GetNextValidCoordinateInShipDirection(IPlayer targetPlayer)
    {
        if (ShipHitCoordinate == null)
            throw new NullReferenceException("BattleshipsAi.ShipHitCoordinate was null but the ai still tried to use it.");
        
        if (ShipDirection == null)
            throw new NullReferenceException("BattleshipsAi.ShipDirection was null but the ai still tried to use it.");
        
        var searchCoordinate = (TargetCoordinates) ShipHitCoordinate;
        var searchDirection = (TargetCoordinates) ShipDirection;
        
        while (true)
        {
            searchCoordinate += searchDirection;

            if (!IsInArray(searchCoordinate))
                break;

            
        }
    }
    
    private bool IsInArray(TargetCoordinates c)
    {
        return c.X >= 0 && c.X < arenaXSize && c.Y >= 0 && c.Y < arenaYSize;
    }
}