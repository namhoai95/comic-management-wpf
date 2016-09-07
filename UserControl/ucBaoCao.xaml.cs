using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
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
using Microsoft.Reporting.WinForms;
using QuanLyTruyenTranh.Helper;
using QuanLyTruyenTranh.Report;
using QuanLyTruyenTranh.Report.DataSet1TableAdapters;

namespace QuanLyTruyenTranh.UserControl
{
    /// <summary>
    /// Interaction logic for ucBaoCao.xaml
    /// </summary>
    public partial class ucBaoCao : System.Windows.Controls.UserControl
    {
        private bool _isReportViewerLoad;
        public ucBaoCao()
        {
            InitializeComponent();

            var listMonth = new ArrayList()
            {
                new { Month = 1, NameMonth = "Tháng 1"},
                new { Month = 2, NameMonth = "Tháng 2"},
                new { Month = 3, NameMonth = "Tháng 3"},
                new { Month = 4, NameMonth = "Tháng 4"},
                new { Month = 5, NameMonth = "Tháng 5"},
                new { Month = 6, NameMonth = "Tháng 6"},
                new { Month = 7, NameMonth = "Tháng 7"},
                new { Month = 8, NameMonth = "Tháng 8"},
                new { Month = 9, NameMonth = "Tháng 9"},
                new { Month = 10, NameMonth = "Tháng 10"},
                new { Month = 11, NameMonth = "Tháng 11"},
                new { Month = 12, NameMonth = "Tháng 12"},         
            };

            cbMonth.ItemsSource = listMonth;
            cbMonth.SelectedValue = DateTime.Now.Month;
            txtYear.Text = DateTime.Now.Year.ToString();

           
        }

        private void btnXem_Click(object sender, RoutedEventArgs e)
        {
            _isReportViewerLoad = false;
            if (!_isReportViewerLoad)
            {
                reportViewer.Reset();
                PageSettings ps = new PageSettings();
                ps.Landscape = true;
                ps.PaperSize = new PaperSize("A4", 827, 1170);
                ps.PaperSize.RawKind = (int)PaperKind.A4;
                reportViewer.SetPageSettings(ps);

                int month = Convert.ToInt32(cbMonth.SelectedValue);
                int year = Convert.ToInt32(txtYear.Text);
                var reportDataSource = new ReportDataSource();
                var dataset = new DataSet1();
                dataset.BeginInit();

                reportDataSource.Name = "DataSet1";
                reportDataSource.Value = dataset.DataTable1;
                reportViewer.LocalReport.DataSources.Add(reportDataSource);
                reportViewer.LocalReport.ReportEmbeddedResource = "QuanLyTruyenTranh.Report.Report1.rdlc";

                var dataAdapter = new DataTable1TableAdapter();
                dataAdapter.ClearBeforeFill = true;
                dataset.EnforceConstraints = false;
                dataAdapter.Fill(dataset.DataTable1, month, year);

                string tenNhanVien = AccountLogin.AccountLogged.TenNhanVien;
                ReportParameter parameter = new ReportParameter("Parameter1", tenNhanVien);
                ReportParameter parameter2 = new ReportParameter("Parameter2", "TP HCM, Ngày " + DateTime.Now.Day + " Tháng " + DateTime.Now.Month + " Năm " + DateTime.Now.Year);
                ReportParameter parameter3 = new ReportParameter("Parameter3", "Tổng Cộng: " + dataset.DataTable1.Rows.Count);
                ReportParameter parameter4 = new ReportParameter("Parameter4", "Doanh Thu Bán Hàng Tháng " + month + "/" + year);
                reportViewer.LocalReport.SetParameters(new ReportParameter[] { parameter, parameter2, parameter3, parameter4});
                reportViewer.RefreshReport();
                _isReportViewerLoad = true;
            }
        }

        private void txtYear_TextChanged(object sender, TextChangedEventArgs e)
        {
            int t = 0;
            if (int.TryParse(txtYear.Text.ToString().Trim(), out t))
            {
                if (t >= 1900 && t <= 9999)
                {
                    btnXem.IsEnabled = true;
                }
                else
                {
                    btnXem.IsEnabled = false;
                }
            }
            else
            {
                btnXem.IsEnabled = false;
            }
        }
    }
}
