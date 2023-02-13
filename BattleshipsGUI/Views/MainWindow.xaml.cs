using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BattleshipsGUI.ViewModels;

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
            PopulateGrid(Player1Field);
            PopulateGrid(Player2Field);
        }

        private void PopulateGrid(Grid grid)
        {
            for (var x = 0; x < 10; x++)
            {
                for (var y = 1; y < 11; y++)
                {
                    var button = new Button();
                    grid.Children.Add(button);
                    button.SetValue(Grid.RowProperty, y);
                    button.SetValue(Grid.ColumnProperty, x);
                    button.BorderBrush = new SolidColorBrush(Colors.DimGray);
                    button.Background = new SolidColorBrush();
                }
            }
        }

        private bool t;
        
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            (DataContext as MainWindowViewModel).PlayerFieldClickable = t;
            (DataContext as MainWindowViewModel).EnemyFieldClickable = !t;
            t = !t;
        }
    }
}