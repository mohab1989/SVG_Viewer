using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace SVG_Viewer.Model
{
    public class PathModel
    {
        public string Data { get; set; }
        public SolidColorBrush Fill { get; set; }
        public SolidColorBrush Stroke { get; set; }
        public int Tag { get; set; }
        public double StrokeThickness { get; set; }
    }
}
