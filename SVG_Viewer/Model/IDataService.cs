using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SVG_Viewer.Model
{
    public interface IDataService
    {
        void GetData(Action<PathModel, Exception> callback);
    }
}
