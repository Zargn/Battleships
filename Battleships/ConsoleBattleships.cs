namespace Battleships;

public class ConsoleBattleships
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
}