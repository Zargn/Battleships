using Unnamed_Networking_Plugin.Interfaces;

namespace BattleshipsFwServer;

public class Logger : ILogger
{
    public void Log(object sender, string message, LogType logType)
    {
        Console.WriteLine($"[{logType.ToString()}] {sender}: {message}");
    }
}