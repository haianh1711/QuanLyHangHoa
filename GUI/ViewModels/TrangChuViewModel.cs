using CommunityToolkit.Mvvm.ComponentModel;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GUI.ViewModels
{
    partial class TrangChuViewModel : ObservableObject
    {
        private MainViewModel _mainViewModel;

        [ObservableProperty]
        private NhanVienDTO nhanVien;

        [ObservableProperty]
        private TaiKhoanDTO taiKhoan;
        public TrangChuViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;

            NhanVien = _mainViewModel.NhanVien;
            TaiKhoan = _mainViewModel.TaiKhoan;

        }


    }
}
