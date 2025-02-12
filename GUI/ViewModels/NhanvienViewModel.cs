using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using DTO;
using BLL;
using CommunityToolkit.Mvvm.Input;
using GUI.ViewModels.UserControls;
using GUI.Views.UserControls;

namespace GUI.ViewModels
{
    internal partial class NhanvienViewModel : ObservableObject
    {
        [ObservableProperty]
        private ThongBaoViewModel thongBaoVM = new ThongBaoViewModel();

        private NhanVienBLL hangHoaBLL = new();

        // dataGrid
        [ObservableProperty]
        private ObservableCollection<NhanVienDTO> hangHoaDTOs = [];

        // Thuộc tính của phieu nhạp
        [ObservableProperty]
        private NhanVienDTO? selectedNhanVien;

        // Tìm kiếm
        [ObservableProperty]
        private string? tuKhoaTimKiem;

        public NhanvienViewModel()
        {
            LoadDanhSachNhanVien();
            SelectedNhanVien = new();
        }

        private void LoadDanhSachNhanVien()
        {
            NhanVienDTOs.Clear();
            NhanVienDTOs = new ObservableCollection<NhanVienDTO>(BLL.HienThiDanhSachHH());
        }

        [RelayCommand]
        private async Task ThemNhanVien()
        {
            try
            {
                if (SelectedNhanVien != null)
                {

                    bool result = hangHoaBLL.ThemNhanVien(SelectedNhanVien);
                    if (result)
                    {
                        await ThongBaoVM.MessageOK("Thêm hàng hóa thành công");
                        LoadDanhSachNhanVien();
                    }

                }
            }
            catch (Exception ex)
            {
                await ThongBaoVM.MessageOK(ex.ToString());
            }
        }

        [RelayCommand]
        private async Task SuaNhanVien()
        {
            try
            {
                if (SelectedNhanVien != null)
                {
                    bool result = hangHoaBLL.CapnhatNhanVien(SelectedNhanVien);
                    if (result)
                    {
                        await ThongBaoVM.MessageOK("Sửa hàng hóa thành công");
                        LoadDanhSachNhanVien();
                    }

                }
            }
            catch (Exception ex)
            {
                await ThongBaoVM.MessageOK(ex.ToString());
            }
        }

        [RelayCommand]
        private async Task XoaNhanVien()
        {
            try
            {
                if (SelectedNhanVien != null)
                {
                    bool isXoaPhieuNhap = await ThongBaoVM.MessageYesNo("Bạn có chắc chắn muốn xóa phiếu nhập này? Dữ liệu sẽ bị mất vĩnh viễn.");
                    if (isXoaPhieuNhap)
                    {
                        bool result = hangHoaBLL.XoaNhanVien(SelectedNhanVien.MaHang);
                        if (result)
                        {
                            await ThongBaoVM.MessageOK("Xóa phiếu nhập thành công");
                            LoadDanhSachNhanVien();
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
            if (SelectedNhanVien != null)
            {
                TuKhoaTimKiem = TuKhoaTimKiem ?? "";
                NhanVienDTOs = new ObservableCollection<NhanVienDTO>(hangHoaBLL.TimNhanVien(TuKhoaTimKiem));


            }

        }
    }
}
