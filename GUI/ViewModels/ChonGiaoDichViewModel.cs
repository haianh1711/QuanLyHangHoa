using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.ViewModels
{
     partial class ChonGiaoDichViewModel(MainViewModel mainViewModel) : ObservableObject
    {
        [ObservableProperty]
        private MainViewModel mainVM = mainViewModel;

        [RelayCommand]
        public void GiaoDichNhap()
        {
            MainVM.View = new PhieuNhapViewModel();
        }

        [RelayCommand]
        public void GiaoDichXuat()
        {
            MainVM.View = new PhieuXuatViewModel();
        }


    }
}
