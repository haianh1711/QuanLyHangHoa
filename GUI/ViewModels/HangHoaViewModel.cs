using BLL;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DAL;
using DTO;
using GUI.ViewModels.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.ViewModels
{
    partial class HangHoaViewModel : ObservableObject
    {
        [ObservableProperty]
        private ThongBaoViewModel thongBaoVM = new ThongBaoViewModel();

        private HangHoaBLL hangHoaBLL = new();

        // dataGrid
        [ObservableProperty]
        private ObservableCollection<HangHoaDTO> hangHoaDTOs = [];

        // Thuộc tính của phieu nhạp
        [ObservableProperty]
        private HangHoaDTO? selectedHangHoa;

        // Tìm kiếm
        [ObservableProperty]
        private string? maTimKiem;

        public HangHoaViewModel()
        {
            LoadDanhSachHangHoa();
            SelectedHangHoa = new();
        }

        private void LoadDanhSachHangHoa()
        {
            HangHoaDTOs.Clear();
            HangHoaDTOs = new ObservableCollection<HangHoaDTO>(hangHoaBLL.HienThiDanhSachHH());
        }
        // xong thêm
        [RelayCommand]
        private async Task ThemHangHoa()
        {
            try
            {
                if (string.IsNullOrEmpty(selectedHangHoa.MaHang))
                {
                    await ThongBaoVM.MessageOK("Vui lòng nhập mã hàng hoá");
                    return;
                }
                else
                {
                    var danhsach = hangHoaBLL.HienThiDanhSachHH();
                    bool result = danhsach.Any(hh => hh.MaHang.Equals(SelectedHangHoa.MaHang, StringComparison.OrdinalIgnoreCase));
                    if (result)
                    {
                        await ThongBaoVM.MessageOK($"Mã hàng {selectedHangHoa.TenHang} đã tồn tại.");
                        return;
                    }
                    bool result1 = hangHoaBLL.ThemHangHoa(SelectedHangHoa);
                    if (result1)
                    {
                        await ThongBaoVM.MessageOK("Thêm hàng hoá thành công");
                        LoadDanhSachHangHoa();
                    }
                    else
                    {
                        await ThongBaoVM.MessageOK("Thêm hàng hoá thất bại");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                await ThongBaoVM.MessageOK(ex.ToString());
            }
        }

        [RelayCommand]
        private async Task SuaHangHoa()
        {
            try
            {
                if (SelectedHangHoa != null)
                {
                    bool result = hangHoaBLL.CapnhatHangHoa(SelectedHangHoa);
                    if (result)
                    {
                        await ThongBaoVM.MessageOK("Sửa hàng hóa thành công");
                        LoadDanhSachHangHoa();
                    }

                }
            }
            catch (Exception ex)
            {
                await ThongBaoVM.MessageOK(ex.ToString());
            }
        }

        [RelayCommand]
        private async Task XoaHangHoa()
        {
            try
            {
                if (SelectedHangHoa != null)
                {
                    bool isXoaPhieuNhap = await ThongBaoVM.MessageYesNo("Bạn có chắc chắn muốn xóa hàng hoá này? Dữ liệu sẽ bị mất vĩnh viễn.");
                    if (isXoaPhieuNhap)
                    {
                        bool result = hangHoaBLL.XoaHangHoa(SelectedHangHoa.MaHang);
                        if (result)
                        {
                            await ThongBaoVM.MessageOK("Xóa hàng hoá thành công");
                            LoadDanhSachHangHoa();
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
            if (string.IsNullOrWhiteSpace(maTimKiem))
            {
                await thongBaoVM.MessageOK("Vui lòng nhập mã hàng hoặc tên để tìm kiếm.");
                return;
            }

            string maCanTim = maTimKiem.ToUpper();

            var danhSach = hangHoaBLL
                .TimHangHoa(maCanTim)
                .Where(hh => hh.MaHang.Equals(maCanTim, StringComparison.OrdinalIgnoreCase)
                             || hh.TenHang.IndexOf(maCanTim, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();

            if (danhSach == null || !danhSach.Any())
            {
                await ThongBaoVM.MessageOK($"Không tìm thấy {maCanTim}");
                return;
            }

            HangHoaDTOs.Clear();
            foreach (var item in danhSach)
            {
                HangHoaDTOs.Add(item);
            }

            if (HangHoaDTOs.Count == 1)
            {
                SelectedHangHoa = HangHoaDTOs.First();
               // LoadDanhSachHangHoa();
            }
        }
    }
}
