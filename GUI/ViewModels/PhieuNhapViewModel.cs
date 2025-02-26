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

namespace GUI.ViewModels
{
    internal partial class PhieuNhapViewModel : ObservableObject
    {
        [ObservableProperty]
        private ThongBaoViewModel thongBaoVM = new ThongBaoViewModel();

        private PhieuNhapBLL phieuNhapBLL = new();

        // dataGrid
        [ObservableProperty]
        private ObservableCollection<PhieuNhapDTO> phieuNhaps = [];

        // Thuộc tính của phieu nhạp
        [ObservableProperty]
        private PhieuNhapDTO? selectedPhieuNhap;

        // Tìm kiếm
        [ObservableProperty]
        private string? tuKhoaTimKiem;

        [ObservableProperty]
        private MainViewModel mainVM;

        public PhieuNhapViewModel(MainViewModel mainViewModel)
        {
            this.mainVM = mainViewModel;

            LoadDanhSachPhieuNhap();
            SelectedPhieuNhap = new();
        }


        private void LoadDanhSachPhieuNhap()
        {
            PhieuNhaps.Clear();
            PhieuNhaps = new ObservableCollection<PhieuNhapDTO>(phieuNhapBLL.HienThiDanhSachPN());
        }

        [RelayCommand]
        private async Task ThemPhieuNhap()
        {
            try
            {
                PhieuNhapDTO phieuNhapDTO = new()
                {
                    MaPhieuNhap = phieuNhapBLL.TaoMaPNMoi(),
                    MaNhanVien = MainVM.NhanVien.MaNhanVien,
                    NgayNhap = DateTime.Now.ToString(),
                    TongTien = '0',
                };

                bool result = phieuNhapBLL.ThemPhieuNhap(phieuNhapDTO);
                if (result)
                {
                    await ThongBaoVM.MessageOK("Thêm phiếu nhập thành công");
                    LoadDanhSachPhieuNhap();
                }
            }
            catch (Exception ex)
            {
                await ThongBaoVM.MessageOK(ex.ToString());
            }
        }

        [RelayCommand]
        private async Task XemChiTiet()
        {
            if (SelectedPhieuNhap != null && !string.IsNullOrEmpty(SelectedPhieuNhap.MaPhieuNhap))
            {
                MainVM.View = new ChiTietPhieuNhapViewModel(SelectedPhieuNhap, MainVM, this);
            }
            else
            {
                await ThongBaoVM.MessageOK("Vui lòng chọn phiếu nhập!");
            }
        }

        [RelayCommand]
        private async Task XoaPhieuNhap()
        {
            try
            {
                if (SelectedPhieuNhap != null && SelectedPhieuNhap.MaPhieuNhap != null)
                {
                    bool isXoaPhieuNhap = await ThongBaoVM.MessageYesNo("Bạn có chắc chắn muốn xóa phiếu nhập này? Dữ liệu sẽ bị mất vĩnh viễn.");
                    if (isXoaPhieuNhap)
                    {
                        bool result = phieuNhapBLL.XoaPhieuNhap(SelectedPhieuNhap.MaPhieuNhap);
                        if (result)
                        {
                            await ThongBaoVM.MessageOK("Xóa phiếu nhập thành công");
                            LoadDanhSachPhieuNhap();
                        }
                    }
                    else
                    {
                        await ThongBaoVM.MessageOK("Vui lòng chọn phiếu nhập muốn xóa");
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
            if (string.IsNullOrWhiteSpace(TuKhoaTimKiem))
            {
                await ThongBaoVM.MessageOK("Vui lòng nhập mã phiếu nhập để tìm kiếm.");
                return;
            }

            if (phieuNhapBLL != null)
            {
                SelectedPhieuNhap = PhieuNhaps.FirstOrDefault(pn => pn.MaPhieuNhap == TuKhoaTimKiem.ToUpper());
                if (SelectedPhieuNhap == null)
                {
                    await ThongBaoVM.MessageOK("Không tìm thấy phiếu nhập có mã " + TuKhoaTimKiem.ToUpper());
                }

            }
        }
    }
}
