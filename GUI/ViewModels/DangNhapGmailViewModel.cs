using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using DTO;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Input;
using BLL;
using DAL;
using GUI.Views.UserControls;
using GUI.ViewModels.UserControls;
using GUI.Views;


namespace GUI.ViewModels
{
   partial class DangNhapGmailViewModel : ObservableObject
    {
        
        private DangNhapBLL dangNhapBLL = new DangNhapBLL();

        [ObservableProperty]
        private ObservableCollection<TaiKhoanDTO> data;

        [ObservableProperty]
        private ThongBaoViewModel thongBaoVM = new ThongBaoViewModel();

        [ObservableProperty]
        private TaiKhoanDTO selectTaiKhoan;



        public DangNhapGmailViewModel() 
        {
            

        }

        [RelayCommand]
        public async Task loginGmail()
        {
            TaiKhoanDTO? taikhoanHopLe = await dangNhapBLL.DangNhapGmail();
            if (taikhoanHopLe == null)
            {
                await thongBaoVM.MessageOK("Đăng nhập thất bại.");
                return;
            }

            NhanVienDTO? nhanVien = await dangNhapBLL.LayNhanVienTheoEmail(taikhoanHopLe.Gmail);
            if (nhanVien == null)
            {
                await thongBaoVM.MessageOK("Không tìm thấy thông tin nhân viên.");
                return;
            }

            MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

            Application.Current.MainWindow.Hide();
            MainForm mainForm = new MainForm();
            mainForm.View = new TrangChuForm(nhanVien); // Gán UserControl vào View
            mainForm.Show();

            Application.Current.MainWindow = mainForm;

        }


    }
}
