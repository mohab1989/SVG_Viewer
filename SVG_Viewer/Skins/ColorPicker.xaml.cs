using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Xceed.Wpf.Toolkit;

namespace SVG_Viewer.Skins
{
    public partial class ColorPicker : UserControl
    {
        public ColorPicker()
        {
            InitializeComponent();
        }

        #region Color Property

        public Color? Color
        {
            get { return (Color?)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }


        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register(
            "Color",
            typeof(Color?),
            typeof(ColorPicker),
            new UIPropertyMetadata(Colors.Green, OnColorPropertyChanged));

        static void OnColorPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var self = obj as ColorPicker;
            if (self != null)
            {
                var NewValue = (Color?)args.NewValue;
                var oldValue = (Color?)args.OldValue;

                //self.setActiveColor(NewValue);
            }
        }


        #endregion

        private void setActiveColor(Color? _newColor)
        {
            Color = Colors.Red;
        }

        private void ColorOptions_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

    }
}
