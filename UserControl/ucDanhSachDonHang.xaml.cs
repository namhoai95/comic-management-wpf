using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using QuanLyTruyenTranh.ViewModel;

namespace QuanLyTruyenTranh.UserControl
{
    /// <summary>
    /// Interaction logic for ucDanhSachDonHang.xaml
    /// </summary>
    public partial class ucDanhSachDonHang : System.Windows.Controls.UserControl
    {
        public ucDanhSachDonHang()
        {
            InitializeComponent();
            this.DataContext = new DonHangViewModel();
        }
    }
}
