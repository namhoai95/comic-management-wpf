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
using QuanLyTruyenTranh.Helper;
using QuanLyTruyenTranh.Helper.Log;
using QuanLyTruyenTranh.Model;
using QuanLyTruyenTranh.ViewModel;
using MessageBox = System.Windows.MessageBox;

namespace QuanLyTruyenTranh.UserControl
{
    /// <summary>
    /// Interaction logic for ucDanhSachNhaXuatBan.xaml
    /// </summary>
    public partial class ucDanhSachNhaXuatBan : System.Windows.Controls.UserControl
    {
        public ucDanhSachNhaXuatBan()
        {
            InitializeComponent();
            this.DataContext = new NhaXuatBanViewModel();
            dgDanhSachNhaXuatBan.SelectedValuePath = "MaNXB";
        }

        private void btnThemTacGia_Click(object sender, RoutedEventArgs e)
        {
            var frm = new frmThemNhaXuatBan();
            frm.flag = 0;
            frm.AddNhaXuatBan += AddNhaXuatBan;
            frm.ShowDialog();
        }

        private void AddNhaXuatBan(NHAXUATBAN nhaXuatBan)
        {
            NhaXuatBanViewModel.AddNhaXuatBan(nhaXuatBan);
            lbSuccess.Content = "Thêm thành công";
            this.DataContext = new NhaXuatBanViewModel();
        }

        private void UpdateNhaXuatBan(NHAXUATBAN nhaXuatBan)
        {
            NhaXuatBanViewModel.UpdateNhaXuatBan(nhaXuatBan);
            lbSuccess.Content = "Cập nhật thành công";
            this.DataContext = new NhaXuatBanViewModel();
        }

        private void ShowFormUpdateNhaXuatBan()
        {
            int id = Convert.ToInt32(dgDanhSachNhaXuatBan.SelectedValue.ToString());
            var frm = new frmThemNhaXuatBan(id);
            frm.flag = 1;
            frm.UpdateNhaXuatBan += UpdateNhaXuatBan;
            frm.ShowDialog();
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(dgDanhSachNhaXuatBan.SelectedValue);
            var nhaXuatBan = NhaXuatBanViewModel.GetNhaXuatBanById(id);
            string tenNhaXuatBan = nhaXuatBan.TenNXB;
            if (id != 0)
            {
                DialogResult dialogResult = (DialogResult)MessageBox.Show(String.Format("Bạn có chắc chắn muốn xóa nhà xuất bản {0} không?",
                    nhaXuatBan.TenNXB),
                    "Thông báo",
                    MessageBoxButton.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    NhaXuatBanViewModel.DeleteNhaXuatBan(nhaXuatBan);
                    lbSuccess.Content = "Xóa thành công";
                    this.DataContext = new NhaXuatBanViewModel();
                    Logger log = new Logger("Màn hình danh sách nhà xuất bản");
                    log.LogInfo(string.Format("Tài khoản {0} đã xóa nhà xuất bản {1}", AccountLogin.AccountLogged.TenTaiKhoan, tenNhaXuatBan));
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn nhà xuất bản cần xóa", "Thông báo", MessageBoxButton.OK);
            }
        }

        private void dgDanhSachNhaXuatBan_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ShowFormUpdateNhaXuatBan();
        }

        private void btnCapNhat_Click(object sender, RoutedEventArgs e)
        {
            if (dgDanhSachNhaXuatBan.SelectedValue != null)
            {
                ShowFormUpdateNhaXuatBan();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn nhà xuất bản cần cập nhật", "Thông báo", MessageBoxButton.OK);
            }
        }
    }
}
