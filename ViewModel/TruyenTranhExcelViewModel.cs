using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyTruyenTranh.Model;

namespace QuanLyTruyenTranh.ViewModel
{
    public class TruyenTranhExcelViewModel : ViewModelBase
    {
        private ObservableCollection<TRUYENTRANH> _TruyenTranh;

        public ObservableCollection<TRUYENTRANH> TruyenTranh
        {
            get { return _TruyenTranh; }
            set
            {
                _TruyenTranh = value;
                OnPropertyChanged("TruyenTranh");
            }
        }

        public TruyenTranhExcelViewModel()
        {
            TruyenTranh = new ObservableCollection<TRUYENTRANH>();
        }

        public void AddListTruyenTranh(List<TRUYENTRANH> list)
        {
            TruyenTranhDB.Instance.TRUYENTRANHs.AddRange(list);
            TruyenTranhDB.Instance.SaveChanges();
        }

        public void AddListTruyenTranh(TRUYENTRANH truyentranh)
        {          
            TruyenTranh.Add(truyentranh);
        }
    }
}
