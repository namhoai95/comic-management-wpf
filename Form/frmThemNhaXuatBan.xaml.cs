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
    /// Interaction logic for frmThemNhaXuatBan.xaml
    /// </summary>
    public partial class frmThemNhaXuatBan : Window
    {
        public int flag = 0;
        private int id;

        public delegate void AddingNhaXuatBan(NHAXUATBAN nhaXuatBan);
        public event AddingNhaXuatBan AddNhaXuatBan;

        public delegate void UpdatingNhaXuatBan(NHAXUATBAN nhaXuatBan);
        public event UpdatingNhaXuatBan UpdateNhaXuatBan;

        public frmThemNhaXuatBan()
        {
            InitializeComponent();
            txtTenNXB.Focus();
        }

        public frmThemNhaXuatBan(int id)
        {
            InitializeComponent();
            this.id = id;
            LoadNhaXuatBanById(id);
            txtTenNXB.Focus();
        }

        private void AddNXB()
        {
            NHAXUATBAN nhaXuatBan = new NHAXUATBAN();

            string tenNhaXuatBan = txtTenNXB.Text;
            string ghiChu = txtGhiChu.Text;

            if (String.IsNullOrEmpty(tenNhaXuatBan))
            {
                MessageBox.Show("Vui lòng nhập tên nhà xuất bản");
                txtTenNXB.Focus();
                return;
            }
            else if (String.IsNullOrEmpty(ghiChu))
            {
                ghiChu = "";
            }

            nhaXuatBan.TenNXB = tenNhaXuatBan;
            nhaXuatBan.GhiChu = ghiChu;
            nhaXuatBan.BiXoa = false;

            Logger log = new Logger("Màn hình thêm nhà xuất bản");
            AddNhaXuatBan(nhaXuatBan);
            log.LogInfo(string.Format("Tài khoản {0} đã thêm nhà xuất bản {1}", AccountLogin.AccountLogged.TenTaiKhoan, nhaXuatBan.TenNXB));
            this.Close();

        }

        private void btnThemNhaXuatBan_Click(object sender, RoutedEventArgs e)
        {
            if (flag == 0)
            {
                AddNXB();
            }
            else
            {
                UpdateNXB();
            }
        }

        private void UpdateNXB()
        {
            NHAXUATBAN nhaXuatBan = NhaXuatBanViewModel.GetNhaXuatBanById(id);

            string tenNhaXuatBan = txtTenNXB.Text;
            string ghiChu = txtGhiChu.Text;


            if (String.IsNullOrEmpty(tenNhaXuatBan))
            {
                MessageBox.Show("Vui lòng nhập tên nhà xuất bản");
                txtTenNXB.Focus();
                return;
            }
            else if (String.IsNullOrEmpty(ghiChu))
            {
                ghiChu = "";
            }

            nhaXuatBan.TenNXB = tenNhaXuatBan;
            nhaXuatBan.GhiChu = ghiChu;
            nhaXuatBan.BiXoa = cbBiXoa.IsChecked;

            Logger log = new Logger("Màn hình cập nhật nhà xuất bản");
            UpdateNhaXuatBan(nhaXuatBan);
            log.LogInfo(string.Format("Tài khoản {0} đã cập nhật nhà xuất bản {1}", AccountLogin.AccountLogged.TenTaiKhoan, nhaXuatBan.MaNXB));
            this.Close();

        }

        private void LoadNhaXuatBanById(int id)
        {
            var nhaXuatBan = NhaXuatBanViewModel.GetNhaXuatBanById(id);
            txtTenNXB.Text = nhaXuatBan.TenNXB;
            txtGhiChu.Text = nhaXuatBan.GhiChu;
            cbBiXoa.Visibility = Visibility.Visible;
            cbBiXoa.IsChecked = nhaXuatBan.BiXoa;
            btnThemNhaXuatBan.Content = "Cập nhật";
            this.Title = "Cập nhật nhà xuất bản";
        }

        private void btnHuy_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
