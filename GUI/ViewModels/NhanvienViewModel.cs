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
using DAL;

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
        [ObservableProperty]
        private ObservableCollection<string> danhSachChucVu = [];
        private void LoadDanhSachNhanVien()
        {
            NhanVienDTOs.Clear();
            NhanVienDTOs = new ObservableCollection<NhanVienDTO>(nhanVienBLL.HienThiDanhSachNV());
            var danhSachNhanVien = nhanVienBLL.HienThiDanhSachNV();
            NhanVienDTOs.Clear();
            foreach (var nv in danhSachNhanVien)
            {
                NhanVienDTOs.Add(nv);
            }

            danhSachChucVu = new ObservableCollection<string>(
                danhSachNhanVien.Select(nv => nv.ChucVu).Distinct().ToList());
        }


        [RelayCommand]
        private async Task SuaNhanVien()
        {
            try
            {
                if (SelectedNhanVien != null)
                {
                    // Chỉ truyền dữ liệu có trong bảng NhanVien
                    var nhanVienCapNhat = new NhanVienDTO
                    {
                        MaNhanVien = SelectedNhanVien.MaNhanVien,
                        TenNhanVien = SelectedNhanVien.TenNhanVien,
                        NgayBatDau = SelectedNhanVien.NgayBatDau,
                        ChucVu = SelectedNhanVien.ChucVu,
                        HinhAnh = SelectedNhanVien.HinhAnh
                    };

                    bool result = nhanVienBLL.SuaNhanVien(nhanVienCapNhat);
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
        [RelayCommand]
        private async Task TimKiem()
        {
            if (string.IsNullOrWhiteSpace(tuKhoaTimKiem))
            {
                await ThongBaoVM.MessageOK("Vui lòng nhập từ khóa tìm kiếm.");
                return;
            }

            string TuKhoa = tuKhoaTimKiem.Trim().ToUpper();

            var danhSach = nhanVienBLL
                .TimKiemNhanVien(TuKhoa)
                .Where(hh => (hh.MaNhanVien != null && hh.MaNhanVien.Contains(TuKhoa, StringComparison.OrdinalIgnoreCase))
                          || (hh.TenNhanVien != null && hh.TenNhanVien.Contains(TuKhoa, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            if (!danhSach.Any())
            {
                await ThongBaoVM.MessageOK($"Không tìm thấy {TuKhoa}");
                return;
            }

            nhanVienDTOs.Clear();
            foreach (var item in danhSach)
            {
                nhanVienDTOs.Add(item);
            }
        }

    }
}



