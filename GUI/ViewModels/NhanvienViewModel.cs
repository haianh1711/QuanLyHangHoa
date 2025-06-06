﻿using System;
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
using Microsoft.Win32;
using System.IO;

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
        [ObservableProperty]
        private NhanVienDTO? tempNhanVien;

        // Tìm kiếm
        [ObservableProperty]
        private string? tuKhoaTimKiem;


        [ObservableProperty]
        private bool quyen;

        private string hinhAnhDefault = @"pack://application:,,,/Images/ImageDefault.png";

        public NhanvienViewModel(MainViewModel mainViewModel)
        {
            LoadDanhSachNhanVien();
            SelectedNhanVien = new();
            TempNhanVien = new();
            TempNhanVien.HinhAnh = hinhAnhDefault; // Hình mặc định khi chưa chọn
            quyen = mainViewModel.TaiKhoan.Quyen.ToLower() == "admin";
        }
        [ObservableProperty]
        private ObservableCollection<string> danhSachChucVu = [];

        partial void OnSelectedNhanVienChanged(NhanVienDTO? value)
        {
            if (value != null)
            {
                TempNhanVien = new NhanVienDTO()
                {
                    MaNhanVien = value.MaNhanVien,
                    TenNhanVien = value.TenNhanVien,
                    NgayBatDau = value.NgayBatDau,
                    ChucVu = value.ChucVu,
                    HinhAnh = string.IsNullOrEmpty(value.HinhAnh) ? hinhAnhDefault : value.HinhAnh
                };
                OnPropertyChanged(nameof(TempNhanVien));
            }
        }
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
                if (SelectedNhanVien == null || TempNhanVien == null)
                {
                    await thongBaoVM.MessageOK("Vui lòng chọn nhân viên cần sửa");
                    return;
                }

                if (string.IsNullOrEmpty(TempNhanVien.MaNhanVien) || string.IsNullOrEmpty(TempNhanVien.TenNhanVien))
                {
                    await thongBaoVM.MessageOK("Vui lòng nhập đầy đủ thông tin nhân viên");
                    return;
                }

                bool result = nhanVienBLL.SuaNhanVien(TempNhanVien);
                if (result)
                {
                    await ThongBaoVM.MessageOK("Sửa nhân viên thành công");
                    LoadDanhSachNhanVien();
                }
                else
                {
                    await ThongBaoVM.MessageOK("Sửa nhân viên thất bại");
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
                if (string.IsNullOrEmpty(selectedNhanVien.MaNhanVien) || string.IsNullOrEmpty(selectedNhanVien.TenNhanVien))
                {
                    await ThongBaoVM.MessageOK("Vui lòng chọn nhân viên cần sửa");
                    return;
                }
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

        [RelayCommand]
        private async Task ThayDoiHinhAnh()
        {
            if (TempNhanVien == null || string.IsNullOrEmpty(TempNhanVien.MaNhanVien))
            {
                await ThongBaoVM.MessageOK("Vui lòng chọn hoặc nhập mã nhân viên");
                return;
            }

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Chọn ảnh",
                Filter = "Ảnh (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string thuMucLuuAnh = Path.Combine(Directory.GetCurrentDirectory(), "Images", "NhanVien");
                    string originalFilePath = openFileDialog.FileName;

                    TempNhanVien.HinhAnh = hinhAnhDefault;
                    OnPropertyChanged(nameof(TempNhanVien));

                    // Gửi file tạm xuống BLL
                    string newFilePath = await nhanVienBLL.LuuHinhAnh(originalFilePath, thuMucLuuAnh, TempNhanVien.MaNhanVien);

                    if (!string.IsNullOrEmpty(newFilePath))
                    {
                        // Giải phóng ảnh cũ nếu cần
                        if (!string.IsNullOrEmpty(TempNhanVien.HinhAnh) && TempNhanVien.HinhAnh != hinhAnhDefault)
                        {
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                        }

                        TempNhanVien.HinhAnh = newFilePath;
                        OnPropertyChanged(nameof(TempNhanVien));
                    }

                }
                catch (IOException ex)
                {
                    await ThongBaoVM.MessageOK($"Lỗi khi thay đổi ảnh: File bị khóa hoặc không thể truy cập. {ex.Message}");
                }
                catch (Exception ex)
                {
                    await ThongBaoVM.MessageOK($"Có lỗi xảy ra: {ex.Message}");
                }
            }
        }
    }
}



