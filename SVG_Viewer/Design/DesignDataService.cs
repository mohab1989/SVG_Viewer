using System;
using SVG_Viewer.Model;

namespace SVG_Viewer.Design
{
    public class DesignDataService : IDataService
    {
        public void GetData(Action<PathModel, Exception> callback)
        {
            // Use this to create design time data

            //var item = new DataItem("Welcome to MVVM Light [design]");
            //callback(item, null);
        }
    }
}