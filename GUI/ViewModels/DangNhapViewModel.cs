using BLL;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DTO;
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
        private DangNhapBLL dangNhapBLL = new DangNhapBLL();

        [RelayCommand]
        public async Task DangNhapGmail()
        {
            TaiKhoanDTO GmailHopLe = await dangNhapBLL.DangNhapGmail();
            if (GmailHopLe != null)
            {
                MessageBox.Show("Đăng nhập thành công!");
                MainForm mainForm = new MainForm();
                mainForm.Show();

                Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive)?.Hide();
            }
            else
            {
                MessageBox.Show("Đăng nhập thất bại");
                return;
            }
        }
    }
}
