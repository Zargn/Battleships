using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

namespace BattleshipsGUI.ViewSpecificCode;

public static class PlayingFieldGridCreator
{
    public static void PopulateGrid(Grid grid)
    {
        for (var x = 0; x < 10; x++)
        {
            for (var y = 0; y < 10; y++)
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
}