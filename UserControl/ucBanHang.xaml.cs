using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls.Dialogs;
using MaterialDesignThemes.Wpf;
using QuanLyTruyenTranh.Helper;
using QuanLyTruyenTranh.Helper.Log;
using QuanLyTruyenTranh.Model;
using QuanLyTruyenTranh.ViewModel;

namespace QuanLyTruyenTranh.UserControl
{
    /// <summary>
    /// Interaction logic for ucBanHang.xaml
    /// </summary>
    public partial class ucBanHang : System.Windows.Controls.UserControl
    {
        private GioHangViewModel gioHangViewModel = new GioHangViewModel();

        public ucBanHang()
        {
            InitializeComponent();
            this.DataContext = gioHangViewModel;
            Load();
        }


        private void Load()
        {
            LoadNhanVienBanHang();
            LoadComboboxKhachHang();
            LoadComboxTruyenTranh();
            dpNgayDat.Text = DateTime.Now.ToString();
        }

        private void LoadNhanVienBanHang()
        {
            var taiKhoanLogged = AccountLogin.AccountLogged;
            txtNhanVienBanHang.Text = taiKhoanLogged.TenNhanVien;
        }
        private void LoadComboboxKhachHang()
        {
            cbxKhachHang.ItemsSource = KhachHangViewModel.GetListKhachHang();
            cbxKhachHang.DisplayMemberPath = "TenKhachHang";
            cbxKhachHang.SelectedValuePath = "MaKhachHang";
        }
        private void LoadComboxTruyenTranh()
        {
            cbxTruyenTranh.ItemsSource = TruyenTranhViewModel.GetListTruyenTranh();
            cbxTruyenTranh.DisplayMemberPath = "TenTruyenTranh";
            cbxTruyenTranh.SelectedValuePath = "MaTruyenTranh";
        }

        private void AddDetailGioHang(GioHang gioHang)
        {
            gioHangViewModel.AddDetailGioHang(gioHang);
            this.DataContext = gioHangViewModel;
        }

        private void cbxTruyenTranh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int id = Convert.ToInt32(cbxTruyenTranh.SelectedValue);
            var truyenTranh = TruyenTranhViewModel.GetTruyenTranhById(id);

            txtSoLuong.Text = "1";
            txtGia.Text = truyenTranh.Gia.ToString();
        }

        private void btnThemGioHang_Click(object sender, RoutedEventArgs e)
        {
            if (isValid())
            {
                AddGioHang();
            }

        }

        private void AddGioHang()
        {
            GioHang gioHang = new GioHang();
            double tongtien = 0;
            int id = Convert.ToInt32(cbxTruyenTranh.SelectedValue);
            var truyenTranh = TruyenTranhViewModel.GetTruyenTranhById(id);

            if (Convert.ToInt32(txtSoLuong.Text) == 0)
            {
                MessageBox.Show(
                    "Số lượng truyện tranh đã hết không thể thêm vào giỏ hàng, vui lòng chọn truyện tranh khác");
            }
            else if (cbDaThanhToan.IsChecked == false)
            {
                MessageBox.Show(
                    "Vui lòng check vào ô đã thanh toán");
            }
            else if (Convert.ToInt32(txtSoLuong.Text) < 0)
            {
                MessageBox.Show(
                    "Số lượng không đượng nhỏ hơn 0");
            }
            else
            {
                gioHang.ChiTietGioHang.TruyenTranh = truyenTranh;
                gioHang.ChiTietGioHang.DonGia = (double)truyenTranh.Gia;
                gioHang.ChiTietGioHang.SoLuong = Convert.ToInt32(txtSoLuong.Text);
                gioHang.DaThanhToan = cbDaThanhToan.IsChecked;
                gioHang.TongTien = (double)(truyenTranh.Gia * Convert.ToInt32(txtSoLuong.Text));

                AddDetailGioHang(gioHang);
                foreach (var item in gioHangViewModel.GioHang)
                {
                    tongtien += item.TongTien;
                }
                txtTongTien.Text = tongtien.ToString();
            }
        }

        private bool isValid()
        {
            bool valid = true;

            if (cbxKhachHang.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn khách hàng mua");
                cbxKhachHang.Focus();
                valid = false;
            }
            else if (cbxTruyenTranh.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn truyện tranh cần bán");
                cbxTruyenTranh.Focus();
                valid = false;
            }
            return valid;
        }

        private void btnThanhToan_Click(object sender, RoutedEventArgs e)
        {
            AddChiTietDonHang();
        }

        private void AddDonHang()
        {
            DonHang donHang = new DonHang();

            double tongTien = 0;
            int soLuong = 0;
            int id = Convert.ToInt32(cbxTruyenTranh.SelectedValue);
            var truyenTranh = TruyenTranhViewModel.GetTruyenTranhById(id);
            try
            {
                if (gioHangViewModel.GioHang.Count == 0)
                {
                    MessageBox.Show("Giỏ hàng rỗng không thể thanh toán");
                }
                else
                {
                    foreach (var gioHang in gioHangViewModel.GioHang)
                    {
                        tongTien += gioHang.ChiTietGioHang.DonGia * gioHang.ChiTietGioHang.SoLuong;
                        soLuong += gioHang.ChiTietGioHang.SoLuong;
                        if (truyenTranh.SoLuong < soLuong)
                        {
                            throw new Exception();
                        }
                    }

                    donHang.MaKhachHang = Convert.ToInt32(cbxKhachHang.SelectedValue);
                    donHang.NgayDat = DateTime.ParseExact(dpNgayDat.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    donHang.TongTien = tongTien;
                    donHang.MaKhachHang = Convert.ToInt32(cbxKhachHang.SelectedValue);
                    donHang.MaTaiKhoan = AccountLogin.AccountLogged.MaTaiKhoan;
                    donHang.MaTinhTrang = cbDaThanhToan.IsChecked == true ? 1 : 2;

                    DonHangViewModel.AddDonHang(donHang);
                }
            }
            catch (Exception)
            {

            }
        }


        private void AddChiTietDonHang()
        {
            using (var transaction = TruyenTranhDB.Instance.Database.BeginTransaction())
            {
                try
                {
                    if (gioHangViewModel.GioHang.Count != 0)
                    {
                        AddDonHang();
                        int lastIdDonHang = DonHangViewModel.GetLastMaDonHang();
                        bool flag = true;
                        ChiTietDonHang chiTietDonHang;

                        foreach (var gioHang in gioHangViewModel.GioHang)
                        {
                            chiTietDonHang = new ChiTietDonHang();

                            chiTietDonHang.MaDonHang = lastIdDonHang;
                            chiTietDonHang.MaTruyenTranh = gioHang.ChiTietGioHang.TruyenTranh.MaTruyenTranh;
                            chiTietDonHang.SoLuong = gioHang.ChiTietGioHang.SoLuong;
                            chiTietDonHang.Gia = gioHang.ChiTietGioHang.TruyenTranh.Gia * gioHang.ChiTietGioHang.SoLuong;

                            if (gioHang.ChiTietGioHang.TruyenTranh.SoLuong < chiTietDonHang.SoLuong)
                            {
                                flag = false;
                            }
                            else
                            {
                                gioHang.ChiTietGioHang.TruyenTranh.SoLuong -= chiTietDonHang.SoLuong;
                                if (gioHang.ChiTietGioHang.TruyenTranh.SoLuong < 0)
                                {
                                    throw new Exception();
                                }
                                lbSuccess.Visibility = Visibility.Visible;
                                lbSuccess.Content = "Mua thành công";
                                ChiTietDonHangViewModel.AddChiTietDonHang(chiTietDonHang);
                            }
                        }
                        if (flag == false)
                        {
                            MessageBox.Show("Vui lòng chọn mua sản phẩm khác vì số lượng hiện tại đã hết");
                        }
                        transaction.Commit();
                        this.DataContext = gioHangViewModel = null;
                        gioHangViewModel = new GioHangViewModel();
                        Logger log = new Logger("Màn hình bán hàng");
                        log.LogInfo(string.Format("Tài khoản {0} đã thực hiện bán hàng", AccountLogin.AccountLogged.TenTaiKhoan));
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
        }

        private void btnXoaGioHang_Click(object sender, RoutedEventArgs e)
        {
            this.DataContext = gioHangViewModel = null;
            gioHangViewModel = new GioHangViewModel();
        }
    }
}
