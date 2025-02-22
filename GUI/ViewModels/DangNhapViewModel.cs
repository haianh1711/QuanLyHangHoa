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
        [ObservableProperty]
        private DangNhapBLL dangNhapBLL = new DangNhapBLL();

        [RelayCommand]
        public async Task DangNhapGmail()
        {
            TaiKhoanDTO GmailHopLe = await dangNhapBLL.DangNhapGmail();
            if (GmailHopLe != null)
            {
                await thongBaoVM.MessageOK("Đăng nhập thành công!");
                Application.Current.Windows
                    .OfType<Window>()
                    .FirstOrDefault(w => w.IsActive)?.Hide();

                MainForm mainForm = new MainForm();
                Application.Current.MainWindow = mainForm;
                mainForm.Show();
            }
            else
            {
                MessageBox.Show("Đăng nhập thất bại");
                return;
            }
        }
    }
}
