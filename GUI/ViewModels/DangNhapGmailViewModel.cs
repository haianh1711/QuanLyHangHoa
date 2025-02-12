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


namespace GUI.ViewModels
{
   partial class DangNhapGmailViewModel : ObservableObject
    {
        
        private DangNhapBLL dangNhapBLL = new DangNhapBLL();

        [ObservableProperty]
        private ObservableCollection<TaiKhoanDTO> data;

        [ObservableProperty]
        private TaiKhoanDTO selectTaiKhoan;



        public DangNhapGmailViewModel() 
        {
            

        }

        [RelayCommand]
        public async Task loginGmail()
        {
            if (selectTaiKhoan != null)
            {
                TaiKhoanDTO taikhoanHopLe = await dangNhapBLL.DangNhapGmail();

                if (taikhoanHopLe != null)
                {
                    MessageBox.Show("Đăng nhập thành công!");
                }

            }
        }

    }
}
