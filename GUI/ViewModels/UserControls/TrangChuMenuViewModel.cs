using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GUI.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GUI.ViewModels.UserControls
{
    partial class TrangChuMenuViewModel(MainViewModel mainViewModel) : ObservableObject
    {
        [ObservableProperty]
        private MainViewModel mainViewModel = mainViewModel;

        [ObservableProperty]
        private Visibility backButtonVisibility = Visibility.Collapsed;



        [RelayCommand]
        private void GoBack()
        {
            BackButtonVisibility = Visibility.Collapsed; // Ẩn nút khi quay lại
            MainViewModel.View = new TrangChuViewModel(MainViewModel);
            // Quay lại trang chính
        }

        [RelayCommand]
        private void QlNhanVien()
        {
            MainViewModel.View = new NhanvienViewModel();
        }

        [RelayCommand]
        private void QlKhachHang()
        {
            MainViewModel.View = new KhachHangViewModel();
        }


        [RelayCommand]
        private void QlHangHoa()
        {
            MainViewModel.View = new HangHoaViewModel();
        }

        [RelayCommand]
        private void ChonGiaoDien()
        {
            MainViewModel.View = new ChonGiaoDichViewModel(MainViewModel);
        }

        [RelayCommand]
        private void QlThongKeNhap()
        {
            MainViewModel.View = new ThongKeSPNhapViewModel();
        }

        [RelayCommand]
        private void QlThongKeXuat()
        {
            MainViewModel.View = new ThongKeSPXuatViewModel();
        }

        [RelayCommand]
        private async Task TatNguon()
        {
            try
            {
                bool tatNguon = await MainViewModel.ThongBaoVM.MessageYesNo("Bạn có muốn thoát không?");

                if (tatNguon)
                {
                    App.Current.Shutdown();
                }
            }
            catch (Exception ex)
            {
                await MainViewModel.ThongBaoVM.MessageOK(ex.Message);

            }
        }

        private bool _isThongKeSelected;
        public bool IsThongKeSelected
        {
            get => _isThongKeSelected;
            set
            {
                _isThongKeSelected = value;
                OnPropertyChanged(nameof(IsThongKeSelected));
            }
        }

    }
}
