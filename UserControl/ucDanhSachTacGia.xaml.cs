﻿using System;
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
    /// Interaction logic for ucDanhSachTacGia.xaml
    /// </summary>
    public partial class ucDanhSachTacGia : System.Windows.Controls.UserControl
    {   
        public ucDanhSachTacGia()
        {
            InitializeComponent();
            this.DataContext = new TacGiaViewModel(); ;       
        }
   
    }
}