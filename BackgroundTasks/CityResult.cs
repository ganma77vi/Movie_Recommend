using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundTasks
{
    public sealed class Rootobject2
    {
        public string status { get; set; }
        public string info { get; set; }
        public string infocode { get; set; }
        public Regeocode regeocode { get; set; }
    }

    public sealed class Regeocode
    {
        public string formatted_address { get; set; }
        public Addresscomponent addressComponent { get; set; }
    }

    public sealed class Addresscomponent
    {
        public string country { get; set; }
        public string province { get; set; }
        public object city { get; set; }
    }
}
