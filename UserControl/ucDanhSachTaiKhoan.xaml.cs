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
using QuanLyTruyenTranh.Form;
using QuanLyTruyenTranh.Model;
using QuanLyTruyenTranh.ViewModel;
using MessageBox = System.Windows.MessageBox;

namespace QuanLyTruyenTranh.UserControl
{
    /// <summary>
    /// Interaction logic for ucDanhSachNhanVien.xaml
    /// </summary>
    public partial class ucDanhSachNhanVien : System.Windows.Controls.UserControl
    {
        public ucDanhSachNhanVien()
        {
            InitializeComponent();
            this.DataContext = new TaiKhoanViewModel();
            dgDanhSachTaiKhoan.SelectedValuePath = "MaTaiKhoan";
        }

        private void btnThemNhanVien_Click(object sender, RoutedEventArgs e)
        {
            var frm = new frmThemTaiKhoan();
            frm.flag = 0;
            frm.AddTaiKhoan += AddTaiKhoan;
            frm.ShowDialog();
        }

        private void AddTaiKhoan(TaiKhoan info)
        {
            TaiKhoanViewModel.AddTaiKhoan(info);
            lbSuccess.Content = "Thêm thành công";
            this.DataContext = new TaiKhoanViewModel();
        }

        private void ShowFormUpdateTaiKhoan()
        {
            int id = Convert.ToInt32(dgDanhSachTaiKhoan.SelectedValue.ToString());
            var frm = new frmThemTaiKhoan(id);
            frm.flag = 1;
            frm.UpdateTaiKhoan += UpdateTaiKhoan;
            frm.ShowDialog();
        }

        private void UpdateTaiKhoan(TaiKhoan info)
        {
            TaiKhoanViewModel.UpdateTaiKhoan(info);
            lbSuccess.Content = "Cập nhật thành công";
            this.DataContext = new TaiKhoanViewModel();
        }

        private void dgDanhSachTaiKhoan_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ShowFormUpdateTaiKhoan();
        }

        private void btnCapNhat_Click(object sender, RoutedEventArgs e)
        {
            if (dgDanhSachTaiKhoan.SelectedValue != null)
            {
                ShowFormUpdateTaiKhoan();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần cập nhật", "Thông báo", MessageBoxButton.OK);
            }

        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(dgDanhSachTaiKhoan.SelectedValue);
            var taiKhoan = TaiKhoanViewModel.GetTaiKhoanById(id);

            if (id != 0)
            {
                DialogResult dialogResult =
                    (DialogResult)MessageBox.Show(String.Format("Bạn có chắc chắn muốn xóa nhân viên {0} không?",
                        taiKhoan.TenNhanVien),
                        "Thông báo",
                        MessageBoxButton.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    if (taiKhoan.MaQuyen != 1)
                    {
                        TaiKhoanViewModel.DeleteTaiKhoan(taiKhoan);
                        lbSuccess.Content = "Xóa thành công";
                        this.DataContext = new TaiKhoanViewModel();
                    }
                    else
                    {
                        MessageBox.Show("Không được phép xóa tài khoản quản lý", "Thông báo", MessageBoxButton.OK);
                    }

                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần xóa", "Thông báo", MessageBoxButton.OK);
            }
        }

    }
}
