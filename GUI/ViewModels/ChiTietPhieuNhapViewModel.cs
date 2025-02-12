using BLL;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DTO;
using Google.Apis.Gmail.v1.Data;
using GUI.ViewModels.UserControls;
using GUI.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.ViewModels
{
    partial class ChiTietPhieuNhapViewModel : ObservableObject
    {
        [ObservableProperty]
        private ThongBaoViewModel thongBaoVM = new ThongBaoViewModel();

        private PhieuNhapBLL phieuNhapBLL = new();
        private ChiTietPhieuNhapBLL chiTietPhieuNhapBLL = new();
        private HangHoaBLL sanPhamBLL = new();

        // dataGrid
        [ObservableProperty]
        private ObservableCollection<ChiTietPhieuNhapDTO> chiTietPhieuNhaps = [];

        private List<ChiTietPhieuNhapDTO> danhSachThem = new();
        private List<ChiTietPhieuNhapDTO> danhSachSua = new();
        private List<ChiTietPhieuNhapDTO> danhSachXoa = new();

        // Tìm kiếm
        [ObservableProperty]
        private string? thongTinTimKiem;

        // Thuộc tính của phieu nhạp
        [ObservableProperty]
        private PhieuNhapDTO? phieuNhap;

        // Thuộc tính của chi tiết phiếu nhập
        [ObservableProperty]
        private ChiTietPhieuNhapDTO? selectedChiTiet = new();

        [ObservableProperty]
        private ChiTietPhieuNhapDTO? tempChiTiet = new();


        // ComboBox
        [ObservableProperty]
        private List<HangHoaDTO> sanPhams;

        [ObservableProperty]
        private HangHoaDTO? selectedSanPham;

        public ChiTietPhieuNhapViewModel()
        {
            sanPhams = sanPhamBLL.LayMaVaTenSP();

            phieuNhap = new()
            {
                MaPhieuNhap = "PN002",
                TongTien = 0
            };

            chiTietPhieuNhaps = new ObservableCollection<ChiTietPhieuNhapDTO>(chiTietPhieuNhapBLL.HienThiDanhSachCTPN(phieuNhap.MaPhieuNhap));
        }


        partial void OnSelectedChiTietChanged(ChiTietPhieuNhapDTO? value)
        {
            if (value != null)
            {
                TempChiTiet = new ChiTietPhieuNhapDTO
                {
                    MaPhieuNhap = value.MaPhieuNhap,
                    MaHang = value.MaHang,
                    TenHang = value.TenHang,
                    GiaNhap = value.GiaNhap,
                    SoLuongNhap = value.SoLuongNhap
                };
            }
            else
            {
                TempChiTiet = new ChiTietPhieuNhapDTO();
            }
        }

        partial void OnSelectedSanPhamChanged(HangHoaDTO? value)
        {
            if (value != null && TempChiTiet != null)
            {
                TempChiTiet.MaHang = value.MaHang;

                TempChiTiet.TenHang = value.TenHang;
                // Lỗi không biết: không thể tự động cập nhập tên sp thay đổi
                // giải pháp code thủ công để thông báo cho giao diện
                OnPropertyChanged(nameof(TempChiTiet));
            }
        }

        [RelayCommand]
        private void TimKiem()
        {
            if (PhieuNhap != null)
            {
                ThongTinTimKiem = ThongTinTimKiem ?? "";
                PhieuNhap.MaPhieuNhap = PhieuNhap.MaPhieuNhap ?? "";
                ChiTietPhieuNhaps = new ObservableCollection<ChiTietPhieuNhapDTO>(chiTietPhieuNhapBLL.TimKiemCTPN(ThongTinTimKiem, PhieuNhap.MaPhieuNhap));
            }
        }

        [RelayCommand]
        private void ThemChiTiet()
        {
            if (ChiTietPhieuNhaps != null && TempChiTiet != null && PhieuNhap != null)
            {
                var chiTietMoi = new ChiTietPhieuNhapDTO
                {
                    MaCTPN = chiTietPhieuNhapBLL.TaoMaCTPNMoi(),
                    MaPhieuNhap = PhieuNhap.MaPhieuNhap,
                    MaHang = TempChiTiet.MaHang,
                    TenHang = TempChiTiet.TenHang,
                    GiaNhap = TempChiTiet.GiaNhap,
                    SoLuongNhap = TempChiTiet.SoLuongNhap,
                    ThanhTien = chiTietPhieuNhapBLL.TinhThanhTien(TempChiTiet)
                };

                ChiTietPhieuNhaps.Add(chiTietMoi);
                danhSachThem.Add(chiTietMoi);
                PhieuNhap.TongTien = phieuNhapBLL.TinhTongTien(ChiTietPhieuNhaps.ToList());
                OnPropertyChanged(nameof(PhieuNhap));
            }
        }

        [RelayCommand]
        private async void SuaChiTiet()
        {
            if (ChiTietPhieuNhaps != null && SelectedChiTiet != null && TempChiTiet != null && PhieuNhap != null)
            {

                int index = ChiTietPhieuNhaps.IndexOf(SelectedChiTiet);
                if (index >= 0)
                {

                    ChiTietPhieuNhapDTO chiTiet = new ChiTietPhieuNhapDTO
                    {
                        MaCTPN = SelectedChiTiet.MaCTPN,
                        MaPhieuNhap = SelectedChiTiet.MaPhieuNhap,
                        MaHang = TempChiTiet.MaHang,
                        TenHang = TempChiTiet.TenHang,
                        GiaNhap = TempChiTiet.GiaNhap,
                        SoLuongNhap = TempChiTiet.SoLuongNhap,
                        ThanhTien = chiTietPhieuNhapBLL.TinhThanhTien(TempChiTiet),
                    };

                    ChiTietPhieuNhaps[index] = chiTiet; // Cập nhật danh sách
                    danhSachSua.Add(chiTiet);
                    // Cập nhật tổng tiền
                    PhieuNhap.TongTien = phieuNhapBLL.TinhTongTien(ChiTietPhieuNhaps.ToList());
                    OnPropertyChanged(nameof(PhieuNhap));
                }
            }
        }


        [RelayCommand]
        private void XoaChiTiet()
        {
            if (ChiTietPhieuNhaps != null && SelectedChiTiet != null && PhieuNhap != null)
            {
                ChiTietPhieuNhapDTO chiTietCanXoa = ChiTietPhieuNhaps.First(chiTiet => chiTiet.MaHang == SelectedChiTiet.MaHang);

                ChiTietPhieuNhaps.Remove(chiTietCanXoa);
                danhSachXoa.Add(chiTietCanXoa);

                PhieuNhap.TongTien = phieuNhapBLL.TinhTongTien(ChiTietPhieuNhaps.ToList());
                OnPropertyChanged(nameof(PhieuNhap));
            }
        }


        [RelayCommand]
        private async Task LuuPhieu()
        {
            try
            {
                // Hiển thị hộp thoại xác nhận
                bool result = await ThongBaoVM.MessageYesNo("Bạn có muốn lưu phiếu này không?");

                // Nếu người dùng chọn "Có" (true), tiến hành lưu dữ liệu
                if (result && PhieuNhap != null)
                {
                    bool isSuccess = chiTietPhieuNhapBLL.ThemDanhSachCTPN(danhSachThem)
                                    && chiTietPhieuNhapBLL.SuaDanhSachCTPN(danhSachSua)
                                    && chiTietPhieuNhapBLL.XoaDanhSachCTPN(danhSachXoa);

                    if (isSuccess)
                    {
                        await ThongBaoVM.MessageOK("Lưu thành công!");
                    }
                    else
                    {
                        await ThongBaoVM.MessageOK("Lưu thất bại!");
                    }
                }
            }
            catch (Exception ex)
            {
                await ThongBaoVM.MessageOK($"Lỗi: {ex.Message}");
            }
        }
    }

}
