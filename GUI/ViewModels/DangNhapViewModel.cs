using BLL;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DTO;
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
        private DangNhapBLL dangNhapBLL = new DangNhapBLL();

        [RelayCommand]
        public async Task DangNhapGmail()
        {
            TaiKhoanDTO GmailHopLe = await dangNhapBLL.DangNhapGmail();
            if (GmailHopLe != null)
            {
                MessageBox.Show("Đăng nhập thành công");
            }else
            {
                MessageBox.Show("Đăng nhập thất bại");
                return;
            }
        }
    }
}
