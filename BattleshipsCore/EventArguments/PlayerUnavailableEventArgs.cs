namespace BattleshipsCore.EventArguments;

public class PlayerUnavailableEventArgs : EventArgs
{
    public string Reason { get; }
    public string DetailedReason { get; }

    public PlayerUnavailableEventArgs(string reason, string detailedReason)
    {
        Reason = reason;
        DetailedReason = detailedReason;
    }
}