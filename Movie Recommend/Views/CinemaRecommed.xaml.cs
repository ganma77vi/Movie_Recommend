using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Movie_Recommend.Model;
using System.Reflection;
using Windows.UI.Xaml.Controls.Maps;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Shapes;
using Windows.UI;
using Movie_Recommend.ShareHelper;
using Newtonsoft.Json;
using Movie_Recommend.ViewModel;
using System.Threading.Tasks;
using System.ComponentModel;
using Windows.Services.Maps;
using Windows.UI.Popups;
using Windows.System;
using System.Net.Http;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace Movie_Recommend.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class CinemaRecommed : Page
    {
        private CinemaRecommendViewModel viewmodel;
        Rootobject1 POIResult;
        double SerchRange;
        Geopoint myLocation;
        public CinemaRecommed()
        {
            this.InitializeComponent();
            viewmodel = new CinemaRecommendViewModel();
            this.DataContext = viewmodel;
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            SerchRange = 3000;
            MyMap.ZoomLevel = 14;
            await FirstStep();
        }
        private async Task FirstStep()
        {
            var accessStatus = await Geolocator.RequestAccessAsync();
            BasicGeoposition baspos = new BasicGeoposition();
            switch (accessStatus)
            {
                case GeolocationAccessStatus.Allowed:
                    {
                        Geolocator geolocator = new Geolocator { DesiredAccuracyInMeters = 0 };
                        Geoposition pos = await geolocator.GetGeopositionAsync();
                        myLocation = pos.Coordinate.Point;
                        baspos = myLocation.Position;
                        baspos = PositionTransform.Transfrom(baspos);
                        myLocation = new Geopoint(baspos);
                        UIElement marker = PointMeMarker();
                        MyMap.Children.Add(marker);
                        MapControl.SetLocation(marker, myLocation);
                        MyMap.Center = myLocation;
                        MyMap.LandmarksVisible = true;
                        break;
                    }
                case GeolocationAccessStatus.Denied:
                    {
                        var dialog = new MessageDialog("已拒绝程序对位置的访问权,请至设置-隐私-位置中打开本程序的位置访问权限", "异常提示");
                        dialog.Commands.Add(new UICommand("确定", cmd => { }));
                        var a = await dialog.ShowAsync();
                        bool result = await Launcher.LaunchUriAsync(new Uri("ms-settings:privacy-location"));
                        return;
                    }
                case GeolocationAccessStatus.Unspecified:
                    {
                        var dialog = new MessageDialog("未指明程序对位置的访问权,请至设置-隐私-位置中打开本程序的位置访问权限", "异常提示");
                        dialog.Commands.Add(new UICommand("确定", cmd => { }));
                        var a = await dialog.ShowAsync();
                        bool result = await Launcher.LaunchUriAsync(new Uri("ms-settings:privacy-location"));
                        return;
                    }
            }
            string position = baspos.Longitude + "," + baspos.Latitude;
            string json=null;
            try
            {
                json = await Httprequest.HttpRequest.GetPositionSearchRequest(position, SerchRange);
            }
            catch(HttpRequestException e)
            {
                var dialog = new MessageDialog("周边检索异常(未能成功发送请求)", "异常提示");
                dialog.Commands.Add(new UICommand("确定", cmd => { }));
                var a = await dialog.ShowAsync();
            }
            if (!string.IsNullOrWhiteSpace(json))
            {
                POIResult = JsonConvert.DeserializeObject<Rootobject1>(json);
                viewmodel.poiresult = POIResult.pois;
                foreach (Pois i in POIResult.pois)
                {
                    Geopoint geo = LocationToGeoPoint(i.location);
                    MapIcon mapIcon1 = new MapIcon();
                    mapIcon1.Location = geo;
                    mapIcon1.NormalizedAnchorPoint = new Point(0.5, 1.0);
                    mapIcon1.Title = i.name;
                    MyMap.MapElements.Add(mapIcon1);
                }
            }
        }
        public UIElement PointMeMarker()
        {
            Canvas marker = new Canvas();
            Ellipse outer = new Ellipse() { Width = 25, Height = 25 };
            outer.Fill = new SolidColorBrush(Color.FromArgb(255, 240, 240, 240));
            outer.Margin = new Thickness(-12.5, -12.5, 0, 0);
            Ellipse inner = new Ellipse() { Width = 20, Height = 20 };
            inner.Fill = new SolidColorBrush(Colors.Black);
            inner.Margin = new Thickness(-10, -10, 0, 0);
            Ellipse core = new Ellipse() { Width = 10, Height = 10 };
            core.Fill = new SolidColorBrush(Colors.White);
            core.Margin = new Thickness(-5, -5, 0, 0);
            marker.Children.Add(outer);
            marker.Children.Add(inner);
            marker.Children.Add(core);
            return marker;
        }
        public Geopoint LocationToGeoPoint(string Location)
        {
            string[] arr = Location.Split(',');
            BasicGeoposition Bas = new BasicGeoposition();
            Bas.Latitude = Convert.ToDouble(arr[1]);
            Bas.Longitude = Convert.ToDouble(arr[0]);
            Geopoint Geo = new Geopoint(Bas);
            return Geo;
        }

        private async void AddRange_Click(object sender, RoutedEventArgs e)
        {
            SerchRange += 1000;
            await FirstStep();
        }

        private async void CinemaListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MyMap.Routes.Clear();
            ListView Lis = sender as ListView;
            CinemaRecommendViewModel _poi = Lis.DataContext as CinemaRecommendViewModel;
            if(Lis.SelectedIndex==-1)
            {
                return;
            }
            Pois poi = _poi.poiresult[Lis.SelectedIndex] as Pois;
            string[] arr = poi.location.Split(',');
            BasicGeoposition startLocation = new BasicGeoposition() { Latitude = myLocation.Position.Latitude, Longitude = myLocation.Position.Longitude };
            BasicGeoposition endLocation = new BasicGeoposition() { Latitude = Convert.ToDouble(arr[1]), Longitude = Convert.ToDouble(arr[0]) };
            MapRouteFinderResult routeResult = await MapRouteFinder.GetDrivingRouteAsync(new Geopoint(startLocation), new Geopoint(endLocation), MapRouteOptimization.Time, MapRouteRestrictions.None);
            if (routeResult.Status == MapRouteFinderStatus.Success)
            {
                MapRouteView viewOfRoute = new MapRouteView(routeResult.Route);
                viewOfRoute.RouteColor = Colors.Yellow;
                viewOfRoute.OutlineColor = Colors.Black;
                MyMap.Routes.Add(viewOfRoute);
                await MyMap.TrySetViewBoundsAsync(
                      routeResult.Route.BoundingBox,
                      null,
                      Windows.UI.Xaml.Controls.Maps.MapAnimationKind.None);
            }
        }

    }
    public class Converter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, string language)
        {
            return ("直线距离: " + value.ToString() + "米");
        }
        public object ConvertBack(object value, System.Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
