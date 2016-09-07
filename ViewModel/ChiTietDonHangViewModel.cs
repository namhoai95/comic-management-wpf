using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyTruyenTranh.Model;

namespace QuanLyTruyenTranh.ViewModel
{
    public class ChiTietDonHangViewModel : ViewModelBase
    {
        private ObservableCollection<ChiTietDonHang> _ChiTietDonHang;

        public ObservableCollection<ChiTietDonHang> ChiTietDonHang
        {
            get
            {
                return _ChiTietDonHang;
            }
            set
            {
                _ChiTietDonHang = value;
                OnPropertyChanged("ChiTietDonHang");
            }
        }

        public ChiTietDonHangViewModel()
        {
            _ChiTietDonHang = GetAllChiTietDonHang();
        }

        public ObservableCollection<ChiTietDonHang> GetAllChiTietDonHang()
        {
            var chiTietDonHangs = TruyenTranhDB.Instance.ChiTietDonHangs.ToList();

            ObservableCollection<ChiTietDonHang> list = new ObservableCollection<ChiTietDonHang>();
            foreach (var chiTietDonHang in chiTietDonHangs)
            {
                list.Add(chiTietDonHang);
            }

            return list;
        }

        public static void AddChiTietDonHang(ChiTietDonHang chiTietDonHang)
        {
            TruyenTranhDB.Instance.ChiTietDonHangs.Add(chiTietDonHang);
            TruyenTranhDB.Instance.SaveChanges();
        }
        
        
    }
}
