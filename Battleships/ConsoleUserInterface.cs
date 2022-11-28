﻿using System.Net;
using System.Text;
using Battleships.Interfaces;
using Battleships.objects;
using Battleships.Players;

namespace Battleships;

public class ConsoleUserInterface : IUserInterface
{
    private IPlayer player1;
    
    public IPlayer GetPlayer1()
    {
        Console.WriteLine("Make player type selection possible...");
        player1 = new LocalPlayer(this);
        return player1;
    }

    public IPlayer GetPlayer2()
    {
        Console.WriteLine("Make player type selection possible...");
        return new RemotePlayer(this, player1);
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

                if (tile.Hit)
                    symbol = 'O';

                if (tile.OccupiedByShip)
                    symbol = '#';

                if (tile.Hit && tile.OccupiedByShip)
                    symbol = 'X';

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
        var direction = GetDirection();

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
    public TargetCoordinates GetTargetCoordinates()
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

    public IPAddress GetIpAddress()
    {
        Console.WriteLine("Please enter target coordinates: (Xvalue Yvalue)");
        while (true)
        {
            try
            {
                return IPAddress.Parse(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.WriteLine("Input was in wrong format. Please enter coordinates in the following format: (Xvalue Yvalue)");
            }
        }
    }

    private TargetCoordinates GetDirection()
    {
        Console.WriteLine("Please enter direction: (North, East, South, West)");
        while (true)
        {
            var input = Console.ReadLine();
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

    public string GetTargetGroupCode()
    {
        Console.WriteLine("Please enter group code to join: ");
        while (true)
        {
            var input = Console.ReadLine();
            if (input.ToUpper().Length == 5)
                return input;
            Console.WriteLine("Invalid code length. Please enter 5 characters only.");
        }
    }
}