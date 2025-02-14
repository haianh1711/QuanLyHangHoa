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
    partial class ChiTietPhieuXuatViewModel : ObservableObject
    {
        [ObservableProperty]
        private MainViewModel mainVM;

        [ObservableProperty]
        private ThongBaoViewModel thongBaoVM = new ThongBaoViewModel();

        private PhieuXuatBLL phieuXuatBLL = new();
        private ChiTietPhieuXuatBLL chiTietPhieuXuatBLL = new();
        private HangHoaBLL sanPhamBLL = new();

        // dataGrid
        [ObservableProperty]
        private ObservableCollection<ChiTietPhieuXuatDTO> chiTietPhieuXuats = [];

        private List<ChiTietPhieuXuatDTO> danhSachThem = new();
        private List<ChiTietPhieuXuatDTO> danhSachSua = new();
        private List<ChiTietPhieuXuatDTO> danhSachXoa = new();

        // Tìm kiếm
        [ObservableProperty]
        private string? thongTinTimKiem;

        // Thuộc tính của phieu nhạp
        [ObservableProperty]
        private PhieuXuatDTO? phieuXuat;

        // Thuộc tính của chi tiết phiếu nhập
        [ObservableProperty]
        private ChiTietPhieuXuatDTO? selectedChiTiet = new();

        [ObservableProperty]
        private ChiTietPhieuXuatDTO? tempChiTiet = new();


        // ComboBox
        [ObservableProperty]
        private List<HangHoaDTO> sanPhams;

        [ObservableProperty]
        private HangHoaDTO? selectedSanPham;

        PhieuXuatViewModel formTruoc;

        public ChiTietPhieuXuatViewModel(PhieuXuatDTO phieuXuatDTO, MainViewModel mainViewModel, PhieuXuatViewModel formTruoc)
        {
            this.mainVM = mainViewModel;
            this.formTruoc = formTruoc;
            sanPhams = sanPhamBLL.LayMaVaTenSP();

            this.phieuXuat = phieuXuatDTO;

            chiTietPhieuXuats = new ObservableCollection<ChiTietPhieuXuatDTO>(chiTietPhieuXuatBLL.HienThiDanhSachCTPX(this.phieuXuat.MaPhieuXuat));
        }

        [RelayCommand]
        private void TroVe()
        {
            MainVM.View = formTruoc;
        }


        partial void OnSelectedChiTietChanged(ChiTietPhieuXuatDTO? value)
        {
            if (value != null)
            {
                TempChiTiet = new ChiTietPhieuXuatDTO
                {
                    MaPhieuXuat = value.MaPhieuXuat,
                    MaHang = value.MaHang,
                    TenHang = value.TenHang,
                    GiaXuat = value.GiaXuat,
                    SoLuongXuat = value.SoLuongXuat
                };
            }
            else
            {
                TempChiTiet = new ChiTietPhieuXuatDTO();
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
            if (PhieuXuat != null)
            {
                ThongTinTimKiem = ThongTinTimKiem ?? "";
                PhieuXuat.MaPhieuXuat = PhieuXuat.MaPhieuXuat ?? "";
                ChiTietPhieuXuats = new ObservableCollection<ChiTietPhieuXuatDTO>(chiTietPhieuXuatBLL.TimKiemCTPX(ThongTinTimKiem, PhieuXuat.MaPhieuXuat));
            }
        }

        [RelayCommand]
        private void ThemChiTiet()
        {
            if (ChiTietPhieuXuats != null && TempChiTiet != null && PhieuXuat != null)
            {
                var chiTietMoi = new ChiTietPhieuXuatDTO
                {
                    MaCTPX = chiTietPhieuXuatBLL.TaoMaCTPXMoi(),
                    MaPhieuXuat = PhieuXuat.MaPhieuXuat,
                    MaHang = TempChiTiet.MaHang,
                    TenHang = TempChiTiet.TenHang,
                    GiaXuat = TempChiTiet.GiaXuat,
                    SoLuongXuat = TempChiTiet.SoLuongXuat,
                    ThanhTien = chiTietPhieuXuatBLL.TinhThanhTien(TempChiTiet)
                };

                ChiTietPhieuXuats.Add(chiTietMoi);
                danhSachThem.Add(chiTietMoi);
                PhieuXuat.TongTien = phieuXuatBLL.TinhTongTien(ChiTietPhieuXuats.ToList());
                OnPropertyChanged(nameof(PhieuXuat));
            }
        }

        [RelayCommand]
        private async void SuaChiTiet()
        {
            if (ChiTietPhieuXuats != null && SelectedChiTiet != null && TempChiTiet != null && PhieuXuat != null)
            {

                int index = ChiTietPhieuXuats.IndexOf(SelectedChiTiet);
                if (index >= 0)
                {

                    ChiTietPhieuXuatDTO chiTiet = new ChiTietPhieuXuatDTO
                    {
                        MaCTPX = SelectedChiTiet.MaCTPX,
                        MaPhieuXuat = SelectedChiTiet.MaPhieuXuat,
                        MaHang = TempChiTiet.MaHang,
                        TenHang = TempChiTiet.TenHang,
                        GiaXuat = TempChiTiet.GiaXuat,
                        SoLuongXuat = TempChiTiet.SoLuongXuat,
                        ThanhTien = chiTietPhieuXuatBLL.TinhThanhTien(TempChiTiet),
                    };

                    ChiTietPhieuXuats[index] = chiTiet; // Cập nhật danh sách
                    danhSachSua.Add(chiTiet);
                    // Cập nhật tổng tiền
                    PhieuXuat.TongTien = phieuXuatBLL.TinhTongTien(ChiTietPhieuXuats.ToList());
                    OnPropertyChanged(nameof(PhieuXuat));
                }
            }
        }


        [RelayCommand]
        private void XoaChiTiet()
        {
            if (ChiTietPhieuXuats != null && SelectedChiTiet != null && PhieuXuat != null)
            {
                ChiTietPhieuXuatDTO chiTietCanXoa = ChiTietPhieuXuats.First(chiTiet => chiTiet.MaHang == SelectedChiTiet.MaHang);

                ChiTietPhieuXuats.Remove(chiTietCanXoa);
                danhSachXoa.Add(chiTietCanXoa);

                PhieuXuat.TongTien = phieuXuatBLL.TinhTongTien(ChiTietPhieuXuats.ToList());
                OnPropertyChanged(nameof(PhieuXuat));
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
                if (result && PhieuXuat != null)
                {
                    bool isSuccess = chiTietPhieuXuatBLL.ThemDanhSachCTPX(danhSachThem)
                                    && chiTietPhieuXuatBLL.SuaDanhSachCTPX(danhSachSua)
                                    && chiTietPhieuXuatBLL.XoaDanhSachCTPX(danhSachXoa);

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
