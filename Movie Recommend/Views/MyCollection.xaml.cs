using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Movie_Recommend.HelperClass;
using Movie_Recommend.Model;
using Movie_Recommend.Views;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace Movie_Recommend.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MyCollection : Page
    {
        ObservableCollection<FavoriteData> FavoriteList = new ObservableCollection<FavoriteData>();
        SQLiteHelper sqliteHelper = new SQLiteHelper();
        public MyCollection()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            FavoriteList.Clear();
            FavoriteList = sqliteHelper.ReadData(FavoriteList);
            Mylistview.ItemsSource = FavoriteList;
            if(FavoriteList.Count==0)
            {
                TB.Opacity = 1;
            }
            else
            {
                TB.Opacity = 0;
            }
        }

        private async void InformationButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            FavoriteData favoritedata = button.DataContext as FavoriteData;
            var uri = new Uri(favoritedata.MovieImage);
            await Windows.System.Launcher.LaunchUriAsync(uri);
        }

        private async void MovieDiscussButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            FavoriteData favoritedata = button.DataContext as FavoriteData;
            var uri = new Uri(favoritedata.MovieDiscuss);
            await Windows.System.Launcher.LaunchUriAsync(uri);
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            FavoriteData favoritedata = button.DataContext as FavoriteData;
            sqliteHelper.DeleteData(favoritedata);
            string sql = "delete from sqlite_sequence where name='FavoriteData';";
            sqliteHelper.UpadateData(sql);
            FavoriteList.Remove(favoritedata);
            if (FavoriteList.Count == 0)
            {
                TB.Opacity = 1;
            }
            else
            {
                TB.Opacity = 0;
            }
        }
    }
}
