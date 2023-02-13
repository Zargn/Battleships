using System.Windows;
using BattleshipsGUI.ViewSpecificCode;

namespace BattleshipsGUI.Views;

public partial class Player1ConfigWindow : Window
{
    public Player1ConfigWindow()
    {
        InitializeComponent();
        PlayingFieldGridCreator.PopulateGrid(PreviewField);
    }
}