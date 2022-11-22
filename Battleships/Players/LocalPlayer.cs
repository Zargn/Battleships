using Battleships.EventArguments;
using Battleships.Interfaces;
using Battleships.objects;
using Battleships.objects.Enums;
using Battleships.objects.Exceptions;

namespace Battleships.Players;

public class LocalPlayer : IPlayer
{
    private Arena arena;
    private StartingPlayer playerStartPriority;
    private IUserInterface userInterface;

    public LocalPlayer(IUserInterface userInterface)
    {
        this.userInterface = userInterface;
    }

    public Task InitializePlayer(int[] shipLengths, int xSize, int ySize, CancellationToken cancellationToken)
    {
        UserName = userInterface.GetUsername();

        if (userInterface.GetYesNoAnswer("Do you want to place your ships manually?"))
        {
            PlaceShipsManual(shipLengths, xSize, ySize, cancellationToken);
        }
        else
        {
            PlaceShipsAuto(shipLengths, xSize, ySize, cancellationToken);
        }
        
        this.arena = arena;
        throw new NotImplementedException();
    }

    private void PlaceShipsManual(int[] shipLengths, int xSize, int ySize, CancellationToken cancellationToken)
    {
        arena = new Arena(xSize, ySize);
        
        while (!cancellationToken.IsCancellationRequested)
        {
            foreach (var i in shipLengths)
            {
                PlaceShip(i, cancellationToken);
            }
        }
    }

    private void PlaceShip(int shipLength, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var placementInformation = userInterface.GetShipPlacementInformation(shipLength);
            var ship = new Ship(shipLength, placementInformation.Name);
            try
            {
                var tiles = arena.PlaceShip(ship, placementInformation.Coordinates, placementInformation.Direction);
                userInterface.DrawTiles(tiles);
                return;
            }
            catch (LocationUnavailableException)
            {
                userInterface.DisplayError("Selected location was unavailable.");
            }
        }
    }

    private void PlaceShipsAuto(int[] shipLengths, int xSize, int ySize, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            arena = new Arena(xSize, ySize);
            var tiles = arena.RandomiseShipLocations(shipLengths);
            
            userInterface.DrawTiles(tiles);
            if (!userInterface.GetYesNoAnswer("Randomise again?"))
                return;
        }
    }

    public StartingPlayer PlayerStartPriority { get; private set; }
    public string UserName { get; private set; }

    public Tile[,] KnownArenaTiles => arena.CurrentView;

    public Task<TurnResult> PlayTurnAsync(IPlayer target, CancellationToken cancellationToken)
    {
        var firingTarget = GetFiringTarget();

        var fireAtPlayerTask = FireAtOtherPlayer(firingTarget);
        
        throw new NotImplementedException();
    }

    public Task<HitResult> HitTile(TargetCoordinates targetCoordinates)
    {
        return Task.FromResult(arena.FireAtTile(targetCoordinates));
        
        throw new NotImplementedException();
    }

    public Task UnloadPlayer(EndOfGameStatistics endOfGameStatistics)
    {
        throw new NotImplementedException();
    }

    public event EventHandler<PlayerDefeatedEventArgs>? PlayerDefeatedDEPRECATED;
    public event EventHandler<ShipSunkEventArgs>? ShipSunkDEPRECATED;

    
    
    private TargetCoordinates GetFiringTarget()
    {
        throw new NotImplementedException();
    }

    private Task FireAtOtherPlayer(TargetCoordinates targetCoordinates)
    {
        throw new NotImplementedException();
    }
}