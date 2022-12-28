using Battleships.objects;
using Battleships.objects.Exceptions;



namespace Battleships;

public class Arena
{
    private readonly Tile[,] tiles;
    private readonly List<Ship> ships = new();

    public int XSize => tiles.GetLength(0);
    public int YSize => tiles.GetLength(1);
    public Tile[,] CurrentView { get; }
    public Tile[,] CompleteView => tiles;

    public event EventHandler? ShipSunk;    

    
    
    public Arena(int xSize, int ySize)
    {
        tiles = new Tile[xSize, ySize];
        CurrentView = new Tile[xSize, ySize];
    }
    
    

    public Tile this[TargetCoordinates targetCoordinates]
    {
        get => tiles[targetCoordinates.X, targetCoordinates.Y];
        set => tiles[targetCoordinates.X, targetCoordinates.Y] = value;
    }

    public Tile[,] RandomiseShipLocations(int[] shipLengths)
    {
        foreach (var shipLength in shipLengths)
        {
            var ship = new Ship(shipLength, "Ai");

            FindLocationAndPlaceShip(ship);
        }

        return tiles;
    }

    private void FindLocationAndPlaceShip(Ship ship)
    {
        while (true)
        {
            var targetCoordinates = GetRandomTargetCoordinates();

            if (AttemptAutoPlaceShip(ship, targetCoordinates))
            {
                return;
            }
        }
    }

    private TargetCoordinates GetRandomTargetCoordinates()
    {
        while (true)
        {
            var x = Random.Shared.Next(XSize);
            var y = Random.Shared.Next(YSize);

            if (!tiles[x, y].OccupiedByShip)
                return new TargetCoordinates(x, y);
        }
    }

    private bool AttemptAutoPlaceShip(Ship ship, TargetCoordinates targetCoordinates)
    {
        var startDirectionIndex = Random.Shared.Next(4);
        var directions = TargetCoordinates.Directions;

        for (int i = 0; i < 4; i++)
        {
            try
            {
                PlaceShip(ship, targetCoordinates, directions[startDirectionIndex]);
                return true;
            }
            catch (LocationUnavailableException)
            {
            }
        }

        return false;
    }
    
    /// <summary>
    /// Attempt to place the provided ship at the provided coordinates and direction.
    /// </summary>
    /// <returns>Full Tile array for user review.</returns>
    /// <exception cref="LocationUnavailableException">Location provided does not have enough space for provided ship.</exception>
    public Tile[,] PlaceShip(Ship ship, TargetCoordinates targetCoordinates, TargetCoordinates direction)
    {
        // TODO: Instead of throwing an exception, maybe return if successful or not?

        if (!IsInArray(targetCoordinates))
            throw new LocationUnavailableException("Provided coordinates was outside the map");
        
        if (!IsInArray(targetCoordinates + direction*(ship.Length - 1)))
            throw new LocationUnavailableException("Provided direction and ship size resulted in ship being unable to fit inside the map.");

        if (SurroundingAreaContainsShips(ship.Length, targetCoordinates, direction))
            throw new LocationUnavailableException("Selected location had ships in too close proximity.");

        SetShipTilesAndCoordinates(ship, targetCoordinates, direction);

        ships.Add(ship);
        
        return tiles;
    }

    private bool IsInArray(TargetCoordinates c)
    {
        return c.X >= 0 && c.X < XSize && c.Y >= 0 && c.Y < YSize;
    }

    private bool SurroundingAreaContainsShips(int length, TargetCoordinates startCoordinates, TargetCoordinates direction)
    {
        // This might seem weird since it will not give the same side of the direction, but it doesn't really matter in this case.
        var sideDirection = new TargetCoordinates(direction.Y, direction.X);

        var startSearchCords = startCoordinates - direction - sideDirection;

        for (var shortSide = 0; shortSide < 3; shortSide++)
        {
            var searchCoords = startSearchCords;
            for (var longSide = 0; longSide < length + 2; longSide++)
            {
                if (IsInArray(searchCoords))
                {
                    if (tiles[searchCoords.X, searchCoords.Y].OccupiedByShip)
                        return true;
                }

                searchCoords += direction;
            }

            startSearchCords += sideDirection;
        }

        return false;
    }

    private void SetShipTilesAndCoordinates(Ship ship, TargetCoordinates startCoordinates, TargetCoordinates direction)
    {
        var setCoordinate = startCoordinates;
        for (var i = 0; i < ship.Length; i++)
        {
            tiles[setCoordinate.X, setCoordinate.Y].OccupiedByShip = true;
            ship.CoordinatesArray[i] = setCoordinate;
            setCoordinate += direction;
        }
    }
    
    /// <summary>
    /// Fires at the selected coordinates and updates ships hit.
    /// </summary>
    /// <param name="targetCoordinates"></param>
    /// <returns>HitResult containing if a ship was hit and in that case which ship.</returns>
    /// <exception cref="LocationUnavailableException">If the provided target was invalid.</exception>
    public HitResult FireAtTile(TargetCoordinates targetCoordinates)
    {
        if (!IsInArray(targetCoordinates))
            throw new LocationUnavailableException("Provided coordinates was outside the map.");

        var tile = this[targetCoordinates];

        if (tile.Hit)
            throw new LocationUnavailableException("Provided coordinates has been hit already!");
        
        tile.Hit = true;
        CurrentView[targetCoordinates.X, targetCoordinates.Y].Hit = true;
        this[targetCoordinates] = tile;

        if (!tile.OccupiedByShip) return new HitResult(false, null);
        
        CurrentView[targetCoordinates.X, targetCoordinates.Y].OccupiedByShip = true;
        var ship = GetShipAtCoordinates(targetCoordinates);
        ship.Health--;
        
        if (ship.ShipSunk)
            ShipSunk?.Invoke(this, EventArgs.Empty);
        
        return new HitResult(true, ship);
    }

    private Ship? GetShipAtCoordinates(TargetCoordinates targetCoordinates)
    {
        foreach (var ship in from ship in ships from shipCoordinate in ship.CoordinatesArray where targetCoordinates == shipCoordinate select ship)
        {
            return ship;
        }

        throw new Exception("A ship with the hit coordinates was not found.");
    }
}