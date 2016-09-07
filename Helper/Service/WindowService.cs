using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using QuanLyTruyenTranh.Form;
using QuanLyTruyenTranh.ViewModel;

namespace QuanLyTruyenTranh.Helper.Service
{
    public class WindowService
    {
        private static Window view;
        public static void ShowWindow<T>(object DataContext) where T : Window, new()
        {
            view = new T();
            view.DataContext = DataContext;
            view.ShowDialog();
        }

        public static void ShowWindow<T>() where T : Window, new()
        {
            view = new T();
            view.ShowDialog();
        }

        public static void CloseWindow<T>() where T : Window, new()
        {
            view.Close();
        }

        public static void HideWindow<T>() where T : Window, new()
        {
            view = new T();
            view.Hide();
        }

        public static void ShowFormThemTacGia(object DataContext)
        {
            WindowService.ShowWindow<frmThemTacGia>(DataContext);
        }

        public static void ShowFormCapNhatTacGia(object DataContext)
        {
            WindowService.ShowWindow<frmCapNhatTacGia>(DataContext);
        }

        public static void CloseFormThemTacGia()
        {
            WindowService.CloseWindow<frmThemTacGia>();
        }

        public static void CloseFormCapNhatTacGia()
        {
            WindowService.CloseWindow<frmCapNhatTacGia>();
        }

        public static void ShowFormCapNhatTheLoai(object DataContext)
        {
            WindowService.ShowWindow<frmCapNhatTheLoai>(DataContext);
        }

        public static void ShowFormThemTheLoai(object DataContext)
        {
            WindowService.ShowWindow<frmThemTheLoai>(DataContext);
        }

        public static void CloseFormThemTheLoai()
        {
            WindowService.CloseWindow<frmThemTheLoai>();
        }

        public static void CloseFormCapNhatTheLoai()
        {
            WindowService.CloseWindow<frmCapNhatTheLoai>();
        }
    }
}
