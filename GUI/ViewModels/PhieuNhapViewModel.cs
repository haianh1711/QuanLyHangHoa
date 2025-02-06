using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using DTO;
using System.Collections.ObjectModel;
using BLL;
using System.Windows;
using System.Transactions;


namespace GUI.ViewModels
{
    internal partial class PhieuNhapViewModel : ObservableObject
    {
        private PhieuNhapBLL phieuNhapBLL = new();
        private ChiTietPhieuNhapBLL chiTietPhieuNhapBLL = new();
        private SanPhamBLL sanPhamBLL= new();
        
        // dataGrid
        [ObservableProperty]
        private ObservableCollection<ChiTietPhieuNhapDTO> chiTietPhieuNhaps = [];

        // Thuộc tính của phieu nhạp
        [ObservableProperty]
        private PhieuNhapDTO? phieuNhap;

        // Thuộc tính của chi tiết phiếu nhập
        [ObservableProperty]
        private ChiTietPhieuNhapDTO? selectedChiTiet = new();


        // ComboBox
        [ObservableProperty]
        private List<SanPhamDTO> sanPhams;

        [ObservableProperty]
        private SanPhamDTO? selectedSanPham;


        public PhieuNhapViewModel()
        {
            sanPhams = sanPhamBLL.GetMaVaTenSP();

            phieuNhap = new()
            {
                MaPhieuNhap = phieuNhapBLL.TaoMaPNMoi(),
                NgayNhap = DateTime.Today.ToString("dd-MM-yyyy"),
                MaNhanVien = "NV001", // để tạm
                TongTien = 0
            };
        }

        partial void OnSelectedSanPhamChanged(SanPhamDTO? value)
        {
            if(value != null && SelectedChiTiet != null)
            {
                SelectedChiTiet.MaSanPham = value.MaSanPham;

                SelectedChiTiet.TenSanPham = value.TenSanPham;
                // Lỗi không biết: không thể tự động cập nhập tên sp thay đổi
                // giải pháp code thủ công để thông báo cho giao diện
                OnPropertyChanged(nameof(SelectedChiTiet));
            }
        }

        [RelayCommand]
        private void ThemChiTiet()
        {
            if(ChiTietPhieuNhaps!= null && SelectedChiTiet != null && PhieuNhap != null)
            {
                SelectedChiTiet.GiaNhap = Convert.ToDecimal(SelectedChiTiet.GiaNhap);
                SelectedChiTiet.SoLuongNhap = Convert.ToInt32(SelectedChiTiet.SoLuongNhap);

                var chiTietMoi = new ChiTietPhieuNhapDTO
                {
                    MaCTPN = chiTietPhieuNhapBLL.TaoMaCTPNMoi(),
                    MaPhieuNhap = PhieuNhap.MaPhieuNhap,
                    MaSanPham = SelectedChiTiet.MaSanPham,
                    TenSanPham = SelectedChiTiet.TenSanPham,
                    GiaNhap = SelectedChiTiet.GiaNhap,
                    SoLuongNhap = SelectedChiTiet.SoLuongNhap,
                    ThanhTien = SelectedChiTiet.GiaNhap * SelectedChiTiet.SoLuongNhap
                };

                ChiTietPhieuNhaps.Add(chiTietMoi);


                PhieuNhap.TongTien = phieuNhapBLL.TinhTongTien(ChiTietPhieuNhaps.ToList());
                OnPropertyChanged(nameof(PhieuNhap));
            }
        }

        [RelayCommand]
        private void XoaPhieu()
        {
            if (ChiTietPhieuNhaps != null && SelectedChiTiet != null && PhieuNhap != null)
            {
                ChiTietPhieuNhaps.Clear();
                SelectedChiTiet = new();
                PhieuNhap = new();
            }
        }

        [RelayCommand]
        private void LuuPhieu()
        {
            try
            {
                if (phieuNhapBLL.LuuPhieu(PhieuNhap, ChiTietPhieuNhaps.ToList()))
                {
                    MessageBox.Show("Lưu thành công");
                }

            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }
    }
}
