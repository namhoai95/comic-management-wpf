using System;
using System.Collections.Generic;
using System.Drawing;
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
using System.Windows.Shapes;
using QuanLyTruyenTranh.Form;
using QuanLyTruyenTranh.Helper;
using QuanLyTruyenTranh.Helper.Log;
using QuanLyTruyenTranh.Helper.Paging;
using QuanLyTruyenTranh.Model;
using QuanLyTruyenTranh.ViewModel;
using MessageBox = System.Windows.MessageBox;

namespace QuanLyTruyenTranh.UserControl
{
    /// <summary>
    /// Interaction logic for ucDanhSachTruyenTranh.xaml
    /// </summary>
    public partial class ucDanhSachTruyenTranh : System.Windows.Controls.UserControl
    {
        private static readonly Logger log = new Logger("Màn hình danh sách truyện tranh");
        TruyenTranhViewModel truyenTranhViewModel = new TruyenTranhViewModel();

        public ucDanhSachTruyenTranh()
        {
            InitializeComponent();
            this.DataContext = truyenTranhViewModel;
        }

        private void btnThemTruyenTranh_Click(object sender, RoutedEventArgs e)
        {
            var frm = new frmThemTruyenTranh();
            frm.flag = 0;
            frm.AddTruyenTranh += AddTruyenTranh;
            frm.ShowDialog();
        }

        private void AddTruyenTranh(TRUYENTRANH truyenTranh)
        {
            TruyenTranhViewModel.AddTruyenTranh(truyenTranh);
            lbSuccess.Content = "Thêm thành công";
            this.DataContext = new TruyenTranhViewModel();
        }

        private void UpdateTruyenTranh(TRUYENTRANH truyenTranh)
        {
            TruyenTranhViewModel.UpdateTruyenTranh(truyenTranh);
            lbSuccess.Content = "Cập nhật thành công";
            this.DataContext = new TruyenTranhViewModel();
        }


        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (dgDanhSachTruyenTranh.SelectedValue != null)
            {
                int id = Convert.ToInt32(dgDanhSachTruyenTranh.SelectedValue);
                var truyenTranh = TruyenTranhViewModel.GetTruyenTranhById(id);

                if (id != 0)
                {
                    DialogResult dialogResult =
                        (DialogResult)MessageBox.Show(String.Format("Bạn có chắc chắn muốn xóa truyện tranh {0} không",
                            truyenTranh.TenTruyenTranh),
                            "Thông báo",
                            MessageBoxButton.YesNo);

                    if (dialogResult == DialogResult.Yes)
                    {

                        TruyenTranhViewModel.DeleteTruyenTranh(truyenTranh);
                        lbSuccess.Content = "Xóa thành công";
                        log.LogInfo(string.Format("Tài khoản {0} đã xóa truyện tranh {1}", AccountLogin.AccountLogged.TenTaiKhoan, truyenTranh.TenTruyenTranh));
                        this.DataContext = new TruyenTranhViewModel();
                    }
                }
                else
                {
                    lbSuccess.Content = "Vui lòng chọn truyện tranh cần xóa";
                    lbSuccess.Foreground = new SolidColorBrush(Colors.Red);
                }
            }
        }

        private void dgDanhSachTruyenTranh_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ShowFormUpdateTruyenTranh();
        }

        private void ShowFormUpdateTruyenTranh()
        {
            if (dgDanhSachTruyenTranh.SelectedValue != null)
            {
                int id = Convert.ToInt32(dgDanhSachTruyenTranh.SelectedValue);
                var frm = new frmThemTruyenTranh(id);
                frm.flag = 1;
                frm.UpdateTruyenTranh += UpdateTruyenTranh;
                frm.ShowDialog();
            }
        }


        private void btnCapNhat_Click(object sender, RoutedEventArgs e)
        {
            if (dgDanhSachTruyenTranh.SelectedValue != null)
            {
                ShowFormUpdateTruyenTranh();
            }
            else
            {
                lbSuccess.Content = "Vui lòng chọn truyện tranh cần cập nhật";
                lbSuccess.Foreground = new SolidColorBrush(Colors.Red);
            }
        }

        private void btnImportExcel_Click(object sender, RoutedEventArgs e)
        {
            var frm = new frmImportTruyenTranh();
            frm.ImportExcel += ImportExcel;
            frm.ShowDialog();
        }

        private void ImportExcel(List<TRUYENTRANH> list)
        {
            lbSuccess.Content = "Import thành công";
            this.DataContext = truyenTranhViewModel = new TruyenTranhViewModel();
            truyenTranhViewModel.MoveToLastPage();
        }
    }
}
