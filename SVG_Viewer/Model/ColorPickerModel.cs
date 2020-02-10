using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

namespace SVG_Viewer.Model
{
    static class ColorPickerModel
    {
        static public Color selectedColor = Colors.Red;

        static public IList<ColorItem> colorList = new ObservableCollection<ColorItem>() {
            new ColorItem(Colors.Beige, "Beige"),
            new ColorItem(Colors.Black, "Black"),
            new ColorItem(Colors.Blue, "Blue"),
            new ColorItem(Colors.Pink, "Pink"),
            new ColorItem(Colors.Red, "Red"),
            new ColorItem(Colors.White, "White"),
            new ColorItem(Colors.Yellow, "Yellow")
        };
    }
}
