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
                if (string.IsNullOrEmpty(selectedNhanVien.MaNhanVien) || string.IsNullOrEmpty(selectedNhanVien.TenNhanVien))
                {
                    await ThongBaoVM.MessageOK("Vui lòng chọn nhân viên cần sửa");
                    return;
                }
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
                else
                {
                    await ThongBaoVM.MessageOK("Vui lòng chọn nhân viên cần sửa");
                }
            }
            catch (Exception ex)
            {
                await thongBaoVM.MessageOK(ex.ToString());
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

                    // Copy file tạm thời để tránh khóa file gốc
                    string tempFilePath = Path.GetTempFileName();
                    File.Copy(originalFilePath, tempFilePath, true);

                    // Gửi file tạm xuống BLL
                    string newFilePath = nhanVienBLL.LuuHinhAnh(tempFilePath, thuMucLuuAnh, TempNhanVien.MaNhanVien);

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

                    // Xóa file tạm
                    if (File.Exists(tempFilePath))
                    {
                        File.Delete(tempFilePath);
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



