using System.Net;
using System.Text;
using Battleships.objects;
using Battleships.objects.networking;
using BattleshipsCore.Interfaces;
using BattleshipsCore.objects;
using BattleshipsCore.Players;

namespace Battleships;

public class ConsoleUserInterface : IUserInterface
{
    private IPlayer player1;
    
    public IPlayer GetPlayer1()
    {
        if (GetYesNoAnswer("Do you want to control your player?", CancellationToken.None))
        {
            player1 = new LocalPlayer(this);
        }
        else
        {
            player1 = new BotPlayer();
        }

        return player1;
    }

    public IPlayer GetPlayer2()
    {
        if (GetYesNoAnswer("Do you want to play multiplayer?", CancellationToken.None))
        {
            return new RemotePlayer(this, player1, new ConsoleLogger(), new JsonSerializerAdapter());
        }
        
        return new BotPlayer();
    }

    public void DrawTiles(Tile[,] tiles, string username)
    {
        Console.WriteLine();
        
        StringBuilder sb = new StringBuilder();

        sb.AppendLine($"{username}'s board");
        sb.AppendLine("   0 1 2 3 4 5 6 7 8 9 ");
        
        for (int y = tiles.GetLength(1) - 1; y >= 0; y--)
        {
            sb.Append($"{y.ToString()}  ");
            
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                var tile = tiles[x, y];
                var symbol = '⋅';

                if (tile.Hit)
                    symbol = 'O';

                if (tile.OccupiedByShip)
                    symbol = '#';

                if (tile.Hit && tile.OccupiedByShip)
                    symbol = 'X';

                sb.Append($"{symbol} ");
                // sb.Append($"{x}{y} ");
            }

            sb.Append($" {y.ToString()}");
            
            sb.AppendLine();
        }

        sb.Append("   0 1 2 3 4 5 6 7 8 9 ");

        Console.WriteLine(sb);
    }

    public string GetUsername()
    {
        Console.WriteLine("Please enter username: ");
        return Console.ReadLine();
    }

    public bool GetYesNoAnswer(string question, CancellationToken cancellationToken)
    {
        while (true)
        {
            if (cancellationToken.IsCancellationRequested)
                throw new OperationCanceledException();
            
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

    public ShipPlacementInformation GetShipPlacementInformation(int shipLength, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Please enter location for ship of length: {shipLength}");
        var coordinates = GetTargetCoordinates(cancellationToken);

        if (cancellationToken.IsCancellationRequested)
            throw new OperationCanceledException();
        
        Console.WriteLine($"Please enter direction: ");
        var direction = GetDirection(cancellationToken);

        Console.WriteLine("Please enter Ship Name. (Leave empty for default.)");
        var name = Console.ReadLine();

        return new ShipPlacementInformation(coordinates, direction, name);
    }

    public void DisplayError(string message)
    {
        Console.WriteLine($"Error: {message}");
    }

    public void DisplayMessage(string message)
    {
        Console.WriteLine(message);
    }

    // TODO: Add string message argument?
    public TargetCoordinates GetTargetCoordinates(CancellationToken cancellationToken)
    {
        Console.WriteLine("Please enter target coordinates: (Xvalue Yvalue)");
        while (true)
        {
            if (cancellationToken.IsCancellationRequested)
                throw new OperationCanceledException();
            
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

    public IPAddress GetIpAddress(CancellationToken cancellationToken)
    {
        // return IPAddress.Parse("192.168.1.228");
        return IPAddress.Parse("90.226.3.160");
        Console.WriteLine("Please enter server ip address: (0-255.0-255.0-255.0-255)");
        while (true)
        {
            try
            {
                return IPAddress.Parse(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.WriteLine("Input was in wrong format. Please enter a valid Ipv4 address. Format: (0-255.0-255.0-255.0-255)");
            }
        }
    }

    private TargetCoordinates GetDirection(CancellationToken cancellationToken)
    {
        Console.WriteLine("Please enter direction: (North, East, South, West)");
        while (true)
        {
            if (cancellationToken.IsCancellationRequested)
                throw new OperationCanceledException();
            
            var input = Console.ReadLine().ToLower();
            if (input == null)
            {
                Console.WriteLine("Please enter one of the following: (North, East, South, West)");
                continue;
            }
            switch (input[0])
            {
                case 'n':
                    return TargetCoordinates.North;
                case 'e':
                    return TargetCoordinates.East;
                case 's':
                    return TargetCoordinates.South;
                case 'w':
                    return TargetCoordinates.West;
                default:
                    Console.WriteLine("Please enter one of the following: (North, East, South, West)");
                    break;
            }
        }
    }

    public string GetTargetGroupCode(CancellationToken cancellationToken)
    {
        Console.WriteLine("Please enter group code to join: ");
        while (true)
        {
            if (cancellationToken.IsCancellationRequested)
                throw new OperationCanceledException();
            
            var input = Console.ReadLine();
            if (input.ToUpper().Length == 5)
                return input;
            Console.WriteLine("Invalid code length. Please enter 5 characters only.");
        }
    }
}