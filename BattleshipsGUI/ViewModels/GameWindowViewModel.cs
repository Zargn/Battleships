using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using BattleshipsCore;
using BattleshipsCore.Interfaces;
using BattleshipsCore.objects;

namespace BattleshipsGUI.ViewModels;

public class GameWindowViewModel : IUserInterface, INotifyPropertyChanged
{
    public GameWindowViewModel()
    {
        game = new Game(this);
    }
    
    private Game game;
    
    private bool playerFieldClickable;
    private bool enemyFieldClickable;
    private bool player1ConfigVisible;
    private bool player2ConfigVisible;
    
    public bool PlayerFieldClickable
    {
        get => playerFieldClickable;
        set
        {
            playerFieldClickable = value;
            OnPropertyChanged();
        }
    }

    public bool EnemyFieldClickable
    {
        get => enemyFieldClickable; 
        set
        {
            enemyFieldClickable = value;
            OnPropertyChanged();
        }
    }

    public bool Player1ConfigVisible
    {
        get => player1ConfigVisible;
        set
        {
            player1ConfigVisible = value;
            OnPropertyChanged();
        }
    }
    
    public bool Player2ConfigVisible
    {
        get => player2ConfigVisible;
        set
        {
            player2ConfigVisible = value;
            OnPropertyChanged();
        }
    }
    
    
    
    
    
    public IPlayer GetPlayer1()
    {
        throw new System.NotImplementedException();
    }

    public IPlayer GetPlayer2()
    {
        throw new System.NotImplementedException();
    }

    public void DrawTiles(Tile[,] tiles, string username)
    {
        throw new System.NotImplementedException();
    }

    public string GetUsername()
    {
        throw new System.NotImplementedException();
    }

    public bool GetYesNoAnswer(string question, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }

    public ShipPlacementInformation GetShipPlacementInformation(int shipLength, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }

    public void DisplayError(string message)
    {
        throw new System.NotImplementedException();
    }

    public void DisplayMessage(string message)
    {
        throw new System.NotImplementedException();
    }

    public TargetCoordinates GetTargetCoordinates(CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }

    public IPAddress GetIpAddress(CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }

    public string GetTargetGroupCode(CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }

    
    
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}