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

        private NhanVienBLL nhanVienBLL = new();

        // dataGrid
        [ObservableProperty]
        private ObservableCollection<NhanVienDTO> nhanVienDTOs = [];

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
            NhanVienDTOs = new ObservableCollection<NhanVienDTO>(nhanVienBLL.HienThiDanhSachNV());
        }


        [RelayCommand]
        private async Task SuaNhanVien()
        {
            try
            {
                if (SelectedNhanVien != null)
                {
                    bool result = nhanVienBLL.SuaNhanVien(SelectedNhanVien);
                    if (result)
                    {
                        await ThongBaoVM.MessageOK("Sửa nhân viên thành công");
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
                    bool isXoaPhieuNhap = await ThongBaoVM.MessageYesNo("Bạn có chắc chắn muốn xóa nhân viên này? Dữ liệu sẽ bị mất vĩnh viễn.");
                    if (isXoaPhieuNhap)
                    {
                        bool result = nhanVienBLL.XoaNhanVien(SelectedNhanVien.MaNhanVien);
                        if (result)
                        {
                            await ThongBaoVM.MessageOK("Xoá nhân viên thành công");
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

        //[RelayCommand]
        //private async Task TimKiem()
        //{
        //    if (SelectedNhanVien != null)
        //    {
        //        TuKhoaTimKiem = TuKhoaTimKiem ?? "";
        //        NhanVienDTOs = new ObservableCollection<NhanVienDTO>(nhanVienBLL.Tim(TuKhoaTimKiem));
        //    }

        //}
    }
}
