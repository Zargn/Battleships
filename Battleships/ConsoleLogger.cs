﻿using System.Diagnostics;
using Unnamed_Networking_Plugin.Interfaces;

namespace Battleships.objects.networking;

public class ConsoleLogger : ILogger
{
    public void Log(object sender, string message, LogType logType)
    {
#if  DEBUG
        Console.WriteLine($"[{logType.ToString()}] {sender}: {message}");
#endif
    }
}