using BLL;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DAL;
using DTO;
using GUI.ViewModels.UserControls;
using GUI.Views.UserControls;
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

        private HangHoaBLL HangHoaBLL = new();

        // dataGrid
        [ObservableProperty]
        private ObservableCollection<HangHoaDTO> hangHoaDTOs = [];

        // Thuộc tính của phieu nhạp
        [ObservableProperty]
        private HangHoaDTO? selectedHangHoa;

        // Thuộc tính của phieu nhạp
        [ObservableProperty]
        private HangHoaDTO? tempHangHoa;


        // Tìm kiếm
        [ObservableProperty]
        private string? tuKhoaTimKiem;

        string hinhAnhDefault = @"pack://application:,,,/Images/ImageDefault.png";
        string thuMucLuuAnh = Path.Combine(Directory.GetCurrentDirectory(), "Images", "HangHoa");

        public HangHoaViewModel()
        {
            LoadDanhSachHangHoa();
            SelectedHangHoa = new();

            tempHangHoa = new();
            tempHangHoa.HinhAnh = hinhAnhDefault; // hình mặc định khi chưa chọn sản phẩm
        }

        partial void OnSelectedHangHoaChanged(HangHoaDTO? value)
        {
            if (value != null)
            {
                TempHangHoa = new HangHoaDTO()
                {
                    MaHang = value.MaHang,
                    GiaNhap = value.GiaNhap,
                    TenHang = value.TenHang,
                    SoLuong = value.SoLuong,
                    MoTa = value.MoTa,
                };

                if (string.IsNullOrEmpty(value.HinhAnh))
                {
                    TempHangHoa.HinhAnh = hinhAnhDefault;
                }
                else
                {
                    TempHangHoa.HinhAnh = value.HinhAnh;
                }
                OnPropertyChanged(nameof(TempHangHoa));
            }
        }


        [RelayCommand]
        private void MaHangThayDoi()
        {
            if (TempHangHoa != null && TempHangHoa.MaHang != null)
            {
                if (TempHangHoa.MaHang.Length == 5)
                {
                    HangHoaDTO? hangHoaTim = HangHoaDTOs.FirstOrDefault(hh => hh.MaHang == TempHangHoa.MaHang);
                    if (hangHoaTim != null)
                    {
                        SelectedHangHoa = hangHoaTim;
                    }
                    else
                    {
                        TempHangHoa = new()
                        {
                            MaHang = TempHangHoa.MaHang,
                            SoLuong = 0,
                            HinhAnh = hinhAnhDefault
                        };
                    }
                }
                else
                {
                    TempHangHoa.SoLuong = 0;
                }
                OnPropertyChanged(nameof(TempHangHoa));
            }
        }

        [RelayCommand]
        private async Task ThayDoiHinhAnh()
        {
            if (TempHangHoa !=null && TempHangHoa.MaHang == null)
            {
                await ThongBaoVM.MessageOK("Vui lòng nhập mã hàng hóa");
                return;
            }

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Chọn ảnh",
                Filter = "Ảnh (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string newFilePath = await HangHoaBLL.LuuHinhAnh(openFileDialog.FileName, thuMucLuuAnh, TempHangHoa.MaHang);

                if (!string.IsNullOrEmpty(newFilePath))
                {
                    TempHangHoa.HinhAnh = newFilePath;
                    OnPropertyChanged(nameof(TempHangHoa));
                }
            }
        }

        private void LoadDanhSachHangHoa()
        {
            HangHoaDTOs.Clear();
            HangHoaDTOs = new ObservableCollection<HangHoaDTO>(HangHoaBLL.HienThiDanhSachHH());
        }

        [RelayCommand]
        private async Task ThemHangHoa()
        {
            try
            {
                if (string.IsNullOrEmpty(TempHangHoa.MaHang) || string.IsNullOrEmpty(TempHangHoa.TenHang))
                {
                    await ThongBaoVM.MessageOK("Vui lòng nhập đầy đủ thông tin hàng hoá");
                    return;
                }


                else
                {
                    bool result = HangHoaDTOs.Any(hh => hh.MaHang.Equals(TempHangHoa.MaHang, StringComparison.OrdinalIgnoreCase));
                    if (result)
                    {
                        await ThongBaoVM.MessageOK($"hàng hóa mã {TempHangHoa.MaHang} đã tồn tại.");
                        return;
                    }
                    bool result1 = HangHoaBLL.ThemHangHoa(TempHangHoa);
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
                    bool result = HangHoaBLL.CapnhatHangHoa(TempHangHoa);
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
                        bool result = HangHoaBLL.XoaHangHoa(TempHangHoa.MaHang);
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
            string maCanTim = TuKhoaTimKiem != null ? TuKhoaTimKiem.ToUpper() : "";

            HangHoaDTOs.Clear();
            HangHoaDTOs = new ObservableCollection<HangHoaDTO>(HangHoaBLL.TimHangHoa(maCanTim));

            if (HangHoaDTOs == null || !HangHoaDTOs.Any())
            {
                await ThongBaoVM.MessageOK($"Không tìm thấy {maCanTim}");
                return;
            }
        }
    }
}
