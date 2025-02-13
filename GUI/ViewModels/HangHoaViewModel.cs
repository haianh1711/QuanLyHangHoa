using BLL;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DAL;
using DTO;
using GUI.ViewModels.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.ViewModels
{
    partial class HangHoaViewModel : ObservableObject
    {
        [ObservableProperty]
        private ThongBaoViewModel thongBaoVM = new ThongBaoViewModel();

        private HangHoaBLL hangHoaBLL = new();

        // dataGrid
        [ObservableProperty]
        private ObservableCollection<HangHoaDTO> hangHoaDTOs= [];

        // Thuộc tính của phieu nhạp
        [ObservableProperty]
        private HangHoaDTO? selectedHangHoa;

        // Tìm kiếm
        [ObservableProperty]
        private string? tuKhoaTimKiem;

        public HangHoaViewModel()
        {
            LoadDanhSachHangHoa();
            SelectedHangHoa = new();
        }

        private void LoadDanhSachHangHoa()
        {
            HangHoaDTOs.Clear();
            HangHoaDTOs = new ObservableCollection<HangHoaDTO>(hangHoaBLL.HienThiDanhSachHH());
        }

        [RelayCommand]
        private async Task ThemHangHoa()
        {
            try
            {
                if (SelectedHangHoa != null)
                {

                    bool result = hangHoaBLL.ThemHangHoa(SelectedHangHoa);
                    if (result)
                    {
                        await ThongBaoVM.MessageOK("Thêm hàng hóa thành công");
                        LoadDanhSachHangHoa();
                    }

                }
            }
            catch (Exception ex)
            {
                await ThongBaoVM.MessageOK(ex.ToString());
            }
        }

        [RelayCommand]
        private async Task SuaHangHoa()
        {
            try
            {
                if (SelectedHangHoa != null)
                {
                    bool result = hangHoaBLL.CapnhatHangHoa(SelectedHangHoa);
                    if (result)
                    {
                        await ThongBaoVM.MessageOK("Sửa hàng hóa thành công");
                        LoadDanhSachHangHoa();
                    }

                }
            }
            catch (Exception ex)
            {
                await ThongBaoVM.MessageOK(ex.ToString());
            }
        }

        [RelayCommand]
        private async Task XoaHangHoa()
        {
            try
            {
                if (SelectedHangHoa != null)
                {
                    bool isXoaPhieuNhap = await ThongBaoVM.MessageYesNo("Bạn có chắc chắn muốn xóa phiếu nhập này? Dữ liệu sẽ bị mất vĩnh viễn.");
                    if (isXoaPhieuNhap)
                    {
                        bool result = hangHoaBLL.XoaHangHoa(SelectedHangHoa.MaHang);
                        if (result)
                        {
                            await ThongBaoVM.MessageOK("Xóa phiếu nhập thành công");
                            LoadDanhSachHangHoa();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                await ThongBaoVM.MessageOK(ex.ToString());
            }

        }

        [RelayCommand]
        private async Task TimKiem()
        {
            if(SelectedHangHoa != null)
            {
                TuKhoaTimKiem = TuKhoaTimKiem ?? "";
                HangHoaDTOs = new ObservableCollection<HangHoaDTO>(hangHoaBLL.TimHangHoa(TuKhoaTimKiem));
            }

        }

    }
}
