using System;
using System.Windows.Media;

namespace SVG_Viewer.Model
{
    public class DataService : IDataService
    {
        public void GetData(Action<PathModel, Exception> callback)
        {
            // Use this to connect to the actual data service

            //var item = new DataItem("Welcome to MVVM Light");
            //callback(item, null);
            //var Data= new PathModel();
            //callback(Data,null);
        }
    }
}