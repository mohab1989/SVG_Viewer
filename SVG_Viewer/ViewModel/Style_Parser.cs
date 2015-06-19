using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Xml;

namespace SVG_Viewer.ViewModel
{

    public class StyleParser
    {
        private Dictionary<string, string> STYLE_INFO = new Dictionary<string, string>();
        public StyleParser(string styleString)
        {
            //removes the ; and splits the string between each :
            string[] items = styleString.TrimEnd(';').Split(';');
            foreach (string item in items)
            {
                // split the item from its value
                string[] keyValue = item.Split(':');
                STYLE_INFO.Add(keyValue[0], keyValue[1]);
            }
        }

        public string String(string index, string defaultVal = "")
        {
            string result;
            if (STYLE_INFO.TryGetValue(index, out result))
                return result;
            return defaultVal;
        }

        public Color? Color(string index)
        {
            string colorString = String(index, "none");
            if (colorString != null && colorString != "none")
                return (Color)ColorConverter.ConvertFromString(colorString);

            return null;
        }

        public double Size(string index, double defaultVal = 1)
        {
            string sizeString = String(index);
            double result;
            if (sizeString != null)
                if (double.TryParse(sizeString.Replace("px", ""), out result))
                    return result;

            return defaultVal;
        }
    }
}
