using Movie_Recommend.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using Windows.UI.Popups;
using Movie_Recommend.Model;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using SQLite.Net.Interop;
using SQLite.Net.Attributes;
using Windows.Storage;
using Movie_Recommend.HelperClass;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Windows.ApplicationModel.Background;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace Movie_Recommend
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    /// 
    public sealed partial class MainPage : Page
    {
        private ViewModel.MainPageViewModel viewmodel;
        Rootobject recentmovielist;
        Rootobject2 CityModel;
        SQLiteHelper sqliteHelper = new SQLiteHelper();
        string city;
        private const string taskName = "BlogFeedBackgroundTask";
        private const string taskEntryPoint = "BackgroundTasks.BlogFeedBackgroundTask";
        public MainPage()
        {
            this.InitializeComponent();
            viewmodel = new ViewModel.MainPageViewModel();
            this.DataContext = viewmodel;
            MySpiltView.IsPaneOpen = false;
            this.NavigationCacheMode = NavigationCacheMode.Required;
        }
        private void HamburgerListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (HamburgerListBox.SelectedIndex == 0)
            {
                if (this.Frame != null)
                {
                    MyFrame.Navigate(typeof(MainPage));
                }
            }
            if (HamburgerListBox.SelectedIndex == 1)
            {
                if (this.Frame != null)
                {
                    MyFrame.Navigate(typeof(CinemaRecommed));
                }
            }
            if (HamburgerListBox.SelectedIndex == 2)
            {
                if (this.Frame != null)
                {
                    MyFrame.Navigate(typeof(MyCollection));
                }
            }
            if (HamburgerListBox.SelectedIndex == 3)
            {
                if (this.Frame != null)
                {
                    MyFrame.Navigate(typeof(Set));
                }
            }
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySpiltView.IsPaneOpen = !MySpiltView.IsPaneOpen;
        }
        private async Task FirstStep()
        {
            string json1="1";
            try
            {
                json1 = await Httprequest.HttpRequest.GetCity();
                if (json1 != "0" && json1 != "1" && json1 != "2")
                    CityModel = JsonConvert.DeserializeObject<Rootobject2>(json1);
                else
                    return;
                if (CityModel.regeocode.addressComponent.city == null)
                {
                    city = CityModel.regeocode.addressComponent.province;
                }
                else
                {
                    city = CityModel.regeocode.addressComponent.city.ToString();
                }
            }
            catch (HttpRequestException)
            {
                var dialog = new MessageDialog("获取城市定位异常(未能成功发送请求,建议检查网络环境)，已默认为北京市", "异常提示");
                dialog.Commands.Add(new UICommand("确定", cmd => { }));
                dialog.Commands.Add(new UICommand("重新获取", new UICommandInvokedHandler(this.CommandInvokedHandler)));
                var a = await dialog.ShowAsync();
                if (a.Label == "重新获取")
                {
                    return;
                }
            }
            try
            {
                string json = await Httprequest.HttpRequest.GetRecentMovieInformationRequest(city);
                json = json.Replace("\"1\"", "a");
                json = json.Replace("\"2\"", "b");
                json = json.Replace("\"3\"", "c");
                json = json.Replace("\"4\"", "d");
                json = json.Replace("\"5\"", "e");
                if (!string.IsNullOrWhiteSpace(json))
                {
                    recentmovielist = JsonConvert.DeserializeObject<Model.Rootobject>(json);
                    switch (recentmovielist.error_code)
                    {
                        case 0:
                            {
                                viewmodel.RecentMovieListResult = recentmovielist.result.data[0].data;
                                viewmodel.RecentMovieListResult1 = recentmovielist.result.data[1].data;
                                break;
                            }
                        #region 服务器错误码
                        case 209403:
                            {
                                var dialog = new MessageDialog("网络错误，请重试", "异常提示");
                                dialog.Commands.Add(new UICommand("确定", cmd => { }));
                                var a = await dialog.ShowAsync();
                                break;
                            }
                        case 209405:
                            {
                                var dialog = new MessageDialog("查询不到热映电影相关信息", "异常提示");
                                dialog.Commands.Add(new UICommand("确定", cmd => { }));
                                var a = await dialog.ShowAsync();
                                break;
                            }
                        case 209404:
                            {
                                var dialog = new MessageDialog("定位异常，城市名为空", "异常提示");
                                dialog.Commands.Add(new UICommand("确定", cmd => { }));
                                var a = await dialog.ShowAsync();
                                break;
                            }
                        #endregion
                        #region 系统级错误码
                        case 10001:
                            {
                                var dialog = new MessageDialog("错误的请求KEY,请联系开发人员", "异常提示");
                                dialog.Commands.Add(new UICommand("确定", cmd => { }));
                                var a = await dialog.ShowAsync();
                                break;
                            }
                        case 10002:
                            {
                                var dialog = new MessageDialog("该KEY无请求权限,请联系开发人员", "异常提示");
                                dialog.Commands.Add(new UICommand("确定", cmd => { }));
                                var a = await dialog.ShowAsync();
                                break;
                            }
                        case 10003:
                            {
                                var dialog = new MessageDialog("KEY过期,请联系开发人员", "异常提示");
                                dialog.Commands.Add(new UICommand("确定", cmd => { }));
                                var a = await dialog.ShowAsync();
                                break;
                            }
                        case 10004:
                            {
                                var dialog = new MessageDialog("错误的OPENID", "异常提示");
                                dialog.Commands.Add(new UICommand("确定", cmd => { }));
                                var a = await dialog.ShowAsync();
                                break;
                            }
                        case 10005:
                            {
                                var dialog = new MessageDialog("应用未审核超时，请提交认证，请联系开发人员", "异常提示");
                                dialog.Commands.Add(new UICommand("确定", cmd => { }));
                                var a = await dialog.ShowAsync();
                                break;
                            }
                        case 10007:
                            {
                                var dialog = new MessageDialog("未知的请求源", "异常提示");
                                dialog.Commands.Add(new UICommand("确定", cmd => { }));
                                var a = await dialog.ShowAsync();
                                break;
                            }
                        case 10008:
                            {
                                var dialog = new MessageDialog("被禁止的IP", "异常提示");
                                dialog.Commands.Add(new UICommand("确定", cmd => { }));
                                var a = await dialog.ShowAsync();
                                break;
                            }
                        case 10009:
                            {
                                var dialog = new MessageDialog("被禁止的KEY", "异常提示");
                                dialog.Commands.Add(new UICommand("确定", cmd => { }));
                                var a = await dialog.ShowAsync();
                                break;
                            }
                        case 10011:
                            {
                                var dialog = new MessageDialog("当前IP请求超过限制", "异常提示");
                                dialog.Commands.Add(new UICommand("确定", cmd => { }));
                                var a = await dialog.ShowAsync();
                                break;
                            }
                        case 10012:
                            {
                                var dialog = new MessageDialog("请求超过次数限制", "异常提示");
                                dialog.Commands.Add(new UICommand("确定", cmd => { }));
                                var a = await dialog.ShowAsync();
                                break;
                            }
                        case 10013:
                            {
                                var dialog = new MessageDialog("测试KEY超过请求限制", "异常提示");
                                dialog.Commands.Add(new UICommand("确定", cmd => { }));
                                var a = await dialog.ShowAsync();
                                break;
                            }
                        case 10014:
                            {
                                var dialog = new MessageDialog("系统内部异常", "异常提示");
                                dialog.Commands.Add(new UICommand("确定", cmd => { }));
                                var a = await dialog.ShowAsync();
                                break;
                            }
                        case 10020:
                            {
                                var dialog = new MessageDialog("接口维护", "异常提示");
                                dialog.Commands.Add(new UICommand("确定", cmd => { }));
                                var a = await dialog.ShowAsync();
                                break;
                            }
                        case 10021:
                            {
                                var dialog = new MessageDialog("接口停用", "异常提示");
                                dialog.Commands.Add(new UICommand("确定", cmd => { }));
                                var a = await dialog.ShowAsync();
                                break;
                            }
                            #endregion
                    }
                }
            }
            catch(HttpRequestException)
            {
                var dialog = new MessageDialog("获取影讯异常(未能成功发送请求,建议检查网络环境)", "异常提示");
                dialog.Commands.Add(new UICommand("确定", cmd => { }));
                await dialog.ShowAsync();
                Button button = new Button();
                button.HorizontalAlignment = HorizontalAlignment.Center;
                button.VerticalAlignment = VerticalAlignment.Center;
                button.Content = "刷新";
                button.Click += Button_Click;
                mygrid.Children.Add(button);
            }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            listview1.SelectedIndex = 0;
            this.RegisterBackgroundTask();
            await FirstStep();
        }

        private async void BuyTicketButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Datum1 data = button.DataContext as Datum1;
            var uri = new Uri(data.more.data[0].link);
            await Windows.System.Launcher.LaunchUriAsync(uri);
        }

        private async void InformationButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Datum1 data = button.DataContext as Datum1;
            var uri = new Uri(data.more.data[1].link);
            await Windows.System.Launcher.LaunchUriAsync(uri);
        }

        private async void MovieDiscussButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Datum1 data = button.DataContext as Datum1;
            var uri = new Uri(data.more.data[2].link);
            await Windows.System.Launcher.LaunchUriAsync(uri);
        }

        private async void Favorite_Click(object sender, RoutedEventArgs e)
        {
            sqliteHelper.CreateDB();
            Button button = sender as Button;
            Datum1 data = button.DataContext as Datum1;
            List<FavoriteData> fav=sqliteHelper.CheckData(data.tvTitle);
            int CheckDataResult=0;
            foreach(FavoriteData i in fav)
            {
                if (i.tvTitle == data.tvTitle)
                    CheckDataResult =1;
            }
            if (CheckDataResult == 1)
            {
                var dialog = new MessageDialog("你已经收藏过了厚", "yoho");
                dialog.Commands.Add(new UICommand("确定", cmd => { }));
                var a = await dialog.ShowAsync();
            }
            else
            {
                FavoriteData Addfavorite = new FavoriteData();
                Addfavorite.tvTitle = data.tvTitle;
                Addfavorite.iconaddress = data.iconaddress;
                Addfavorite.grade = data.grade;
                Addfavorite.gradeNum = data.gradeNum;
                Addfavorite.data2 = data.playDate.data2;
                Addfavorite.director1 = data.director.data.a.name;
                if (data.director.data.b != null)
                    Addfavorite.director2 = data.director.data.b.name;
                Addfavorite.star1 = data.star.data.a.name;
                if (data.star.data.b != null)
                    Addfavorite.star2 = data.star.data.b.name;
                if (data.star.data.c != null)
                    Addfavorite.star3 = data.star.data.c.name;
                if (data.star.data.d != null)
                    Addfavorite.star4 = data.star.data.d.name;
                Addfavorite.type1 = data.type.data.a.name;
                if (data.type.data.b != null)
                    Addfavorite.type2 = data.type.data.b.name;
                if (data.type.data.c != null)
                    Addfavorite.type3 = data.type.data.c.name;
                if (data.type.data.d != null)
                    Addfavorite.type4 = data.type.data.d.name;
                if (data.type.data.e != null)
                    Addfavorite.type5 = data.type.data.e.name;
                Addfavorite.storyBrief = data.story.data.storyBrief;
                Addfavorite.MovieImage = data.more.data[1].link;
                Addfavorite.MovieDiscuss = data.more.data[2].link;
                Addfavorite.storyMoreLink = data.story.data.storyMoreLink;
                sqliteHelper.AddData(Addfavorite);
            }
        }
        private async void CommandInvokedHandler(IUICommand command)
        {
            await FirstStep();
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            mygrid.Children.Remove(sender as Button);
            await FirstStep();
        }
        private async void RegisterBackgroundTask()
        {
            var backgroundAccessStatus = await BackgroundExecutionManager.RequestAccessAsync();
            foreach (var task in BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name == taskName)
                {
                    task.Value.Unregister(true);
                }
            }

            BackgroundTaskBuilder taskBuilder = new BackgroundTaskBuilder();
            taskBuilder.Name = taskName;
            taskBuilder.TaskEntryPoint = taskEntryPoint;
            taskBuilder.SetTrigger(new TimeTrigger(15, false));
            var registration = taskBuilder.Register();
        }
    }
}
