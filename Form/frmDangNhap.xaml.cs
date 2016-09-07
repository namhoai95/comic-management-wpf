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
using QuanLyTruyenTranh.ViewModel;

namespace QuanLyTruyenTranh.Form
{
    /// <summary>
    /// Interaction logic for frmDangNhap.xaml
    /// </summary>
    public partial class frmDangNhap : Window
    {
        private static readonly Logger log = new Logger("Màn hình đăng nhập");
        public frmDangNhap()
        {
            InitializeComponent();
        }

        private void btnDangNhap_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }

        private void Login()
        {
            string tenTaiKhoan = txtTenTaiKhoan.Text;
            string matKhau = pbMatKhau.Password;

            var taiKhoan = TaiKhoanViewModel.CheckTaiKhoan(tenTaiKhoan, matKhau);
            if (taiKhoan != null)
            {
                if (taiKhoan.BiXoa == true)
                {
                    lbError.Content = "Tài khoản bị khóa vui lòng liên hệ admin";
                }
                else
                {
                    AccountLogin.AccountLogged = taiKhoan;
                    var main = new MainWindow();

                    this.Hide();
                    main.ThoatAccount += this.Show;
                    main.ShowDialog();
                    log.LogInfo(string.Format("Tài khoản {0} đã đăng nhập vào hệ thống", tenTaiKhoan));
                }
            }
            else
            {
                lbError.Content = "Vui lòng kiểm tra tên tài khoản hoặc mật khẩu!";
                log.LogError(string.Format("Tài khoản {0} thực hiện đăng nhập sai", tenTaiKhoan));
            }
        }

        private void btnThoat_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void pbMatKhau_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Login();
            }
        }

        private void txtTenTaiKhoan_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Login();
            }
        }
    }
}
