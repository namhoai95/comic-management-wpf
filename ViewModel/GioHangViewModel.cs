using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyTruyenTranh.Model;

namespace QuanLyTruyenTranh.ViewModel
{
    public class GioHangViewModel : ViewModelBase
    {
        private ObservableCollection<GioHang> _GioHang;

        public ObservableCollection<GioHang> GioHang
        {
            get { return _GioHang; }
            set
            {
                _GioHang = value;
                OnPropertyChanged("GioHang");
            }
        }

        public GioHangViewModel()
        {
            GioHang = new ObservableCollection<GioHang>();
        }

        public void AddDetailGioHang(GioHang gioHang)
        {
            int index = -1;
            for(int i = 0; i < GioHang.Count ; i++)
            {
                if (GioHang[i].ChiTietGioHang.TruyenTranh.MaTruyenTranh == gioHang.ChiTietGioHang.TruyenTranh.MaTruyenTranh)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                GioHang.Add(gioHang);
            }
            else
            {
                var temp = GioHang[index];
                GioHang.RemoveAt(index);

                temp.ChiTietGioHang.SoLuong += (int)gioHang.ChiTietGioHang.SoLuong;
                temp.TongTien += gioHang.ChiTietGioHang.SoLuong * gioHang.ChiTietGioHang.DonGia;
                temp.DaThanhToan = gioHang.DaThanhToan;
                GioHang.Insert(index ,temp);
            }      
        }
    }
}
