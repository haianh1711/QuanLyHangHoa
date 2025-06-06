﻿using DTO;
using GUI.ViewModels;
using System;
using System.Collections.Generic;
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

namespace GUI.Views
{
    /// <summary>
    /// Interaction logic for MainForm.xaml
    /// </summary>
    public partial class MainForm : Window
    {
        private MainViewModel _viewModel;

        
        public MainForm(TaiKhoanDTO taiKhoan, NhanVienDTO nhanVien)
        {
            InitializeComponent();
            this.DataContext = new MainViewModel(taiKhoan,nhanVien);
        }

       
    }


}
