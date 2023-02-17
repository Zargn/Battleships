using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using BattleshipsCore;
using BattleshipsCore.Interfaces;
using BattleshipsCore.objects;

namespace BattleshipsGUI.ViewModels;

public class GameWindowViewModel : IUserInterface, INotifyPropertyChanged
{
    public GameWindowViewModel()
    {
        game = new Game(this);
        PlayerConfig = new PlayerConfig();
        game.Run();
    }
    
    private Game game;
    
    private bool playerFieldClickable;
    private bool enemyFieldClickable;



    public PlayerConfig PlayerConfig { get; set; }
    
    
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




    public Task<IPlayer> GetPlayer1()
    {
        return PlayerConfig.GetPlayer1();
    }

    public Task<IPlayer> GetPlayer2()
    {
        throw new System.NotImplementedException();
    }

    public void DrawTiles(Tile[,] tiles, string username)
    {
        throw new System.NotImplementedException();
    }

    public Task<string> GetUsername()
    {
        throw new System.NotImplementedException();
    }

    public Task<bool> GetYesNoAnswer(string question, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }

    public Task<ShipPlacementInformation> GetShipPlacementInformation(int shipLength, CancellationToken cancellationToken)
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

    public Task<TargetCoordinates> GetTargetCoordinates(CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }

    public Task<IPAddress> GetIpAddress(CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }

    public Task<string> GetTargetGroupCode(CancellationToken cancellationToken)
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