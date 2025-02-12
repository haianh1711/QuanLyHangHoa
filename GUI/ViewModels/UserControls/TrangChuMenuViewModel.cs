using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.ViewModels.UserControls
{
    partial class TrangChuMenuViewModel(MainViewModel mainViewModel) : ObservableObject
    {
        [ObservableProperty]
        private MainViewModel mainViewModel = mainViewModel;

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
        private void QlPhieuNhap()
        {
            MainViewModel.View = new PhieuNhapViewModel();
        }

        [RelayCommand]
        private void QlHangHoa()
        {
            MainViewModel.View = new SanPhamViewModel();
        }

        [RelayCommand]
        private void QlSpTonKho()
        {
            MainViewModel.View = new ThongKeSPTonKhoViewModel();
        }

        [RelayCommand]
        private void QlSpNhap()
        {
            MainViewModel.View = new ThongKeSPNhapViewModel();
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
    }
}
