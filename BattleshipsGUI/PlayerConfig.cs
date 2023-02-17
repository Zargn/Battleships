using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using BattleshipsCore.Interfaces;

namespace BattleshipsGUI;

public class PlayerConfig : INotifyPropertyChanged
{
    private bool player1ConfigVisible;
    private bool player2ConfigVisible;
    
    private bool player1UserControlled = true;
    private bool player1CpuControlled;
    
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

    public bool Player1UserControlled
    {
        get => player1UserControlled;
        set
        {
            player1UserControlled = value;
            OnPropertyChanged();
        }
    }
    
    public bool Player1CpuControlled
    {
        get => player1CpuControlled;
        set
        {
            player1CpuControlled = value;
            OnPropertyChanged();
        }
    }



    public async Task<IPlayer> GetPlayer1()
    {
        player1ConfigVisible = true;

        throw new NotImplementedException();

        Player1ConfigVisible = false;
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