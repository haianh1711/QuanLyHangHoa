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

        [ObservableProperty]
        private NhanVienDTO nhanVien;

        [ObservableProperty]
        private TaiKhoanDTO taiKhoan;
        public TrangChuViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;

            NhanVien = _mainViewModel.NhanVien;
            TaiKhoan = _mainViewModel.TaiKhoan;

            _mainViewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(MainViewModel.NhanVien))
                {
                    NhanVien = _mainViewModel.NhanVien;
                    OnPropertyChanged(nameof(HinhAnh)); // Cập nhật ảnh
                }
            };

        }



        public BitmapImage HinhAnh
        {
            get
            {
                try
                {
                    // Kiểm tra nếu NhanVien có ảnh không
                    if (!string.IsNullOrEmpty(NhanVien?.HinhAnh))
                    {
                        // Ghép đường dẫn đầy đủ của ảnh
                        string fullPath = Path.Combine("C:\\Images\\NhanVien\\AnhVN", NhanVien.HinhAnh);

                        // Kiểm tra xem file ảnh có tồn tại không
                        if (File.Exists(fullPath))
                        {
                            return new BitmapImage(new Uri(fullPath, UriKind.Absolute));
                        }
                    }
                }
                catch
                {
                    // Nếu có lỗi, bỏ qua để không làm crash app
                }

                // Nếu không có ảnh thì dùng ảnh mặc định
                return new BitmapImage(new Uri("pack://application:,,,/Images/ImageDefault.png", UriKind.Absolute));
            }
        }




        [RelayCommand]
        private void ThayDoiHinhAnh()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Chọn ảnh nhân viên",
                Filter = "Ảnh (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;
                string fileName = Path.GetFileName(selectedFilePath);
                string imagesFolder = "C:\\Images\\NhanVien\\AnhVN";

                if (!Directory.Exists(imagesFolder))
                {
                    Directory.CreateDirectory(imagesFolder);
                }

                string destinationFilePath = Path.Combine(imagesFolder, fileName);
                File.Copy(selectedFilePath, destinationFilePath, true);

                NhanVien.HinhAnh = fileName;
                OnPropertyChanged(nameof(HinhAnh)); 
            }
        }

    }
}
