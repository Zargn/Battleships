namespace BattleshipsCore.EventArguments;

public class PlayerDefeatedEventArgs : EventArgs
{
    public PlayerDefeatedEventArgs(Arena arena)
    {
        Arena = arena;
    }

    public Arena Arena { get; }
}