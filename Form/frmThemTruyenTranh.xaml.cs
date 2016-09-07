using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Win32;
using QuanLyTruyenTranh.Helper;
using QuanLyTruyenTranh.Helper.Log;
using QuanLyTruyenTranh.Model;
using QuanLyTruyenTranh.ViewModel;
using Path = System.IO.Path;

namespace QuanLyTruyenTranh.Form
{
    /// <summary>
    /// Interaction logic for frmThemTruyenTranh.xaml
    /// </summary>
    public partial class frmThemTruyenTranh : Window
    {
        private string pathImage;
        private string source;
        public int flag = 0;
        private int flagImage = 0;
        private int id;

        public delegate void AddingTruyenTranh(TRUYENTRANH truyentranh);
        public event AddingTruyenTranh AddTruyenTranh;

        public delegate void UpdatingTruyenTranh(TRUYENTRANH truyentranh);
        public event UpdatingTruyenTranh UpdateTruyenTranh;

        public frmThemTruyenTranh()
        {
            InitializeComponent();
            LoadAllCombobox();
        }

        public frmThemTruyenTranh(int id)
        {
            InitializeComponent();
            this.id = id;
            LoadAllCombobox();
            LoadTruyenTranhById(id);
        }

        private void LoadTruyenTranhById(int id)
        {
            var truyenTranh = TruyenTranhViewModel.GetTruyenTranhById(id);

            txtTenTruyenTranh.Text = truyenTranh.TenTruyenTranh;
            txtSKU.Text = truyenTranh.SKU;
            txtGia.Text = truyenTranh.Gia.ToString();
            txtSoLuong.Text = truyenTranh.SoLuong.ToString();
            string des = Directory.GetCurrentDirectory() + "\\" + truyenTranh.AnhBia;
            Uri imageUri = new Uri(des);
            BitmapImage bitmapImage = new BitmapImage(imageUri);
            imageTruyenTranh.Source = bitmapImage;
            pathImage = truyenTranh.AnhBia;
            cbxTacGia.SelectedValue = truyenTranh.MaTG;
            cbxNXB.SelectedValue = truyenTranh.MaNXB;
            cbxTheLoai.SelectedValue = truyenTranh.MaTheLoai;
            txtMoTa.Text = truyenTranh.MoTa;
            cbBiXoa.Visibility = Visibility.Visible;
            cbBiXoa.IsChecked = truyenTranh.BiXoa;

            btnThemTruyenTranh.Content = "Cập nhật";
            this.Title = "Cập Nhật Truyện Tranh";
        }

        private void LoadAllCombobox()
        {
            cbxTacGia.ItemsSource = TacGiaViewModel.GetListTacGia();
            cbxTacGia.SelectedValuePath = "MaTG";
            cbxTacGia.DisplayMemberPath = "HoTen";

            cbxTheLoai.ItemsSource = TheLoaiViewModel.GetListTheLoai();
            cbxTheLoai.SelectedValuePath = "MaTheLoai";
            cbxTheLoai.DisplayMemberPath = "TenTheLoai";

            cbxNXB.ItemsSource = NhaXuatBanViewModel.GetListNhaXuatBan();
            cbxNXB.SelectedValuePath = "MaNXB";
            cbxNXB.DisplayMemberPath = "TenNXB";
        }

        private bool IsValidate()
        {
            bool valid = true;
            Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
            if (String.IsNullOrEmpty(txtTenTruyenTranh.Text))
            {
                valid = false;
                MessageBox.Show("Vui lòng nhập tên truyện tranh");
                txtTenTruyenTranh.Focus();
                return valid;
            }
            else if (String.IsNullOrEmpty(txtSKU.Text))
            {
                valid = false;
                MessageBox.Show("Vui lòng nhập SKU");
                txtSKU.Focus();
                return valid;
            }
            else if (!regex.IsMatch(txtSKU.Text))
            {
                valid = false;
                MessageBox.Show("Vui lòng SKU là số");
                txtSKU.Focus();
                return valid;
            }
            else if (String.IsNullOrEmpty(txtGia.Text))
            {
                valid = false;
                MessageBox.Show("Vui lòng nhập tên truyện tranh");
                txtGia.Focus();
                return valid;
            }
            else if (!regex.IsMatch(txtGia.Text))
            {
                valid = false;
                MessageBox.Show("Vui lòng nhập giá là số");
                txtGia.Focus();
                return valid;
            }
            else if (String.IsNullOrEmpty(txtSoLuong.Text))
            {
                valid = false;
                MessageBox.Show("Vui lòng nhập số lượng");
                txtSoLuong.Focus();
                return valid;
            }
            else if (!regex.IsMatch(txtSoLuong.Text))
            {
                valid = false;
                MessageBox.Show("Vui lòng nhập số lượng là số");
                txtSoLuong.Focus();
                return valid;
            }


            return valid;
        }

        private void btnSelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "jpg|*.jpg|png|*.png|gif|*.gif|All files (*.*)|*.*";

            if (dialog.ShowDialog() == true)
            {
                source = dialog.FileName;
                FileInfo fileInfo = new FileInfo(source);
                Uri imageUri = new Uri(source);
                BitmapImage bitmapImage = new BitmapImage(imageUri);
                imageTruyenTranh.Source = bitmapImage;
                DateTime d = DateTime.Now;
                string time = d.Millisecond.ToString() + d.Minute.ToString() + d.Hour.ToString() + d.Day + d.Month +
                              d.Year;

                string extension = Path.GetExtension(dialog.FileName);
                pathImage = time + extension;
                flagImage = 1;
            }
        }

        private void AddTT()
        {
            TRUYENTRANH truyentranh = new TRUYENTRANH();

            if (IsValidate())
            {
                string tenTruyenTranh = txtTenTruyenTranh.Text;
                string SKU = txtSKU.Text;
                float gia = float.Parse(txtGia.Text);
                int soLuong = Convert.ToInt32(txtSoLuong.Text);
                int maTG = Convert.ToInt32(cbxTacGia.SelectedValue);
                int maNXB = Convert.ToInt32(cbxNXB.SelectedValue);
                int maTheLoai = Convert.ToInt32(cbxTheLoai.SelectedValue);
                string moTa = txtMoTa.Text;
                if (cbxTacGia.SelectedValue != null && cbxNXB.SelectedValue != null && cbxTheLoai.SelectedValue != null)
                {
                    if (String.IsNullOrEmpty(moTa))
                    {
                        moTa = "";
                    }

                    truyentranh.TenTruyenTranh = tenTruyenTranh;
                    truyentranh.SKU = SKU;
                    truyentranh.Gia = gia;
                    truyentranh.SoLuong = soLuong;
                    truyentranh.MoTa = moTa;
                    truyentranh.AnhBia = @"/Images/" + pathImage;
                    truyentranh.NgayNhap = DateTime.Now;
                    truyentranh.MaTG = maTG;
                    truyentranh.MaNXB = maNXB;
                    truyentranh.MaTheLoai = maTheLoai;
                    truyentranh.BiXoa = false;
                    if (source != null)
                    {
                        //string desMain = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName +
                        //                 "\\Images\\" + pathImage;
                        string des = Directory.GetCurrentDirectory() + "\\Images\\" + pathImage;
                        File.Copy(source, des, true);
                        //File.Copy(source, desMain, true);
                    }

                    Logger log = new Logger("Màn hình thêm truyện tranh");
                    AddTruyenTranh(truyentranh);
                    log.LogInfo(string.Format("Tài khoản {0} đã thêm truyện tranh {1}", AccountLogin.AccountLogged.TenTaiKhoan, truyentranh.TenTruyenTranh));
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Vui lòng xem lại đã chọn tác giả hay nhà xuất bản hay thể loại!");
                }
            }
        }

        private void UpdateTT()
        {
            var truyenTranh = TruyenTranhViewModel.GetTruyenTranhById(id);

            if (IsValidate())
            {
                string tenTruyenTranh = txtTenTruyenTranh.Text;
                string SKU = txtSKU.Text;
                float gia = float.Parse(txtGia.Text);
                int soLuong = Convert.ToInt32(txtSoLuong.Text);
                int maTG = Convert.ToInt32(cbxTacGia.SelectedValue);
                int maNXB = Convert.ToInt32(cbxNXB.SelectedValue);
                int maTheLoai = Convert.ToInt32(cbxTheLoai.SelectedValue);
                string moTa = txtMoTa.Text;
                if (cbxTacGia.SelectedValue != null && cbxNXB.SelectedValue != null && cbxTheLoai.SelectedValue != null)
                {
                    if (moTa == null)
                    {
                        moTa = "";
                    }

                    truyenTranh.TenTruyenTranh = tenTruyenTranh;
                    truyenTranh.SKU = SKU;
                    truyenTranh.Gia = gia;
                    truyenTranh.SoLuong = soLuong;
                    truyenTranh.MoTa = moTa;
                    if (flagImage == 1)
                    {
                        truyenTranh.AnhBia = @"/Images/" + pathImage;
                    }
                    truyenTranh.MaTG = maTG;
                    truyenTranh.MaNXB = maNXB;
                    truyenTranh.MaTheLoai = maTheLoai;
                    truyenTranh.BiXoa = cbBiXoa.IsChecked;

                    if (source != null)
                    {
                        if (flag == 1)
                        {
                            try
                            {
                                string des = Directory.GetCurrentDirectory() + "\\Images\\" + pathImage;
                                File.Copy(source, des, true);
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Đã có hình ảnh vui lòng chọn ảnh khác", "Thông báo", MessageBoxButton.OK);
                            }
                        }
                        //string desMain = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName +
                        //                "\\Images\\" + pathImage;
                     
                       
                        //File.Copy(source, desMain, true);
                    }

                    Logger log = new Logger("Màn hình cập nhật truyện tranh");
                    UpdateTruyenTranh(truyenTranh);
                    log.LogInfo(string.Format("Tài khoản {0} đã cập nhật truyện tranh {1}", AccountLogin.AccountLogged.TenTaiKhoan, truyenTranh.MaTruyenTranh));
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Vui lòng xem lại đã chọn tác giả hay nhà xuất bản hay thể loại!");
                }
            }
        }

        private void btnThemTruyenTranh_Click(object sender, RoutedEventArgs e)
        {
            if (flag == 0)
            {
                AddTT();
            }
            else
            {
                UpdateTT();
            }
        }

        private void btnHuy_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
