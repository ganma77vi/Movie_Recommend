using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie_Recommend.ViewModel
{
    class CinemaRecommendViewModel:BaseViewModel
    {
        private ObservableCollection<Model.Pois> _poiresult;
        public ObservableCollection<Model.Pois> poiresult
        {
            get
            {
                return _poiresult;
            }
            set
            {
                _poiresult = value;
                RaisePropertyChanged(nameof(poiresult));
            }
        }
    }
}
