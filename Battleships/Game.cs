namespace Battleships;

public class Game
{
    public static async Task Main()
    {
        var game = new Game();
        await game.Run();
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
    }
}