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
using DAL;
using GUI.Views;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using System.Windows;
using GUI.Views.UserControls;
using GUI.ViewModels.UserControls;
using System.Text.RegularExpressions;


namespace GUI.ViewModels
{
    partial class KhachHangViewModel : ObservableObject
    {
        private KhachHangBLL khachHangBLL = new KhachHangBLL();

        [ObservableProperty]
        private ThongBaoViewModel thongBaoVM = new ThongBaoViewModel();

        [ObservableProperty]
        private ObservableCollection<KhachHangDTO> data;

        [ObservableProperty]
        private KhachHangDTO selectedKhachHang;

        [ObservableProperty]
        private bool quyen;

        [ObservableProperty]
        private KhachHangDTO? tempKhachHang;

        // Tìm kiếm
        [ObservableProperty]
        private string? tuKhoaTimKiem;

        partial void OnSelectedKhachHangChanged(KhachHangDTO? value)
        {
            if (value != null)
            {
                TempKhachHang = new KhachHangDTO()
                {
                    MaKhachHang = value.MaKhachHang,
                    TenKhachHang = value.TenKhachHang,
                    SoDienThoai = value.SoDienThoai,
                    DiaChi = value.DiaChi,
                    Gmail = value.Gmail

                };
            }
        }

        public KhachHangViewModel(bool Quyen)
        {
            this.quyen = Quyen;
            Data = new ObservableCollection<KhachHangDTO>(khachHangBLL.HienThiDanhSachKH());
        }

        private void LoadDanhSachKhachHang()
        {
            Data.Clear();
            Data = new ObservableCollection<KhachHangDTO>(khachHangBLL.HienThiDanhSachKH());
        }

        [RelayCommand]
        public async Task SuaKhachHang()
        {
            try
            {
                if (SelectedKhachHang == null || tempKhachHang == null)
                {
                    await ThongBaoVM.MessageOK("Vui lòng chọn một khách hàng trước khi sửa.");
                    return;
                }

                if (string.IsNullOrEmpty(tempKhachHang.MaKhachHang) || string.IsNullOrEmpty(tempKhachHang.TenKhachHang))
                {
                    await ThongBaoVM.MessageOK("Vui lòng nhập đầy đủ thông tin khách hàng");
                    return;
                }

                if (!Regex.IsMatch(tempKhachHang.SoDienThoai ?? "", @"^\d{10}$"))
                {
                    await ThongBaoVM.MessageOK("Số điện thoại không hợp lệ! Vui lòng nhập 10 số.");
                    return;
                }

                bool daSua = khachHangBLL.SuaKhachHang(tempKhachHang);
                if (daSua)
                {
                    await ThongBaoVM.MessageOK("Sửa khách hàng thành công");
                    LoadDanhSachKhachHang();
                }
                else
                {
                    await ThongBaoVM.MessageOK("Sửa khách hàng thất bại.");
                }
            }
            catch (Exception ex)
            {
                await ThongBaoVM.MessageOK(ex.ToString());
            }
        }

        [RelayCommand]
        private async Task XoaKhachHang()
        {
            try
            {

                // Kiểm tra xem có khách hàng nào được chọn không
                if (SelectedKhachHang == null)
                {
                    await ThongBaoVM.MessageOK("Vui lòng chọn một khách hàng trước khi xóa.");
                    return; // Dừng phương thức nếu không có khách hàng nào được chọn
                }

                bool isXoaPhieuNhap = await ThongBaoVM.MessageYesNo("Bạn có chắc chắn muốn xóa khách hàng này? Dữ liệu sẽ bị mất vĩnh viễn.");
                if (SelectedKhachHang != null)
                {
                    if (isXoaPhieuNhap)
                    {
                        bool result = khachHangBLL.DeleteKhachHang(SelectedKhachHang.MaKhachHang);
                        if (result)
                        {
                            await ThongBaoVM.MessageOK("Xóa khách hàng thành công");
                            LoadDanhSachKhachHang();
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
        public async Task SearchKhachHang()
        {

            TuKhoaTimKiem = TuKhoaTimKiem ?? "";
            var ketQua = khachHangBLL.TimKiem(TuKhoaTimKiem);
            Data = new ObservableCollection<KhachHangDTO>(ketQua);

            if (Data.Count == 0)
            {
                bool isOK = await ThongBaoVM.MessageOK("Không tìm thấy khách hàng với từ khóa: " + TuKhoaTimKiem);

                if (isOK)
                {
                    LoadDanhSachKhachHang();
                }
            }




        }

    }



}
