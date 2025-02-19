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
        public void SuaKhachHang()
        {
            if(SelectedKhachHang != null)
            {
                bool daSua = khachHangBLL.SuaKhachHang(SelectedKhachHang);

                if (daSua)
                {
                    MessageBox.Show("Đã sửa thành công!");
                    Data = new ObservableCollection<KhachHangDTO>(khachHangBLL.HienThiDanhSachKH());

                }
            }
        }

        [RelayCommand]
        private async Task XoaKhachHang()
        {
            try
            {
                if (SelectedKhachHang != null)
                {
                    bool isXoaPhieuNhap = await ThongBaoVM.MessageYesNo("Bạn có chắc chắn muốn xóa khách hàng này? Dữ liệu sẽ bị mất vĩnh viễn.");
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
        public void SearchKhachHang()
        {
            
                TuKhoaTimKiem = TuKhoaTimKiem ?? "";
                Data = new ObservableCollection<KhachHangDTO>(khachHangBLL.TimKiem(TuKhoaTimKiem));


        }

    }
    }

