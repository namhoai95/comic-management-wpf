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
    /// Interaction logic for frmThemTaiKhoan.xaml
    /// </summary>
    public partial class frmThemTaiKhoan : Window
    {
        public int flag = 0;
        private int id;

        public delegate void AddingTaiKhoan(TaiKhoan taiKhoan);
        public event AddingTaiKhoan AddTaiKhoan;

        public delegate void UpdatingTaiKhoan(TaiKhoan taiKhoan);
        public event UpdatingTaiKhoan UpdateTaiKhoan;

        private static readonly Logger log = new Logger("Màn hình thêm tài khoản");
        public frmThemTaiKhoan()
        {
            InitializeComponent();
            LoadComboboxQuyen();
            txtTenTaiKhoan.Focus();
        }

        public frmThemTaiKhoan(int id)
        {
            InitializeComponent();
            this.id = id;
            LoadComboboxQuyen();
            LoadTaiKhoanById(id);
        }

        private void LoadTaiKhoanById(int id)
        {
            var taiKhoan = TaiKhoanViewModel.GetTaiKhoanById(id);

            txtTenTaiKhoan.Text = taiKhoan.TenTaiKhoan;
            pbMatKhau.Password = taiKhoan.MatKhau;
            txtTenNhanVien.Text = taiKhoan.TenNhanVien;
            cbQuyen.SelectedValue = taiKhoan.MaQuyen;
            cbBiXoa.Visibility = Visibility.Visible;
            cbBiXoa.IsChecked = taiKhoan.BiXoa;

            txtTenTaiKhoan.Focus();
            btnThemNhanVien.Content = "Cập nhật";
            this.Title = "Cập Nhật Nhân Viên";
        }


        private void LoadComboboxQuyen()
        {
            cbQuyen.ItemsSource = PhanQuyenViewModel.GetListPhanQuyen();
            cbQuyen.DisplayMemberPath = "TenQuyen";
            cbQuyen.SelectedValuePath = "MaQuyen";
        }

        private void btnHuy_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private bool isValid()
        {
            bool valid = true;

            if (String.IsNullOrEmpty(txtTenTaiKhoan.Text))
            {
                MessageBox.Show("Vui lòng nhập tên tài khoản");
                txtTenTaiKhoan.Focus();
                valid = false;
            }
            else if (String.IsNullOrEmpty(pbMatKhau.Password))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu");
                pbMatKhau.Focus();
                valid = false;
            }
            else if (String.IsNullOrEmpty(txtTenNhanVien.Text))
            {
                MessageBox.Show("Vui lòng nhập tên nhân viên");
                txtTenNhanVien.Focus();
                valid = false;
            }
            else if (cbQuyen.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn quyền");
                cbQuyen.Focus();
                valid = false;
            }

            return valid;
        }


        private void btnThemNhanVien_Click(object sender, RoutedEventArgs e)
        {
            if (flag == 0)
            {
                AddTK();
            }
            else
            {
                UpdateTK();
            }
        }

        private void AddTK()
        {
            if (isValid())
            {
                string tenTaiKhoan = txtTenTaiKhoan.Text;
                string matKhau = pbMatKhau.Password;
                string tenNhanVien = txtTenNhanVien.Text;
                int maQuyen = Convert.ToInt32(cbQuyen.SelectedValue);

                TaiKhoan taiKhoan = new TaiKhoan();
                taiKhoan.TenTaiKhoan = tenTaiKhoan;
                taiKhoan.MatKhau = matKhau;
                taiKhoan.TenNhanVien = tenNhanVien;
                taiKhoan.MaQuyen = maQuyen;
                taiKhoan.BiXoa = false;

                AddTaiKhoan(taiKhoan);
                log.LogInfo(string.Format("Tài khoản {0} đã thêm tài khoản {1}", AccountLogin.AccountLogged.MaTaiKhoan, taiKhoan.MaTaiKhoan));
                this.Close();
            }
        }

        private void UpdateTK()
        {
            if (isValid())
            {
                string tenTaiKhoan = txtTenTaiKhoan.Text;
                string matKhau = pbMatKhau.Password;
                string tenNhanVien = txtTenNhanVien.Text;
                int maQuyen = Convert.ToInt32(cbQuyen.SelectedValue);

                TaiKhoan taiKhoan = TaiKhoanViewModel.GetTaiKhoanById(id);
                if (taiKhoan.MaQuyen == 1)
                {
                    MessageBox.Show("Không thể cập nhật quản lý");
                }
                else
                {
                    taiKhoan.TenTaiKhoan = tenTaiKhoan;
                    taiKhoan.MatKhau = matKhau;
                    taiKhoan.TenNhanVien = tenNhanVien;
                    taiKhoan.MaQuyen = maQuyen;
                    taiKhoan.BiXoa = cbBiXoa.IsChecked;

                    UpdateTaiKhoan(taiKhoan);
                    log.LogInfo(string.Format("Tài khoản {0} đã cập nhật tài khoản {1}", AccountLogin.AccountLogged.TenTaiKhoan, taiKhoan.TenTaiKhoan));
                    this.Close();
                }
            }
        }
    }
}
