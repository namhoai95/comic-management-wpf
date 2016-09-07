using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for frmThemKhachHang.xaml
    /// </summary>
    public partial class frmThemKhachHang : Window
    {
        public int flag = 0;
        private int id;

        public delegate void AddingKhachHang(KhachHang khachHang);
        public event AddingKhachHang AddKhachHang;

        public delegate void UpdatingKhachHang(KhachHang khachHang);
        public event UpdatingKhachHang UpdateKhachHang;

      

        public frmThemKhachHang()
        {
            InitializeComponent();
        }

        public frmThemKhachHang(int id)
        {
            InitializeComponent();
            this.id = id;
            LoadKhachHangById(id);
        }

        private void LoadKhachHangById(int id)
        {
            var khachHang = KhachHangViewModel.GetKhachHangById(id);
            txtTenKhachHang.Text = khachHang.TenKhachHang;
            txtDiaChi.Text = khachHang.DiaChi;
            txtSoDienThoai.Text = khachHang.SoDienThoai;

            dpNgaySinh.Text = khachHang.NgaySinh.ToString();

            cbBiXoa.Visibility = Visibility.Visible;
            cbBiXoa.IsChecked = khachHang.BiXoa;

            txtTenKhachHang.Focus();
            btnThemKhachHang.Content = "Cập nhật";
            this.Title = "Cập nhật khách hàng";
        }

        private void AddKH()
        {
            if (isValid())
            {
                string tenKhachHang = txtTenKhachHang.Text;
                string diaChi = txtDiaChi.Text;
                string ngaySinh = dpNgaySinh.Text;
                string soDienThoai = txtSoDienThoai.Text;

                KhachHang khachHang = new KhachHang();
                khachHang.TenKhachHang = tenKhachHang;
                khachHang.DiaChi = diaChi;
                khachHang.SoDienThoai = soDienThoai;
                khachHang.NgaySinh = DateTime.ParseExact(ngaySinh, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                khachHang.BiXoa = false;

                Logger log= new Logger("Màn hình thêm khách hàng");
                AddKhachHang(khachHang);
                
                log.LogInfo(string.Format("Tài khoản {0} đã thêm khách hàng {1}", AccountLogin.AccountLogged.TenTaiKhoan, khachHang.TenKhachHang));
                this.Close();
            }
        }

        private bool isValid()
        {
            bool valid = true;
            if (String.IsNullOrEmpty(txtTenKhachHang.Text))
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng");
                valid = false;
                txtTenKhachHang.Focus();

            }
            else if (String.IsNullOrEmpty(txtDiaChi.Text))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ");
                valid = false;
                txtDiaChi.Focus();
            }
            else if (String.IsNullOrEmpty(txtSoDienThoai.Text))
            {
                Regex regex = new Regex(@"0\d{ 9, 10 }");
                if (!regex.IsMatch(txtSoDienThoai.Text))
                {
                    MessageBox.Show("Vui lòng nhập đúng số điện thoại");
                    valid = false;
                    txtSoDienThoai.Focus();
                }
            }
            else if (String.IsNullOrEmpty(dpNgaySinh.Text))
            {
                MessageBox.Show("Vui lòng chọn ngày sinh");
                valid = false;
                dpNgaySinh.Focus();
            }
            else if (Convert.ToDateTime(dpNgaySinh.Text).Year >= DateTime.Now.Year)
            {
                MessageBox.Show("Vui lòng chọn năm sinh nhỏ hơn năm hiện tại");
                valid = false;
                dpNgaySinh.Focus();
            }

            return valid;
        }

        private void txtSoDienThoai_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }
        }

        private void btnThemKhachHang_Click(object sender, RoutedEventArgs e)
        {
            if (flag == 0)
            {
                AddKH();
            }
            else
            {
                UpdateKH();
            }
        }

        private void UpdateKH()
        {
            if (isValid())
            {
                string tenKhachHang = txtTenKhachHang.Text;
                string diaChi = txtDiaChi.Text;
                string ngaySinh = dpNgaySinh.Text;
                string soDienThoai = txtSoDienThoai.Text;

                KhachHang khachHang = KhachHangViewModel.GetKhachHangById(id);
                khachHang.TenKhachHang = tenKhachHang;
                khachHang.DiaChi = diaChi;
                khachHang.SoDienThoai = soDienThoai;
                khachHang.NgaySinh = DateTime.ParseExact(ngaySinh, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                khachHang.BiXoa = cbBiXoa.IsChecked;

                Logger log = new Logger("Màn hình cập nhật khách hàng");
                UpdateKhachHang(khachHang);
                log.LogInfo(string.Format("Tài khoản {0} đã cập nhật khách hàng {1}", AccountLogin.AccountLogged.TenTaiKhoan, khachHang.MaKhachHang));
                this.Close();
            }
        }

        private void btnHuy_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
