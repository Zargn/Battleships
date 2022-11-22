using System.Numerics;
using Battleships.Interfaces;
using Battleships.objects.Enums;

namespace Battleships;

public class Game
{
    public static async Task Main()
    {
        var game = new Game(new ConsoleUserInterface());
        await game.Run();
    }

    private static readonly int[] ShipLengths = {2, 2, 3, 4, 5};
    private const int ArenaSizeX = 10;
    private const int ArenaSizeY = 10;

    private IUserInterface userInterface;

    public Game(IUserInterface userInterface)
    {
        this.userInterface = userInterface;
    }

    public async Task Run()
    {
        // Todo in order:
        // Welcome player.
        // Let player pick game mode. vs ai, or vs remote player.
        // Instantiate the player.
        // Instantiate the remote/ai player.
        // Decide who starts.
        // PlayTurn on the starting player.
        // PlayTurn on the other player.
        // Repeat last two steps until one player is defeated or something breaks.

        var gameLoopTask = InitializeGame();
        await gameLoopTask;
    }

    private Task InitializeGame()
    {
        CancellationTokenSource cancelSource = new CancellationTokenSource();
        
        var player1 = userInterface.GetPlayer1();
        player1.InitializePlayer(ShipLengths, ArenaSizeX, ArenaSizeY, cancelSource.Token);

        var player2 = userInterface.GetPlayer2();
        player2.InitializePlayer(ShipLengths, ArenaSizeX, ArenaSizeY, cancelSource.Token);

        return GameLoop(player1, player2);
    }

    private async Task GameLoop(IPlayer player1, IPlayer player2)
    {
        IPlayer[] players = ConfigurePlayerOrder(player1, player2);

        bool startingPlayerTurn = true;

        CancellationTokenSource cancelSource = new CancellationTokenSource();
        

        while (true)
        {
            var turnResult = await GetIPlayer(players, startingPlayerTurn).PlayTurnAsync(GetIPlayer(players, !startingPlayerTurn), cancelSource.Token);
            
            if (turnResult.TargetPlayerDefeated)
                break;
            
            startingPlayerTurn = !startingPlayerTurn;
        }
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
}