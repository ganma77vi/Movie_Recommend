using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Movie_Recommend.Model
{
    public class Rootobject1
    {
        public string status { get; set; }
        public string count { get; set; }
        public string info { get; set; }
        public string infocode { get; set; }
        public Suggestion suggestion { get; set; }
        public ObservableCollection<Pois> pois { get; set; }
    }

    public class Suggestion
    {
        public object[] keywords { get; set; }
        public object[] cities { get; set; }
    }

    public class Pois
    {
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string typecode { get; set; }
        public object biz_type { get; set; }
        public string address { get; set; }
        public string location { get; set; }
        public object tel { get; set; }
        public string distance { get; set; }
        public object[] biz_ext { get; set; }
        public string pname { get; set; }
        public string cityname { get; set; }
        public string adname { get; set; }
        public object[] importance { get; set; }
        public object[] shopid { get; set; }
        public string shopinfo { get; set; }
        public object[] poiweight { get; set; }
    }

}
