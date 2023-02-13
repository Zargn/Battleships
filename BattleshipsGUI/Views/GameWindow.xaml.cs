using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BattleshipsGUI.ViewModels;
using BattleshipsGUI.ViewSpecificCode;

namespace BattleshipsGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeFields();
        }

        private void InitializeFields()
        {
            PlayingFieldGridCreator.PopulateGrid(Player1Field);
            PlayingFieldGridCreator.PopulateGrid(Player2Field);
        }

        private bool t;
        
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            (DataContext as GameWindowViewModel).PlayerFieldClickable = t;
            (DataContext as GameWindowViewModel).EnemyFieldClickable = !t;
            t = !t;
        }
    }
}