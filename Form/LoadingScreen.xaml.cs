using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace QuanLyTruyenTranh.Form
{
    /// <summary>
    /// Interaction logic for LoadingScreen.xaml
    /// </summary>
    public partial class LoadingScreen : Window
    {
        private DispatcherTimer time = new DispatcherTimer();
        public LoadingScreen()
        {
            Thread.Sleep(1000);
            InitializeComponent();
            time.Interval = TimeSpan.FromMilliseconds(10);
            time.Tick += SplashWindow_Loaded;
            time.Start();
        }

        private void SplashWindow_Loaded(object sender, EventArgs e)
        {
            this.ShowInTaskbar = false;
            progress.Value += 1;
            if (progress.Value == progress.Maximum)
            {
                time.Stop();
                frmDangNhap frmDangNhap = new frmDangNhap();
                this.Close();
                frmDangNhap.ShowDialog();
            }
        }
    }
}
