using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie_Recommend.ViewModel
{
    public class MainPageViewModel:BaseViewModel
    {
        private ObservableCollection<Model.Datum1> _recentmovielistresult;
        public ObservableCollection<Model.Datum1> RecentMovieListResult
        {
            get
            {
                return _recentmovielistresult;
            }
            set
            {
                _recentmovielistresult = value;
                RaisePropertyChanged(nameof(RecentMovieListResult));
            }
        }
        private ObservableCollection<Model.Datum1> _recentmovielistresult1;
        public ObservableCollection<Model.Datum1> RecentMovieListResult1
        {
            get
            {
                return _recentmovielistresult1;
            }
            set
            {
                _recentmovielistresult1 = value;
                RaisePropertyChanged(nameof(RecentMovieListResult1));
            }
        }
    }
}
