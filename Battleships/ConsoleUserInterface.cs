using System.Text;
using Battleships.Interfaces;
using Battleships.objects;
using Battleships.Players;

namespace Battleships;

public class ConsoleUserInterface : IUserInterface
{
    public IPlayer GetPlayer1()
    {
        return new LocalPlayer(this);
    }

    public IPlayer GetPlayer2()
    {
        throw new NotImplementedException();
    }

    public void DrawTiles(Tile[,] tiles)
    {
        Console.WriteLine();
        
        StringBuilder sb = new StringBuilder();
        
        for (int y = tiles.GetLength(1) - 1; y >= 0; y--)
        {
            sb.Append("| ");
            
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                var tile = tiles[x, y];
                var symbol = '~';

                if (tile.OccupiedByShip)
                    symbol = '#';

                sb.Append($"{symbol} ");
                // sb.Append($"{x}{y} ");
            }

            sb.Append("|");
            
            sb.AppendLine();
        }

        Console.WriteLine(sb);
    }

    public string GetUsername()
    {
        Console.WriteLine("Please enter username: ");
        return Console.ReadLine();
    }

    public bool GetYesNoAnswer(string question)
    {
        while (true)
        {
            Console.WriteLine(question);
            var response = Console.ReadLine();

            if (response.ToLower().StartsWith("y"))
            {
                return true;
            }
            else if (response.ToLower().StartsWith("n"))
            {
                return false;
            }

            Console.WriteLine("Invalid input. [Y]es and [N]o allowed.");
        }
    }

    public ShipPlacementInformation GetShipPlacementInformation(int shipLength)
    {
        Console.WriteLine($"Please enter location for ship of length: {shipLength}");
        var coordinates = GetTargetCoordinates();

        Console.WriteLine($"Please enter direction: ");
    }

    public void DisplayError(string message)
    {
        throw new NotImplementedException();
    }

    private TargetCoordinates GetTargetCoordinates()
    {
        Console.WriteLine("Please enter target coordinates: (Xvalue Yvalue)");
        while (true)
        {
            try
            {
                return TargetCoordinates.FromString(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.WriteLine("Input was in wrong format. Please enter coordinates in the following format: (Xvalue Yvalue)");
            }
        }
    }
}