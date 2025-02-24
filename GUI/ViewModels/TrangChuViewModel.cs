using CommunityToolkit.Mvvm.ComponentModel;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.ViewModels
{
    partial class TrangChuViewModel(MainViewModel mainViewModel) : ObservableObject
    {
        private MainViewModel _mainViewModel = mainViewModel;


        //[ObservableProperty]
        //private NhanVienDTO? nhanVien;

        //public void LoadNhanVien(NhanVienDTO nhanVienDTO)
        //{
        //    NhanVien = nhanVienDTO;
        //}
    }
}
