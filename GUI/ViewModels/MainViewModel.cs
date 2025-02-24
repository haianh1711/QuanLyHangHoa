using CommunityToolkit.Mvvm.ComponentModel;
using DTO;
using GUI.ViewModels.UserControls;
using GUI.Views;
using GUI.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;


namespace GUI.ViewModels
{
    internal partial class MainViewModel : ObservableObject
    {

        [ObservableProperty]
        public ThongBaoViewModel thongBaoVM;

        [ObservableProperty]
        public TrangChuViewModel menuVM;

        [ObservableProperty]
        private NhanVienDTO nhanVien;

        [ObservableProperty]
        private TaiKhoanDTO taiKhoan;


        public TrangChuMenuViewModel trangChuMenuVM { get; set; }
        public TrangChuViewModel trangChuVM { get; set; }

        public MainViewModel(TaiKhoanDTO taiKhoanDTO, NhanVienDTO nhanVienDTO)
        {
            taiKhoan = taiKhoanDTO;
            nhanVien = nhanVienDTO;
            // khởi tạo menu
            ThongBaoVM = new ThongBaoViewModel();
            trangChuMenuVM = new TrangChuMenuViewModel(this);
            Menu = trangChuMenuVM;

            // khởi tạo trang chủ 
            trangChuVM = new TrangChuViewModel(this);
            View = trangChuVM;


            
        }

       
        [ObservableProperty]
        private object menu;

        [ObservableProperty]
        private object view;

    }
}
