using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using DTO;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Input;
using BLL;
using DAL;
using GUI.Views.UserControls;
using GUI.ViewModels.UserControls;


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


        // Tìm kiếm
        [ObservableProperty]
        private string? tuKhoaTimKiem;


        public KhachHangViewModel() 
        {
           
            Data = new ObservableCollection<KhachHangDTO>(khachHangBLL.HienThiDanhSachKH());
        }


        [RelayCommand]
        public async Task SuaKhachHang()
        {
            // Kiểm tra xem có khách hàng nào được chọn không
            if (SelectedKhachHang == null)
            {
                await ThongBaoVM.MessageOK("Vui lòng chọn một khách hàng trước khi sửa.");
                return; // Dừng phương thức nếu không có khách hàng nào được chọn
            }

            if (SelectedKhachHang != null)
            {
                bool daSua = khachHangBLL.SuaKhachHang(SelectedKhachHang);

                if (daSua)
                {
                    await ThongBaoVM.MessageOK("Sửa khách hàng thành công");
                    Data = new ObservableCollection<KhachHangDTO>(khachHangBLL.HienThiDanhSachKH());

                }
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
                            Data = new ObservableCollection<KhachHangDTO>(khachHangBLL.HienThiDanhSachKH());
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

                if (isOK)  // Khi nhấn OK, tải lại toàn bộ danh sách khách hàng
                {
                    Data = new ObservableCollection<KhachHangDTO>(khachHangBLL.HienThiDanhSachKH());
                }
            }




        }

    }
    }

