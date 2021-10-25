using System.Windows;
using System.Windows.Controls;

namespace FlightsSimulator.Extentions
{
    public static class UiExtentions
    {
        public static void SetElementToGrid(this Grid grid, UIElement element, int row = 0, int col = 0)
        {
            Grid.SetRow(element, row);
            Grid.SetColumn(element, col);
            grid.Children.Add(element);
        }
    }
}
