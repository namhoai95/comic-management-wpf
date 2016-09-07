using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyTruyenTranh.Model;

namespace QuanLyTruyenTranh.ViewModel
{
    public class PhanQuyenViewModel
    {
        public static List<PhanQuyen> GetListPhanQuyen()
        {
            return TruyenTranhDB.Instance.PhanQuyens.ToList();
        }
    }
}
