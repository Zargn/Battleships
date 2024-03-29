﻿using System.Net;
using BattleshipsCore.objects;

namespace BattleshipsCore.Interfaces;

public interface IUserInterface
{
    public IPlayer GetPlayer1();
    public IPlayer GetPlayer2();

    // public void DrawArenas(IPlayer player1);
    public void DrawTiles(Tile[,] tiles, string username);

    public string GetUsername();

    public bool GetYesNoAnswer(string question, CancellationToken cancellationToken);

    public ShipPlacementInformation GetShipPlacementInformation(int shipLength, CancellationToken cancellationToken);
    public void DisplayError(string message);
    public void DisplayMessage(string message);

    public TargetCoordinates GetTargetCoordinates(CancellationToken cancellationToken);

    public IPAddress GetIpAddress(CancellationToken cancellationToken);
    public string GetTargetGroupCode(CancellationToken cancellationToken);
}