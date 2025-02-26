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
using GUI.ViewModels.UserControls;
using GUI.Views.UserControls;

namespace GUI.ViewModels
{
    internal partial class PhieuXuatViewModel : ObservableObject
    {
        [ObservableProperty]
        private MainViewModel mainVM;

        [ObservableProperty]
        private ThongBaoViewModel thongBaoVM = new ThongBaoViewModel();

        private PhieuXuatBLL phieuXuatBLL = new();
        private KhachHangBLL khachHangBLL = new();

        // dataGrid
        [ObservableProperty]
        private ObservableCollection<PhieuXuatDTO> phieuXuats = [];

        // Thuộc tính của phieu nhạp
        [ObservableProperty]
        private PhieuXuatDTO? selectedPhieuXuat;

        // Tìm kiếm
        [ObservableProperty]
        private string? maTimKiem;

        [ObservableProperty]
        private List<string> maKhachHangs;

        public PhieuXuatViewModel(MainViewModel mainViewModel)
        {
            this.mainVM = mainViewModel;

            MaKhachHangs = khachHangBLL.LayDanhSachMaKhachHang();

            LoadDanhSachPhieuXuat();
            SelectedPhieuXuat = new();
        }

        private void LoadDanhSachPhieuXuat()
        {
            PhieuXuats.Clear();
            PhieuXuats = new ObservableCollection<PhieuXuatDTO>(phieuXuatBLL.HienThiDanhSachPX());
        }


        [RelayCommand]
        private async Task ThemPhieuXuat()
        {
            try
            {
                if (SelectedPhieuXuat != null && !string.IsNullOrEmpty(SelectedPhieuXuat.MaKhachHang))
                {
                    PhieuXuatDTO phieuXuatDTO = new()
                    {
                        MaPhieuXuat = phieuXuatBLL.TaoMaPXMoi(),
                        MaNhanVien = MainVM.NhanVien.MaNhanVien,
                        NgayXuat = DateTime.Now.ToString(),
                        MaKhachHang = SelectedPhieuXuat.MaKhachHang,
                        TongTien = '0',
                    };

                    bool result = phieuXuatBLL.ThemPhieuXuat(phieuXuatDTO);
                    if (result)
                    {
                        await ThongBaoVM.MessageOK("Thêm phiếu xuất thành công");
                        LoadDanhSachPhieuXuat();
                    }

                }
                else
                {
                    await ThongBaoVM.MessageOK("Vui lòng chọn mã khách hàng");

                }
            }
            catch (Exception ex)
            {
                await ThongBaoVM.MessageOK(ex.ToString());
            }
        }

        [RelayCommand]
        private async Task XoaPhieuXuat()
        {
            try
            {
                if (SelectedPhieuXuat != null)
                {
                    bool isXoaPhieuXuat = await ThongBaoVM.MessageYesNo("Bạn có chắc chắn muốn xóa phiếu nhập này? Dữ liệu sẽ bị mất vĩnh viễn.");
                    if (isXoaPhieuXuat)
                    {
                        bool result = phieuXuatBLL.XoaPhieuXuat(SelectedPhieuXuat.MaPhieuXuat);
                        if (result)
                        {
                            await ThongBaoVM.MessageOK("Xóa phiếu nhập thành công");
                            LoadDanhSachPhieuXuat();
                            SelectedPhieuXuat = new();
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
        private void XemChiTiet()
        {
            if(selectedPhieuXuat != null)
            {
                MainVM.View = new ChiTietPhieuXuatViewModel(SelectedPhieuXuat, MainVM, this);
            }
        }

        [RelayCommand]
        private async Task TimKiem()
        {
            if (string.IsNullOrWhiteSpace(MaTimKiem))
            {
                await ThongBaoVM.MessageOK("Vui lòng nhập mã phiếu nhập để tìm kiếm.");
                return;
            }

            if (phieuXuatBLL != null)
            {
                SelectedPhieuXuat = PhieuXuats.FirstOrDefault(pn => pn.MaPhieuXuat == MaTimKiem.ToUpper());
                if (SelectedPhieuXuat == null)
                {
                    await ThongBaoVM.MessageOK("Không tìm thấy phiếu nhập có mã " + MaTimKiem.ToUpper());
                }

            }
        }
    }
}
