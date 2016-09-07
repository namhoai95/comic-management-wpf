using System;
using System.Collections.Generic;
using System.IO;
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
using Fluent;
using QuanLyTruyenTranh.Helper;
using QuanLyTruyenTranh.Model;
using QuanLyTruyenTranh.UserControl;
using QuanLyTruyenTranh.ViewModel;

namespace QuanLyTruyenTranh
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        public delegate void Thoat();
        public event Thoat ThoatAccount;

        public MainWindow()
        {
            InitializeComponent();
            if (AccountLogin.AccountLogged.MaQuyen != 1)
            {
                rtiQuanLy.Visibility = Visibility.Collapsed;
            }
        }

        private void btnQuanLyTacGia_Click(object sender, RoutedEventArgs e)
        {
            var header = new TextBlock {Text = "Tác giả", Margin = new Thickness(0,0,5,0)};
            ucDanhSachTacGia uc = new ucDanhSachTacGia();

            var tab = new CTabItem();
            tab.SetHeader(header);
            tab.Name = "TacGia";
            tab.Content = uc;

            if (ExistForm(tab))
            {
                return;
            }
            else
            {
                tbContent.Items.Add(tab);
                tbContent.SelectedItem = tab;
            }
           
        }

        private bool ExistForm(CTabItem item)
        {
            foreach (CTabItem f in tbContent.Items)
            {
                if (f.Name == item.Name)
                {

                    f.IsSelected = true;
                    return true;
                }
            }
            return false;
        }

        private void btnQuanLyNhaXuatBan_Click(object sender, RoutedEventArgs e)
        {
            var header = new TextBlock { Text = "Nhà xuất bản" ,Margin = new Thickness(0, 0, 5, 0) };
            ucDanhSachNhaXuatBan uc = new ucDanhSachNhaXuatBan();

            var tab = new CTabItem();
            tab.SetHeader(header);
            tab.Name = "NhaXuatBan";
            tab.Content = uc;

            if (ExistForm(tab))
            {
                return;
            }
            else
            {
                tbContent.Items.Add(tab);
                tbContent.SelectedItem = tab;
            }
        }

        private void btnQuanLyTheLoai_Click(object sender, RoutedEventArgs e)
        {
            var header = new TextBlock { Text = "Thể loại" , Margin = new Thickness(0, 0, 5, 0) };
            ucDanhSachTheLoai uc = new ucDanhSachTheLoai();

            var tab = new CTabItem();
            tab.SetHeader(header);
            tab.Name = "TheLoai";
            tab.Content = uc;

            if (ExistForm(tab))
            {
                return;
            }
            else
            {
                tbContent.Items.Add(tab);
                tbContent.SelectedItem = tab;
            }
        }

        private void btnQuanLyTruyenTranh_Click(object sender, RoutedEventArgs e)
        {
            var header = new TextBlock { Text = "Truyện tranh", Margin = new Thickness(0, 0, 5, 0) };
            ucDanhSachTruyenTranh uc = new ucDanhSachTruyenTranh();

            var tab = new CTabItem();
            tab.SetHeader(header);
            tab.Name = "TruyenTranh";
            tab.Content = uc;

            if (ExistForm(tab))
            {
                return;
            }
            else
            {
                tbContent.Items.Add(tab);
                tbContent.SelectedItem = tab;
            }
        }

        private void btnThoat_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            ThoatAccount();
        }

        private void btnTroGiup_Click(object sender, RoutedEventArgs e)
        {
            Help.ShowHelp(null, Directory.GetCurrentDirectory() + "\\Help\\HelpQuanLyTruyenTranh.chm");
        }

        private void btnQuanLyKhachHang_Click(object sender, RoutedEventArgs e)
        {
            var header = new TextBlock { Text = "Khách hàng", Margin = new Thickness(0, 0, 5, 0) };
            ucDanhSachKhachHang uc = new ucDanhSachKhachHang();

            var tab = new CTabItem();
            tab.SetHeader(header);
            tab.Name = "KhachHang";
            tab.Content = uc;

            if (ExistForm(tab))
            {
                return;
            }
            else
            {
                tbContent.Items.Add(tab);
                tbContent.SelectedItem = tab;
            }
        }

        private void btnQuanLyNhanVien_Click(object sender, RoutedEventArgs e)
        {
            var header = new TextBlock { Text = "Nhân viên", Margin = new Thickness(0, 0, 5, 0) };
            ucDanhSachNhanVien uc = new ucDanhSachNhanVien();

            var tab = new CTabItem();
            tab.SetHeader(header);
            tab.Name = "NhanVien";
            tab.Content = uc;

            if (ExistForm(tab))
            {
                return;
            }
            else
            {
                tbContent.Items.Add(tab);
                tbContent.SelectedItem = tab;
            }
        }

        private void btnBanHang_Click(object sender, RoutedEventArgs e)
        {
            var header = new TextBlock { Text = "Bán hàng", Margin = new Thickness(0, 0, 5, 0) };
            ucBanHang uc = new ucBanHang();

            var tab = new CTabItem();
            tab.SetHeader(header);
            tab.Name = "BanHang";
            tab.Content = uc;

            if (ExistForm(tab))
            {
                return;
            }
            else
            {
                tbContent.Items.Add(tab);
                tbContent.SelectedItem = tab;
            }
        }

        private void RibbonWindow_Closed(object sender, EventArgs e)
        {
            this.Close();
            ThoatAccount();
        }

        private void btnBackUp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string filename = ShowDialogBackUp();
                if (filename != "DONOTHING")
                {
                    using (QuanLyTruyenTranhEntities db = new QuanLyTruyenTranhEntities())
                    {
                        string backupQuery = @"BACKUP DATABASE ""{0}"" TO DISK = N'{1}'";
                        backupQuery = string.Format(backupQuery, "QuanLyTruyenTranh", filename);
                        db.Database.SqlQuery<object>(backupQuery).ToList().FirstOrDefault();
                    }
                    System.Windows.Forms.MessageBox.Show("Sao lưu thành công!", "Thông báo");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }
        }

        private string ShowDialogBackUp()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "BAK file (*.bak)|*.bak";
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return saveFileDialog1.FileName;
            }
            else
            {
                return "DONOTHING";
            }
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string filename = ShowDialogRestore();
                if (filename != "DONOTHING")
                {
                    using (QuanLyTruyenTranhEntities db = new QuanLyTruyenTranhEntities())
                    {
                        string restoreQuery = @"USE [Master]; 
                                                ALTER DATABASE ""{0}"" SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                                                RESTORE DATABASE ""{0}"" FROM DISK='{1}' WITH REPLACE;
                                                ALTER DATABASE ""{0}"" SET MULTI_USER;";
                        restoreQuery = string.Format(restoreQuery, "QuanLyTruyenTranh", filename);
                        var list = db.Database.SqlQuery<object>(restoreQuery).ToList();
                        var resut = list.FirstOrDefault();
                    }
                    System.Windows.MessageBox.Show("Khôi phục thành công!", "Thông báo");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }
        }

        private string ShowDialogRestore()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = "C:\\";
            openFileDialog1.Filter = "BAK file (*.bak)|*.bak";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return openFileDialog1.FileName;
            }
            else
            {
                return "DONOTHING";
            }
        }

        private void btnViewLog_Click(object sender, RoutedEventArgs e)
        {
            Help.ShowHelp(null, Directory.GetCurrentDirectory() + "\\Logs\\QuanLyTruyenTranhLog.txt");
        }

        private void btnDanhSachDonHang_Click(object sender, RoutedEventArgs e)
        {
            var header = new TextBlock { Text = "Danh sách đơn hàng", Margin = new Thickness(0, 0, 5, 0) };
            ucDanhSachDonHang uc = new ucDanhSachDonHang();

            var tab = new CTabItem();
            tab.SetHeader(header);
            tab.Name = "DanhSachDonHang";
            tab.Content = uc;

            if (ExistForm(tab))
            {
                return;
            }
            else
            {
                tbContent.Items.Add(tab);
                tbContent.SelectedItem = tab;
            }
        }

        private void btnBaoCao_Click(object sender, RoutedEventArgs e)
        {
            var header = new TextBlock { Text = "Báo cáo", Margin = new Thickness(0, 0, 5, 0) };
            ucBaoCao uc = new ucBaoCao();

            var tab = new CTabItem();
            tab.SetHeader(header);
            tab.Name = "BaoCao";
            tab.Content = uc;

            if (ExistForm(tab))
            {
                return;
            }
            else
            {
                tbContent.Items.Add(tab);
                tbContent.SelectedItem = tab;
            }
        }
    }
}
