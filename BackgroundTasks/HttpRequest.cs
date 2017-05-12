using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.System;

namespace BackgroundTasks
{
    class HttpRequest
    {
        public static async Task<string> GetRecentMovieInformationRequest(string city)
        {
            const string RecentMovieInformation_Api = "http://op.juhe.cn/onebox/movie/pmovie?key=bf900f647b1fa5a4f409bfd28530835e&city={0}";
            HttpClient httpclient = new HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            string result = null;
            string Api = RecentMovieInformation_Api.Replace("{0}", city);
            response = await httpclient.GetAsync(Api);
            result = await response.Content.ReadAsStringAsync();
            return result;
        }
        public static async Task<string> GetCity()
        {
            var accessStatus = await Geolocator.RequestAccessAsync();
            BasicGeoposition baspos = new BasicGeoposition();
            switch (accessStatus)
            {
                case GeolocationAccessStatus.Allowed:
                    {
                        Geolocator geolocator = new Geolocator { DesiredAccuracyInMeters = 0 };
                        Geoposition pos = await geolocator.GetGeopositionAsync();
                        Geopoint myLocation = pos.Coordinate.Point;
                        baspos = myLocation.Position;
                        baspos = PositionTransform.Transfrom(baspos);
                        const string MovieSearch_Api = "http://restapi.amap.com/v3/geocode/regeo?key=a3a90de8b27f31e0054c34e61e0e3d9f&location={0}&extensions=base ";
                        HttpClient httpclient = new HttpClient();
                        HttpResponseMessage response = new HttpResponseMessage();
                        string result = null;
                        string Location = Convert.ToString(baspos.Longitude) + "," + Convert.ToString(baspos.Latitude);
                        string Api = MovieSearch_Api.Replace("{0}", Location);
                        response = await httpclient.GetAsync(Api);
                        result = await response.Content.ReadAsStringAsync();
                        return result;
                    }
                case GeolocationAccessStatus.Denied:
                    {
                        Debug.WriteLine("已拒绝程序对位置的访问权,请至设置-隐私-位置中打开本程序的位置访问权限");
                        bool result = await Launcher.LaunchUriAsync(new Uri("ms-settings:privacy-location"));
                        return "0";
                    }
                case GeolocationAccessStatus.Unspecified:
                    {
                        Debug.WriteLine("未指明程序对位置的访问权,请至设置-隐私-位置中打开本程序的位置访问权限");
                        bool result = await Launcher.LaunchUriAsync(new Uri("ms-settings:privacy-location"));
                        return "1";
                    }
            }
            return "2";
        }
    }
}
