using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SVG_Viewer.Model;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Xml;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;

namespace SVG_Viewer.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private Path SelectedPath;
        private Color _SelectedColor;

        public double _SvgWidth;
        public double _SvgHeight;

        public double SvgWidth
        {
            get{return _SvgWidth;}
            set
            {
                _SvgWidth= value;
                RaisePropertyChanged("SvgWidth");
            }
        }
        public double SvgHeight
        {
            get { return _SvgHeight; }
            set
            {
                _SvgHeight = value;
                RaisePropertyChanged("SvgHeight");
            }
        }
        public Color SelectedColor
        {
            get { return _SelectedColor; }
            set
            {
                _SelectedColor = value;
                RaisePropertyChanged("SelectedColor");
            }
        }

        public MainViewModel()
        {
            PopulateColorList();
            SelectedColor = Colors.Red;
            AddSvgCommand = new RelayCommand(AddSvgCommandEx);
            OnMouseDownCommand = new RelayCommand<MouseButtonEventArgs>(OnMouseDownCommandEx);
        }

        public RelayCommand AddSvgCommand{ get; private set; }

        private void AddSvgCommandEx()
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.DefaultExt = ".svg";
            openFileDialog.Filter = "svg files (*.svg)|*.svg";
            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result==true)
            {
                try
                {
                    string filename = openFileDialog.FileName;
                    loadSVG(filename);
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        public RelayCommand<MouseButtonEventArgs> OnMouseDownCommand { get; private set; }
        private void OnMouseDownCommandEx(MouseButtonEventArgs args) 
        {
            SelectedPath = args.OriginalSource as Path;
            if (SelectedPath != null)
            {
                //SelectedColor = ((System.Windows.Media.SolidColorBrush)(SelectedPath.Fill)).Color;
                PathViewModel path = SelectedPath.DataContext as PathViewModel;
                path.Fill = ColorToBrush( SelectedColor );
            }
        }

        private IList<ColorItem> _ColorList= new ObservableCollection<ColorItem>();
        public IList<ColorItem> ColorList 
        {
            get { return _ColorList; }
        }
        private void PopulateColorList()
        {
            ColorList.Add(new ColorItem(Colors.Beige, "Beige"));
            ColorList.Add(new ColorItem(Colors.Black, "Black"));
            ColorList.Add(new ColorItem(Colors.Blue, "Blue"));
            ColorList.Add(new ColorItem(Colors.Pink, "Pink"));
            ColorList.Add(new ColorItem(Colors.Red, "Red"));
            ColorList.Add(new ColorItem(Colors.White, "White"));
            ColorList.Add(new ColorItem(Colors.Yellow, "Yellow"));
        }


        private IList<PathViewModel> _PathList = new ObservableCollection<PathViewModel>();
        public IList<PathViewModel> PathList
        {
            get
            {
                return _PathList;
            }
        }

        private void loadSVG(string fileName)
        {
            PathList.Clear();
            XmlDocument SvgDoc = new XmlDocument();
            SvgDoc.Load(fileName);
            XmlNamespaceManager xmlnsManager = new XmlNamespaceManager(SvgDoc.NameTable);
            xmlnsManager.AddNamespace("global", SvgDoc.DocumentElement.NamespaceURI);
            string GXpath = "/global:svg/global:g";
            XmlNodeList currentNodeList = SvgDoc.SelectNodes(GXpath, xmlnsManager);

            SvgWidth = getDoubleVal(Attribute(SvgDoc.DocumentElement, "width", "1"));
            SvgHeight = getDoubleVal(Attribute(SvgDoc.DocumentElement, "height", "1")); 

            while (currentNodeList.Count > 0)
            {
                int TagNum = 0;
                foreach (XmlNode GNode in currentNodeList)
                {
                    XmlNodeList pathsNodes = GNode.SelectNodes("./global:path", xmlnsManager);
                    if (pathsNodes.Count > 0)
                    {
                        foreach (XmlNode pathNode in pathsNodes)
                        {
                            try
                            {
                                //========================= Load Path Data=========================//
                                PathViewModel pathModel = new PathViewModel(new PathModel());
                                pathModel.Data = Attribute(pathNode, "d");

                                //========================= Load Path Style=========================//
                                    string AtrrString = null;
                                    if ((AtrrString = Attribute(pathNode,"style", null)) != null) // inkescape style
                                    {
                                        StyleParser StyleParser = new StyleParser(AtrrString);
                                        pathModel.Fill = ColorToBrush(StyleParser.Color("fill"));
                                        pathModel.Stroke = ColorToBrush(StyleParser.Color("stroke"));
                                        pathModel.StrokeThickness = StyleParser.Size("stroke-width");
                                        pathModel.Tag = TagNum;

                                    }
                                    else
                                    {
                                        pathModel.Fill = ColorToBrush(getcolor(Attribute(pathNode, "fill", "none")));
                                        pathModel.Stroke = ColorToBrush(getcolor(Attribute(pathNode, "stroke", "none")));
                                        pathModel.StrokeThickness = getPenSize(Attribute(pathNode, "stroke-width"));
                                        pathModel.Tag = TagNum;
                                    }

                                    PathList.Add(pathModel);
                                    TagNum++;
                                }
                                catch (Exception ex)
                                {
                                    System.Windows.MessageBox.Show("Error: While Loading SVG. Original error: " + ex.Message);
                                }                            
                        }
                    }
                }
                GXpath = GXpath + "/global:g";
                currentNodeList = SvgDoc.SelectNodes(GXpath, xmlnsManager);
            }
        }

        public static string Attribute(XmlNode node, string Attribute, string defaultVal = "")
        {
            if (node.Attributes[Attribute] != null)
                return node.Attributes[Attribute].Value;
            else
                return defaultVal;
        }
        private SolidColorBrush ColorToBrush (Color? color)
        {
            SolidColorBrush brush;
            if(color!=null)
            {
                return brush= new SolidColorBrush((Color)color);
            }
            else
            {
                return brush=new SolidColorBrush(Colors.Transparent);
            }
        }
        public static Color? getcolor(string color)
        {
            if (color == "none")
                return null;//Colors.Transparent;

            return (Color)ColorConverter.ConvertFromString(color);
        }
        public static double getPenSize(string penSizeString, double defaultVal = 1)
        {
            double result;
            if (!double.TryParse(penSizeString.Replace("px", ""), out result))
                return defaultVal;
            else
                return result;
        }

        public static double getDoubleVal(string doubleString, double defaultVal = 1)
        {
            double result;
            if (!double.TryParse(doubleString, out result))
                return defaultVal;
            else
                return result;
        }
    }
}