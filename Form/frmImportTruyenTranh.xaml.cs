using System;
using System.Collections.Generic;
using System.Data;
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
using Aspose.Cells;
using Microsoft.Win32;
using QuanLyTruyenTranh.Helper;
using QuanLyTruyenTranh.Helper.Log;
using QuanLyTruyenTranh.Model;
using QuanLyTruyenTranh.ViewModel;
using DataTable = System.Data.DataTable;
using Window = System.Windows.Window;

namespace QuanLyTruyenTranh.Form
{
    /// <summary>
    /// Interaction logic for frmImportTruyenTranh.xaml
    /// </summary>
    public partial class frmImportTruyenTranh : Window
    {
        private Logger log = new Logger("Màn hình import excel");
        private TruyenTranhExcelViewModel truyenTranhViewModel = new TruyenTranhExcelViewModel();
        private List<TRUYENTRANH> list;

        public delegate void ImportTruyenTranhExcel(List<TRUYENTRANH> list);
        public event ImportTruyenTranhExcel ImportExcel;

        public frmImportTruyenTranh()
        {
            InitializeComponent();
        }

        private void btnOpenFileExcel_Click(object sender, RoutedEventArgs e)
        {
            DataTable dataTable = new DataTable();

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "xlsx File|*.xlsx; *.xls;";
            dialog.Multiselect = false;
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                Workbook workbook = new Workbook(dialog.FileName);
                Worksheet worksheet = workbook.Worksheets[0];
                int maxdatacol = worksheet.Cells.MaxDataColumn;
                int maxdatarow = worksheet.Cells.MaxDataRow;
                dataTable = worksheet.Cells.ExportDataTableAsString(0, 0, maxdatarow + 1,
                    maxdatacol + 1, true);
                list = new List<TRUYENTRANH>();
                if (dataTable.Columns.Count == 8)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        TRUYENTRANH truyentranh = new TRUYENTRANH();
                        truyentranh.TenTruyenTranh = row[0].ToString();
                        truyentranh.SKU = row[1].ToString();
                        truyentranh.MaTG = Convert.ToInt32(row[2].ToString());
                        truyentranh.MaNXB = Convert.ToInt32(row[3].ToString());
                        truyentranh.MaTheLoai = Convert.ToInt32(row[4].ToString());
                        truyentranh.SoLuong = Convert.ToInt32(row[5].ToString());
                        truyentranh.Gia = Convert.ToDouble(row[6].ToString());
                        truyentranh.BiXoa = row[7].ToString() == "0" ? false : true;
                        truyenTranhViewModel.AddListTruyenTranh(truyentranh);
                        list.Add(truyentranh);
                    }
                    this.DataContext = truyenTranhViewModel;
                }
                else
                {
                    lbSuccess.Content = "Vui lòng nhập đúng định dạng excel";
                    lbSuccess.Foreground = Brushes.Red;
                }
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (truyenTranhViewModel.TruyenTranh.Count != 0)
            {
                truyenTranhViewModel.AddListTruyenTranh(list);
                ImportExcel(list);
                lbSuccess.Content = "Import thành công";

                this.DataContext = truyenTranhViewModel = null;
                truyenTranhViewModel = new TruyenTranhExcelViewModel();
                log.LogInfo(string.Format("Tài khoản {0} đã import excel", AccountLogin.AccountLogged.TenTaiKhoan));
                this.Close();
            }
            else
            {
                lbSuccess.Content = "Vui lòng chọn file để import";
                lbSuccess.Foreground = Brushes.Red;
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
