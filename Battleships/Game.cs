using Battleships.EventArguments;
using Battleships.Interfaces;
using Battleships.objects;

namespace Battleships;

public class Game
{
    public static async Task Main()
    {
        var userInterface = new ConsoleUserInterface();
        
        var game = new Game(userInterface);
        while (true)
        {
            await game.Run();

            if (!userInterface.GetYesNoAnswer("Play again?", CancellationToken.None))
            {
                break;
            }
        }
    }

    // private static readonly int[] ShipLengths = {2, 2, 3, 4, 5};
    private static readonly int[] ShipLengths = {2};
    private const int ArenaSizeX = 3;
    private const int ArenaSizeY = 3;
    
    private CancellationTokenSource cancelSource = new CancellationTokenSource();

    private IUserInterface userInterface;

    public Game(IUserInterface userInterface)
    {
        this.userInterface = userInterface;
    }

    public async Task Run()
    { 
        var gameLoopTask = InitializeGame();
        await gameLoopTask;
    }

    public void Stop(object? o, PlayerUnavailableEventArgs args)
    {
        userInterface.DisplayMessage($"Game ended early due to: {args.Reason}");
        cancelSource.Cancel();
    }

    private async Task InitializeGame()
    {
        CancellationTokenSource initializationCancelSource = new CancellationTokenSource();
        
        var player1 = userInterface.GetPlayer1();
        await player1.InitializePlayer(ShipLengths, ArenaSizeX, ArenaSizeY, initializationCancelSource.Token);
        player1.PlayerUnavailable += Stop;

        var player2 = userInterface.GetPlayer2();
        await player2.InitializePlayer(ShipLengths, ArenaSizeX, ArenaSizeY, initializationCancelSource.Token);
        player2.PlayerUnavailable += Stop;

        await GameLoop(player1, player2);
    }

    private async Task GameLoop(IPlayer player1, IPlayer player2)
    {
        IPlayer[] players = ConfigurePlayerOrder(player1, player2);

        bool startingPlayerTurn = true;

        UpdateVisualPlayingField(new []{player1, player2});
        
        userInterface.DisplayMessage($"{GetIPlayer(players, startingPlayerTurn).UserName} starts!");

        while (true)
        {
            try
            {
                var player = GetIPlayer(players, startingPlayerTurn);
                
                userInterface.DisplayMessage($"{player.UserName}'s turn!");
                
                var turnResult = await player.PlayTurnAsync(GetIPlayer(players, !startingPlayerTurn), cancelSource.Token);
                
                UpdateVisualPlayingField(new []{player1, player2});

                DisplayTurnResult(turnResult, player);

                if (turnResult.TargetPlayerDefeated)
                {
                    userInterface.DisplayMessage($"{player.UserName} wins!");
                    break;
                }
            
                startingPlayerTurn = !startingPlayerTurn;
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }
        
        // TODO: Unload players.
    }

    private IPlayer[] ConfigurePlayerOrder(IPlayer player1, IPlayer player2)
    {
        var players = new IPlayer[2];

        if (player1.PlayerStartPriority > player2.PlayerStartPriority)
        {
            players = new []{player1, player2};
        }

        if (player1.PlayerStartPriority < player2.PlayerStartPriority)
        {
            players = new []{player2, player1};
        }

        if (player1.PlayerStartPriority == player2.PlayerStartPriority)
        {
            players = Random.Shared.Next(2) == 1 ? new []{player2, player1} : new []{player1, player2};
        }

        return players;
    }

    private static IPlayer GetIPlayer(IPlayer[] players, bool startingPlayer)
    {
        return startingPlayer ? players[0] : players[1];
    }

    private void UpdateVisualPlayingField(IEnumerable<IPlayer> players)
    {
        foreach (var player in players)
        {
            userInterface.DrawTiles(player.KnownArenaTiles, player.UserName);
        }
    }

    private void DisplayTurnResult(TurnResult turnResult, IPlayer player)
    {
        if (turnResult.ShipSunk)
            userInterface.DisplayMessage($"{player.UserName} sunk the {turnResult.Ship.Type} {turnResult.Ship.Name}!");
        else if (turnResult.ShipHit)
            userInterface.DisplayMessage($"{player.UserName} hit a ship!");
        else
            userInterface.DisplayMessage($"{player.UserName} missed!");
    }
}