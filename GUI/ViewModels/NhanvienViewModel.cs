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
using Microsoft.Win32;
using System.Windows.Input;

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

        // Thuộc tính của phiếu nhập
        [ObservableProperty]
        private NhanVienDTO? selectedNhanVien;

        // Tìm kiếm
        [ObservableProperty]
        private string? tuKhoaTimKiem;
        [ObservableProperty]
        private ObservableCollection<NhanVienDTO> nhanVien = new();
        [ObservableProperty]
        private ObservableCollection<string> danhSachChucVu = new();

        // Tìm kiếm nhân viên theo mã
        [ObservableProperty]
        private string? maNhanVienTimKiem;

        public NhanvienViewModel()
        {
            LoadDanhSachNhanVien();
            SelectedNhanVien = new();
        }

        private void LoadDanhSachNhanVien()
        {
            NhanVienDTOs.Clear();
            NhanVienDTOs = new ObservableCollection<NhanVienDTO>(nhanVienBLL.HienThiDanhSachNV());

            danhSachChucVu = new ObservableCollection<string>(
                NhanVienDTOs.Select(nv => nv.ChucVu).Distinct().ToList()
            );
        }

        [RelayCommand]
        private async Task SuaNhanVien()
        {
            try
            {
                if (SelectedNhanVien != null)
                {
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
        private void SearchNhanVien()
        {
            if (!string.IsNullOrWhiteSpace(TuKhoaTimKiem))
            {
                NhanVienDTOs = new ObservableCollection<NhanVienDTO>(nhanVienBLL.TimKiemNhanVien(TuKhoaTimKiem));
            }
            else
            {
                LoadDanhSachNhanVien();
            }
        }

        [RelayCommand]
        private async Task ChonHinhAnh()
        {
            var openFileDialog = new OpenFileDialog { Filter = "Image Files|*.jpg;*.jpeg;*.png" };
            if (openFileDialog.ShowDialog() == true)
            {
                SelectedNhanVien.HinhAnh = nhanVienBLL.SaveImage(openFileDialog.FileName);
            }
        }

        [RelayCommand]
        private void TimKiemNhanVien()
        {
            if (!string.IsNullOrEmpty(maNhanVienTimKiem))
            {
                SelectedNhanVien = nhanVienBLL.GetNhanVienByMa(maNhanVienTimKiem);
            }
        }

    }
}
