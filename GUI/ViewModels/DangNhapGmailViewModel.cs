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
            if (SelectTaiKhoan != null)
            {
                TaiKhoanDTO? taikhoanHopLe = await dangNhapBLL.DangNhapGmail();

                if (taikhoanHopLe != null)
                {
                    MessageBox.Show("Đăng nhập thành công!");
                    MainForm mainForm = new MainForm();
                    mainForm.Show();

                    Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive)?.Hide();
                }

            }
            else
            {
                await thongBaoVM.MessageOK("Đăng nhập thất bại");
            }
        }

    }
}
