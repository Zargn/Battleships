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



    public StartingPlayer PlayerStartPriority => StartingPlayer.Maybe;
    public string UserName { get; private set; }

    public Tile[,] KnownArenaTiles => arena.CurrentView;
    public int ShipsLeft { get; set; }

    
    
    public LocalPlayer(IUserInterface userInterface)
    {
        this.userInterface = userInterface;
    }
    
    

    public async Task InitializePlayer(int[] shipLengths, int xSize, int ySize, CancellationToken cancellationToken)
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
        userInterface.DrawTiles(arena.CurrentView);
        
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
    
    

    public async Task<TurnResult> PlayTurnAsync(IPlayer target, CancellationToken cancellationToken)
    {
        userInterface.DrawTiles(target.KnownArenaTiles);
        
        var firingTarget = GetFiringTarget(target.KnownArenaTiles, cancellationToken);

        var hitResult = await FireAtOtherPlayer(firingTarget, target, cancellationToken);

        var turnResult = GetTurnResult(hitResult, target);

        if (turnResult.ShipHitDEPRECATED)
            userInterface.DisplayMessage("Ship hit!");
        if (turnResult.ShipSunkDEPRECATED)
            userInterface.DisplayMessage("Ship sunk!");
        if (turnResult.TargetPlayerDefeated)
            userInterface.DisplayMessage("Player defeated!");

        userInterface.DrawTiles(target.KnownArenaTiles);
        
        return turnResult;
    }
    
    private TargetCoordinates GetFiringTarget(Tile[,] enemyTiles, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var target = userInterface.GetTargetCoordinates();
            if (enemyTiles.GetLength(0) <= target.X || enemyTiles.GetLength(1) <= target.Y)
            {
                userInterface.DisplayError("Target was out of bounds. Try again.");
                continue;
            }

            if (enemyTiles[target.X, target.Y].Hit)
            {
                userInterface.DisplayError("Target has been hit before. Try again.");
                continue;
            }

            return target;
        }

        throw new OperationCanceledException();
    }

    private async Task<HitResult> FireAtOtherPlayer(TargetCoordinates targetCoordinates, IPlayer target, CancellationToken cancellationToken)
    {
        return await target.HitTile(targetCoordinates, cancellationToken);
    }

    private TurnResult GetTurnResult(HitResult hitResult, IPlayer target)
    {
        // TODO: Does this work?
        var shipSunk = hitResult.Ship?.Health <= 0;

        return new TurnResult(hitResult.shipHit, shipSunk, target.PlayerDefeated);
    }
    
    

    public Task<HitResult> HitTile(TargetCoordinates targetCoordinates, CancellationToken cancellationToken)
    {
        return Task.FromResult(arena.FireAtTile(targetCoordinates));
    }
    
    

    public Task UnloadPlayer(EndOfGameStatistics endOfGameStatistics)
    {
        throw new NotImplementedException();
    }
    
    

    public event EventHandler<PlayerDefeatedEventArgs>? PlayerDefeatedDEPRECATED;
    public event EventHandler<ShipSunkEventArgs>? ShipSunkDEPRECATED;
}