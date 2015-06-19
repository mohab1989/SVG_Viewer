using GalaSoft.MvvmLight;
using SVG_Viewer.Model;
using System.Windows.Media;

namespace SVG_Viewer.ViewModel
{
    public class PathViewModel : ViewModelBase
    {
        public PathViewModel(PathModel _model)
        {
            mModel = _model;
        }
        protected PathModel mModel { get; set; }

        public string Data
        {
            get
            {
                return mModel.Data;
            }
            set
            {
                mModel.Data = value;
                RaisePropertyChanged("Data");
            }
        }
        public SolidColorBrush Fill { 
            get 
            { 
                return mModel.Fill; 
            } 
            set 
            {
                if (mModel != null)
                    mModel.Fill = value;
                RaisePropertyChanged("Fill");
            }
        }
        public SolidColorBrush Stroke 
        {
            get 
            {
                return mModel.Stroke;
            } 
            set
            {
                if (mModel != null)
                {
                    mModel.Stroke = value;
                    RaisePropertyChanged("Stroke");
                }
            } 
        }

        public int Tag
        {
            get
            {
                return mModel.Tag;
            }
            set
            {
                if (mModel != null && value>=0)
                {
                    mModel.Tag = value;
                    RaisePropertyChanged("Tag");
                }
            }
        }
        public double StrokeThickness
        {
            get
            {
                return mModel.StrokeThickness;
            }
            set
            {
                if (mModel != null && value >= 0)
                {
                    mModel.StrokeThickness = value;
                    RaisePropertyChanged("StrokeThickness");
                }
            }
        }
    }
}
