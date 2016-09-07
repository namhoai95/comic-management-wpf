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
using System.Windows.Shapes;
using QuanLyTruyenTranh.Helper;
using QuanLyTruyenTranh.Helper.Log;
using QuanLyTruyenTranh.Model;
using QuanLyTruyenTranh.ViewModel;

namespace QuanLyTruyenTranh.Form
{
    /// <summary>
    /// Interaction logic for frmThemTacGia.xaml
    /// </summary>
    public partial class frmThemTacGia : Window
    {
        public frmThemTacGia()
        {
            InitializeComponent();
            txtTenTacGia.Focus();
        }

    }
}
