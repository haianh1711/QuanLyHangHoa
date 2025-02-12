using BLL;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DAL;
using DTO;
using GUI.ViewModels.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.Gaming.Input.ForceFeedback;

namespace GUI.ViewModels.UserControls
{
    partial class DangNhapGmailViewModel: ObservableObject
    {
        [ObservableProperty]
        private DangNhapBLL dangNhapBLL = new DangNhapBLL();

        [ObservableProperty]
        private KhachHangDTO selectedKhachHang;
        public DangNhapGmailViewModel()
        {

        }
        [RelayCommand]
        public async Task DangNhapGmail()
        {
            TaiKhoanDTO GmailHopLe = await dangNhapBLL.DangNhapGmail();
            if (GmailHopLe != null) 
            {
                MessageBox.Show("Đăng nhập thành công");
            }
        }
    }
}
