using CommunityToolkit.Mvvm.ComponentModel;
using GUI.ViewModels.UserControls;
using GUI.Views;
using GUI.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;


namespace GUI.ViewModels
{
    internal partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        public ThongBaoViewModel thongBaoVM;

        [ObservableProperty]
        public TrangChuViewModel menuVM;

        public TrangChuMenuViewModel trangChuMenuVM { get; set; }
        public TrangChuViewModel trangChuVM { get; set; }

        public MainViewModel()
        {
            // khởi tạo menu
            ThongBaoVM = new ThongBaoViewModel();
            trangChuMenuVM = new TrangChuMenuViewModel(this);
            Menu = trangChuMenuVM;

            // khởi tạo trang chủ 
            trangChuVM = new TrangChuViewModel();
            View = trangChuVM;

        }

        [ObservableProperty]
        public object view;

        [ObservableProperty]
        public object menu;
    }
}
