using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Movie_Recommend.Model;
using System.Collections.ObjectModel;

namespace Movie_Recommend.HelperClass
{
    class SQLiteHelper
    {
        public string DbName = "FavoriteData.db";
        public string DbPath;
        internal SQLite.Net.SQLiteConnection GetCreateConn()
        {
            DbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DbName);
            var con = new SQLite.Net.SQLiteConnection(new SQLitePlatformWinRT(), DbPath);

            return con;

        }
        internal void CreateDB()
        { 
            DbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DbName);
            using (var conn = GetCreateConn())
            {
                conn.CreateTable<FavoriteData>();

            }
        }
        internal int AddData(FavoriteData AddFavorite)
        {
            int result = 0;
            using (var conn = GetCreateConn())
            {
                result = conn.Insert(AddFavorite);
            }

            return result;
        }
        internal int DeleteData(FavoriteData FavoriteUID)
        {
            int result = 0;
            DbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DbName);
            using (var conn = GetCreateConn())
            {
                result = conn.Delete(FavoriteUID);
            }
            return result;
        }
        internal void UpadateData(string deleteSqliteSequence)
        {
            DbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DbName);
        }
        internal int UpadateData(FavoriteData updataFavoriteData)
        {
            int result = 0;
            DbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DbName);
            using (var conn = GetCreateConn())
            {
                result = conn.Update(updataFavoriteData);
            }
            return result;
        }
        internal List<FavoriteData> CheckData(string conditions)
        {
            string Condition = "%" + conditions + "%";
            #region 
            using (var conn = GetCreateConn())
            {
                return conn.Query<FavoriteData>("select * from FavoriteData where tvTitle like?;",Condition);

            }
            #endregion
        }
        internal ObservableCollection<FavoriteData> ReadData(ObservableCollection<FavoriteData> FavoriteList)
        {
            FavoriteList.Clear();
            DbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DbName);
            CreateDB();
            using (var conn = GetCreateConn())
            {
                var dbFavoriteData = conn.Table<FavoriteData>();
                foreach (var item in dbFavoriteData)
                {
                    FavoriteList.Add(item);
                }
            }
            return FavoriteList;
        }
    }
}
