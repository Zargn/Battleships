using Battleships.Interfaces;

namespace Battleships;

public class Game
{
    public static async Task Main()
    {
        var game = new Game();
        await game.Run();
    }



    private IUserInput userInput;

    public Game(IUserInput userInput)
    {
        this.userInput = userInput;
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
        
        var player1 = userInput.GetPlayer1();
        player1.InitializePlayer(cancelSource.Token);

        var player2 = userInput.GetPlayer2();
        player2.InitializePlayer(cancelSource.Token);

        return GameLoop(player1, player2);
    }

    private async Task GameLoop(IPlayer player1, IPlayer player2)
    {
        IPlayer[] players = ConfigurePlayerOrder(player1, player2);

        bool startingPlayerTurn = true;

        CancellationTokenSource cancelSource = new CancellationTokenSource();
        

        while (true)
        {
            var turnResult = await GetIPlayer(players, startingPlayerTurn).PlayTurn(GetIPlayer(players, !startingPlayerTurn), cancelSource.Token);
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