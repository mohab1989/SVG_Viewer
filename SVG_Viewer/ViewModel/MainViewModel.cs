using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SVG_Viewer.Model;
using System;
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

        private string svgWidth;
        private string svgHeight;

        public string SvgWidth
        {
            get{return svgWidth; }
            set
            {
                svgWidth = value;
                RaisePropertyChanged("SvgWidth");
            }
        }

        public string SvgHeight
        {
            get { return svgHeight; }
            set
            {
                svgHeight = value;
                RaisePropertyChanged("SvgHeight");
            }

        }
        public Color SelectedColor
        {
            get { return ColorPickerModel.selectedColor; }
            set
            {
                ColorPickerModel.selectedColor = value;
                RaisePropertyChanged("SelectedColor");
            }
        }

        public MainViewModel()
        {
            AddSvgCommand = new RelayCommand(AddSvgCommandEx);
            OnMouseDownCommand = new RelayCommand<MouseButtonEventArgs>(OnMouseDownCommandEx);
        }

        public RelayCommand AddSvgCommand{ get; private set; }

        private void AddSvgCommandEx()
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".svg",
                Filter = "svg files (*.svg)|*.svg"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string filename = openFileDialog.FileName;
                    LoadSVG(filename);
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
                PathViewModel path = SelectedPath.DataContext as PathViewModel;
                path.Fill = SelectedColor.ToString();
            }
        }

        public IList<ColorItem> ColorList 
        {
            get { return ColorPickerModel.colorList; }
        }

        public IList<PathViewModel> PathList { get; } = new ObservableCollection<PathViewModel>();

        private void LoadSVG(string fileName)
        {
            PathList.Clear();
            var SvgDoc = new XmlDocument();
            SvgDoc.Load(fileName);
            var xmlnsManager = new XmlNamespaceManager(SvgDoc.NameTable);
            xmlnsManager.AddNamespace("global", SvgDoc.DocumentElement.NamespaceURI);
            if(SvgDoc.SelectNodes("/global:svg", xmlnsManager).Count == 0)
            {
                System.Windows.MessageBox.Show("No SVG tag was found");
                return;
            }

            SvgWidth = GetAttributeValue(SvgDoc.DocumentElement, "width", "512");
            SvgHeight = GetAttributeValue(SvgDoc.DocumentElement, "height", "512");

            var pathes = SvgDoc.SelectNodes("//global:path", xmlnsManager);

            int TagNum = 0;
            foreach (XmlNode pathNode in pathes)
            {
                //========================= Load Path Data=========================//
                string AtrrString = GetAttributeValue(pathNode, "d", null);
                if (string.IsNullOrEmpty(AtrrString))
                {
                    System.Windows.MessageBox.Show("Path node doesnt have a `d` attribute");
                }

                PathModel pathModel = new PathModel
                {
                    Data = AtrrString,
                    Tag = TagNum,
                };
                ++TagNum;

                //========================= Load Path Style=========================//
                var StyleParser = new StyleParser(GetAttributeValue(pathNode, "style"));
                pathModel.Fill = StyleParser.String("fill","Transparent");
                pathModel.Stroke = StyleParser.String("stroke", "Black");
                pathModel.StrokeThickness = StyleParser.String("stroke-width","1");
                PathList.Add(new PathViewModel(pathModel));
            }
        }

        public static string GetAttributeValue(XmlNode node, string Attribute, string defaultVal = "")
        {
            XmlAttribute attribute;
            while ((attribute = node.Attributes[Attribute]) == null)
            {
                var parentNode = node.ParentNode;
                if(parentNode == null || parentNode.Attributes == null)
                {
                    return defaultVal;
                }
                node = parentNode;
            }
            return attribute.Value;
        }
    }
}