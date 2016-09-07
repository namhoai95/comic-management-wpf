using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using QuanLyTruyenTranh.Model;
using QuanLyTruyenTranh.ViewModel;
using QuanLyTruyenTranh.Form;
using QuanLyTruyenTranh.Helper;
using QuanLyTruyenTranh.Helper.Log;
using MessageBox = System.Windows.MessageBox;

namespace QuanLyTruyenTranh.UserControl
{
    /// <summary>
    /// Interaction logic for ucDanhSachKhachHang.xaml
    /// </summary>
    public partial class ucDanhSachKhachHang : System.Windows.Controls.UserControl
    {
        public ucDanhSachKhachHang()
        {
            InitializeComponent();
            this.DataContext = new KhachHangViewModel();
        }

        private void btnThemKhachHang_Click(object sender, RoutedEventArgs e)
        {
            var frm = new frmThemKhachHang();
            frm.flag = 0;
            frm.AddKhachHang += AddKhachHang;
            frm.ShowDialog();
        }

        private void AddKhachHang(KhachHang khachHang)
        {
            KhachHangViewModel.AddKhachHang(khachHang);
            lbSuccess.Content = "Thêm thành công";
            this.DataContext = new KhachHangViewModel();
        }

        private void btnCapNhat_Click(object sender, RoutedEventArgs e)
        {
            if (dgDanhSachKhachHang.SelectedValue != null)
            {
                ShowFormUpdateKhachHang();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần cập nhật", "Thông báo", MessageBoxButton.OK);
            }
        }

        private void ShowFormUpdateKhachHang()
        {
            int id = Convert.ToInt32(dgDanhSachKhachHang.SelectedValue.ToString());
            var frm = new frmThemKhachHang(id);
            frm.flag = 1;
            frm.UpdateKhachHang += UpdateKhachHang;
            frm.ShowDialog();
        }

        private void dgDanhSachKhachHang_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ShowFormUpdateKhachHang();
        }

        private void UpdateKhachHang(KhachHang khachHang)
        {
            KhachHangViewModel.UpdateKhachHang(khachHang);
            lbSuccess.Content = "Cập nhật thành công";
            this.DataContext = new KhachHangViewModel();
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(dgDanhSachKhachHang.SelectedValue);
            var khachHang = KhachHangViewModel.GetKhachHangById(id);
            string tenKhachHang = khachHang.TenKhachHang;
            if (id != 0)
            {
                DialogResult dialogResult = (DialogResult)MessageBox.Show(String.Format("Bạn có chắc chắn muốn xóa khách hàng {0} không?",
                    khachHang.TenKhachHang),
                    "Thông báo",
                    MessageBoxButton.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    if(KhachHangViewModel.CheckKhachHangHaveDonHang(khachHang))
                    {
                        KhachHangViewModel.DeleteKhachHang(khachHang);
                        lbSuccess.Content = "Xóa thành công";
                        this.DataContext = new KhachHangViewModel();
                        Logger log = new Logger("Màn hình danh sách khách hàng");
                        log.LogInfo(string.Format("Tài khoản {0} đã xóa khách hàng {1}", AccountLogin.AccountLogged.TenTaiKhoan, tenKhachHang));
                     }
                    else
                    {
                        MessageBox.Show("Khách hàng không thể xóa", "Thông báo", MessageBoxButton.OK);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần xóa", "Thông báo", MessageBoxButton.OK);
            }
        }
    }
}
