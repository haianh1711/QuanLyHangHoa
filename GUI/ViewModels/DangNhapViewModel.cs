using BLL;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DTO;
using GUI.ViewModels.UserControls;
using GUI.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GUI.ViewModels
{
    partial class DangNhapViewModel : ObservableObject
    {
        [ObservableProperty]
        private ThongBaoViewModel thongBaoVM = new ThongBaoViewModel();

        private DangNhapBLL dangNhapBLL = new DangNhapBLL();

        [ObservableProperty]
        private TaiKhoanDTO? taiKhoan;

        [ObservableProperty]
        private NhanVienDTO? nhanVien;

        [RelayCommand]
        public async Task DangNhapGmail()
        {
            if ((TaiKhoan = await dangNhapBLL.DangNhapGmail()) != null)
            {
                await thongBaoVM.MessageOK("Đăng nhập thành công!");
                Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive)?.Hide();

                if ((NhanVien = await dangNhapBLL.TimNhanVienTheoEmail(TaiKhoan.Gmail)) != null)
                {
                    
                    var mainWindow = new MainForm(); 
                    var trangChuForm = new TrangChuForm { DataContext = this };

                    // Đặt UserControl vào MainForm
                    mainWindow.Content = trangChuForm;
                    Application.Current.MainWindow = mainWindow;
                    mainWindow.Show();
                }
                else await thongBaoVM.MessageOK("Không tìm thấy thông tin nhân viên.");
            }
            else await thongBaoVM.MessageOK("Đăng nhập thất bại");
        }

    }
}
