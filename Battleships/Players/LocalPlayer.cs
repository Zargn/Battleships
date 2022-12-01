using Battleships.EventArguments;
using Battleships.Interfaces;
using Battleships.objects;
using Battleships.objects.Enums;
using Battleships.objects.Exceptions;

namespace Battleships.Players;

public class LocalPlayer : IPlayer
{
    private Arena? arena;
    private readonly IUserInterface userInterface;



    public StartingPlayer PlayerStartPriority => StartingPlayer.Maybe;
    public string UserName { get; private set; }

    public Tile[,] KnownArenaTiles => arena.CompleteView;
    public int ShipsLeft { get; private set; }

    
    
    public LocalPlayer(IUserInterface userInterface)
    {
        this.userInterface = userInterface;
    }
    
    

    public async Task InitializePlayer(int[] shipLengths, int xSize, int ySize, CancellationToken cancellationToken)
    {
        UserName = userInterface.GetUsername();

        if (userInterface.GetYesNoAnswer("Do you want to place your ships manually?", cancellationToken))
        {
            PlaceShipsManual(shipLengths, xSize, ySize, cancellationToken);
        }
        else
        {
            PlaceShipsAuto(shipLengths, xSize, ySize, cancellationToken);
        }

        ShipsLeft = shipLengths.Length;
        arena.ShipSunk += HandleShipSunkEvent;
    }

    private void HandleShipSunkEvent(object? o, EventArgs e)
    {
        ShipsLeft--;
    }

    private void PlaceShipsManual(int[] shipLengths, int xSize, int ySize, CancellationToken cancellationToken)
    {
        arena = new Arena(xSize, ySize);
        userInterface.DrawTiles(arena.CurrentView, UserName);
        
        foreach (var i in shipLengths)
        {
            PlaceShip(i, cancellationToken);

            if (cancellationToken.IsCancellationRequested)
                throw new OperationCanceledException();
        }
    }

    private void PlaceShip(int shipLength, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var placementInformation = userInterface.GetShipPlacementInformation(shipLength, cancellationToken);
            var ship = new Ship(shipLength, placementInformation.Name);
            try
            {
                var tiles = arena.PlaceShip(ship, placementInformation.Coordinates, placementInformation.Direction);
                userInterface.DrawTiles(tiles, UserName);
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
            
            userInterface.DrawTiles(tiles, UserName);
            if (!userInterface.GetYesNoAnswer("Randomise again?", cancellationToken))
                return;
        }
    }
    
    

    public async Task<TurnResult> PlayTurnAsync(IPlayer target, CancellationToken cancellationToken)
    {
        var firingTarget = GetFiringTarget(target.KnownArenaTiles, cancellationToken);
        
        var hitResult = await target.HitTile(firingTarget, cancellationToken);

        var turnResult = GetTurnResult(hitResult, target);
        
        return turnResult;
    }
    
    private TargetCoordinates GetFiringTarget(Tile[,] enemyTiles, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var target = userInterface.GetTargetCoordinates(cancellationToken);
            if (enemyTiles.GetLength(0) <= target.X || enemyTiles.GetLength(1) <= target.Y)
            {
                userInterface.DisplayError("Target was out of bounds. Try again.");
                continue;
            }

            if (!enemyTiles[target.X, target.Y].Hit) return target;
            
            userInterface.DisplayError("Target has been hit before. Try again.");
        }

        throw new OperationCanceledException();
    }

    private static TurnResult GetTurnResult(HitResult hitResult, IPlayer target)
    {
        var shipSunk = hitResult.Ship?.Health <= 0;

        return new TurnResult(hitResult.ShipHit, shipSunk, target.PlayerDefeated, hitResult.Ship);
    }
    
    

    public Task<HitResult> HitTile(TargetCoordinates targetCoordinates, CancellationToken cancellationToken)
    {
        return Task.FromResult(arena.FireAtTile(targetCoordinates));
    }
    
    

    public Task UnloadPlayer(EndOfGameStatistics endOfGameStatistics)
    {
        throw new NotImplementedException();
    }

    public event EventHandler<PlayerUnavailableEventArgs>? PlayerUnavailable;
    public event EventHandler<PlayerDefeatedEventArgs>? PlayerDefeatedDEPRECATED;
    public event EventHandler<ShipSunkEventArgs>? ShipSunkDEPRECATED;
}