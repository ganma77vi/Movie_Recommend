using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace Movie_Recommend.ShareHelper
{
    class PositionTransform
    {

        // a = 6378245.0, 1/f = 298.3

        // b = a * (1 - f)

        // ee = (a^2 - b^2) / a^2;

        const double a = 6378245.0;

        const double ee = 0.00669342162296594323;



        /// <summary>

        /// 将世界坐标转换成火星坐标

        /// </summary>

        /// <param name="position">将要进行转换的世界坐标</param>

        /// <returns>转换后的火星坐标</returns>

        public static BasicGeoposition Transfrom(BasicGeoposition position)

        {

            double wgLat, wgLon, mgLat, mgLon;

            BasicGeoposition new_position = new BasicGeoposition();

            new_position.Altitude = position.Altitude;

            wgLat = position.Latitude;

            wgLon = position.Longitude;



            //如果坐标点不在国内，直接返回原先坐标

            if (outOfChina(wgLat, wgLon))

            {

                new_position.Latitude = position.Latitude;

                new_position.Longitude = position.Longitude;

                return new_position;

            }



            double dLat = TransformLat(wgLon - 105.0, wgLat - 35.0);

            double dLon = TransformLon(wgLon - 105.0, wgLat - 35.0);

            double radLat = wgLat / 180.0 * Math.PI;

            double magic = Math.Sin(radLat);

            magic = 1 - ee * magic * magic;

            double sqrtMagic = Math.Sqrt(magic);

            dLat = (dLat * 180.0) / ((a * (1 - ee)) / (magic * sqrtMagic) * Math.PI);

            dLon = (dLon * 180.0) / (a / sqrtMagic * Math.Cos(radLat) * Math.PI);

            mgLat = wgLat + dLat;

            mgLon = wgLon + dLon;

            new_position.Latitude = mgLat;

            new_position.Longitude = mgLon;

            return new_position;

        }



        /// <summary>

        /// 判断是否在国外

        /// </summary>

        /// <param name="lat">纬度</param>

        /// <param name="lon">经度</param>

        /// <returns></returns>

        private static bool outOfChina(double lat, double lon)

        {

            if (lon < 72.004 || lon > 137.8347)

                return true;

            if (lat < 0.8293 || lat > 55.8271)

                return true;

            return false;

        }



        /// <summary>

        /// 转换纬度

        /// </summary>

        /// <param name="x"></param>

        /// <param name="y"></param>

        /// <returns></returns>

        private static double TransformLat(double x, double y)

        {

            double ret = -100.0 + 2.0 * x + 3.0 * y + 0.2 * y * y + 0.1 * x * y + 0.2 * Math.Sqrt(Math.Abs(x));

            ret += (20.0 * Math.Sin(6.0 * x * Math.PI) + 20.0 * Math.Sin(2.0 * x * Math.PI)) * 2.0 / 3.0;

            ret += (20.0 * Math.Sin(y * Math.PI) + 40.0 * Math.Sin(y / 3.0 * Math.PI)) * 2.0 / 3.0;

            ret += (160.0 * Math.Sin(y / 12.0 * Math.PI) + 320 * Math.Sin(y * Math.PI / 30.0)) * 2.0 / 3.0;

            return ret;

        }

        /// <summary>

        /// 转换经度

        /// </summary>

        /// <param name="x"></param>

        /// <param name="y"></param>

        /// <returns></returns>

        private static double TransformLon(double x, double y)

        {

            double ret = 300.0 + x + 2.0 * y + 0.1 * x * x + 0.1 * x * y + 0.1 * Math.Sqrt(Math.Abs(x));

            ret += (20.0 * Math.Sin(6.0 * x * Math.PI) + 20.0 * Math.Sin(2.0 * x * Math.PI)) * 2.0 / 3.0;

            ret += (20.0 * Math.Sin(x * Math.PI) + 40.0 * Math.Sin(x / 3.0 * Math.PI)) * 2.0 / 3.0;

            ret += (150.0 * Math.Sin(x / 12.0 * Math.PI) + 300.0 * Math.Sin(x / 30.0 * Math.PI)) * 2.0 / 3.0;

            return ret;

        }
    }
}
