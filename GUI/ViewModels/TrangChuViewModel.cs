using BLL;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DTO;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace GUI.ViewModels
{
    partial class TrangChuViewModel : ObservableObject
    {
        private MainViewModel _mainViewModel;

        private NhanVienBLL _nhanVienBLL = new();


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






        [RelayCommand]
        private async void ThayDoiHinhAnh()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Chọn ảnh",
                Filter = "Ảnh (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string thuMucLuuAnh = Path.Combine(Directory.GetCurrentDirectory(), "Images", "NhanVien");

                //Gửi đường dẫn file gốc và thư mục lưu ảnh xuống BLL
                NhanVien.HinhAnh = await new NhanVienBLL().LuuHinhAnh(openFileDialog.FileName, thuMucLuuAnh, NhanVien.MaNhanVien);
                _nhanVienBLL.CapNhatHinhAnh(NhanVien.MaNhanVien, NhanVien.HinhAnh);

                if (!string.IsNullOrEmpty(NhanVien.HinhAnh))
                {
                    // Hiển thị ảnh lên giao diện
                    OnPropertyChanged(nameof(NhanVien));
                }
            }
        }

    }
}
