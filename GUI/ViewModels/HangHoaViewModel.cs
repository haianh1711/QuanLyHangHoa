using BLL;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DAL;
using DTO;
using GUI.ViewModels.UserControls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
        [ObservableProperty]
        private bool dangsua = true;

        partial void OnSelectedHangHoaChanging(HangHoaDTO? value)
        {

        }

        [ObservableProperty]
        private string? hinhAnhPath;

        public HangHoaViewModel()
        {
            LoadDanhSachHangHoa();
            SelectedHangHoa = new();
        }

        [RelayCommand]
        private void SelectImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Chọn ảnh",
                Filter = "Ảnh (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string thuMucLuuAnh = Path.Combine(Directory.GetCurrentDirectory(), "Images");

                // Gửi đường dẫn file gốc và thư mục lưu ảnh xuống BLL
                //string newFilePath = hangHoaBLL.LuuHinhAnh(openFileDialog.FileName, thuMucLuuAnh);

                //if (!string.IsNullOrEmpty(newFilePath))
                //{
                //    // Cập nhật đường dẫn ảnh vào CSDL
                //    hangHoaBLL.CapNhatDuongDanAnh(SelectedHangHoa.MaHang, newFilePath);

                //    // Hiển thị ảnh lên giao diện
                //    HinhAnhPath = newFilePath;
                //}
            }
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
                if (string.IsNullOrEmpty(selectedHangHoa.MaHang) || string.IsNullOrEmpty(selectedHangHoa.TenHang))
                {
                    await ThongBaoVM.MessageOK("Vui lòng nhập đầy đủ thông tin hàng hoá");
                    return;
                }
                else
                {
                    var danhsach = hangHoaBLL.HienThiDanhSachHH();
                    bool result = danhsach.Any(hh => hh.MaHang.Equals(SelectedHangHoa.MaHang, StringComparison.OrdinalIgnoreCase));
                    if (result)
                    {
                        await ThongBaoVM.MessageOK($"hàng hóa tên {selectedHangHoa.TenHang} đã tồn tại.");
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
        private void ClearSelection()
        {
            SelectedHangHoa = null;
        }


        [RelayCommand]
        private async Task TimKiem()
        {


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
                // SelectedHangHoa = HangHoaDTOs.First();
                // LoadDanhSachHangHoa();
            }
        }
        partial void OnSelectedHangHoaChanged(HangHoaDTO? value)
        {
            dangsua = value == null;
        }
        [RelayCommand]
    }

}
