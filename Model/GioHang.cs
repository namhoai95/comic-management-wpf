using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyTruyenTranh.ViewModel;

namespace QuanLyTruyenTranh.Model
{
    public class GioHang
    {
        public double TongTien { get; set; }
        public bool? DaThanhToan { get; set; }
        public ChiTietGioHang ChiTietGioHang { get; set; }

        public GioHang()
        {
            ChiTietGioHang = new ChiTietGioHang();
        }
    }
}
