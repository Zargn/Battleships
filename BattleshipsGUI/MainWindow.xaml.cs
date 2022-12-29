using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
                for (var y = 0; y < 10; y++)
                {
                    var button = new Button();
                    grid.Children.Add(button);
                    button.SetValue(Grid.RowProperty, y);
                    button.SetValue(Grid.ColumnProperty, x);
                    button.BorderBrush = new SolidColorBrush();
                    button.Background = new SolidColorBrush();
                }
            }
        }
    }
}