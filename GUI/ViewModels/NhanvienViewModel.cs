using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using DTO;
using BLL;
using CommunityToolkit.Mvvm.Input;
using GUI.ViewModels.UserControls;
using GUI.Views.UserControls;
using DAL;

    namespace GUI.ViewModels
    {
        internal partial class NhanvienViewModel : ObservableObject
        {
            private NhanVienBLL nhanVienBLL = new();

            [ObservableProperty]
            private ThongBaoViewModel thongBaoVM = new();

            [ObservableProperty]
            private ObservableCollection<NhanVienDTO> data;

            [ObservableProperty]
            private NhanVienDTO? selectedNhanVien;

            [ObservableProperty]
            private string? tuKhoaTimKiem;

        public NhanvienViewModel()
        {
            var danhSach = nhanVienBLL.HienThiDanhSachNV();
            Console.WriteLine($"Số nhân viên lấy được: {danhSach.Count}");

            Data = new ObservableCollection<NhanVienDTO>(danhSach);
        }


        [RelayCommand]
            private async Task SuaNhanVien()
            {
                if (SelectedNhanVien != null)
                {
                    bool daSua = nhanVienBLL.SuaNhanVien(SelectedNhanVien);
                    if (daSua)
                    {
                        await ThongBaoVM.MessageOK("Sửa nhân viên thành công");
                        Data = new ObservableCollection<NhanVienDTO>(nhanVienBLL.HienThiDanhSachNV());
                        OnPropertyChanged(nameof(Data));
                    }
                }
            }

            [RelayCommand]
            private async Task XoaNhanVien()
            {
                try
                {
                    bool isXoa = await ThongBaoVM.MessageYesNo("Bạn có chắc chắn muốn xóa nhân viên này?");
                    if (SelectedNhanVien != null && isXoa)
                    {
                        bool result = nhanVienBLL.XoaNhanVien(SelectedNhanVien.MaNhanVien);
                        if (result)
                        {
                            await ThongBaoVM.MessageOK("Xóa nhân viên thành công");
                            Data = new ObservableCollection<NhanVienDTO>(nhanVienBLL.HienThiDanhSachNV());
                            OnPropertyChanged(nameof(Data));
                        }
                    }
                }
                catch (Exception ex)
                {
                    await ThongBaoVM.MessageOK(ex.ToString());
                }
            }

            [RelayCommand]
            public void SearchNhanVien()
            {
                TuKhoaTimKiem ??= "";
                Data = new ObservableCollection<NhanVienDTO>(nhanVienBLL.TimKiemNhanVien(TuKhoaTimKiem));
                OnPropertyChanged(nameof(Data));
            }
        }
    }
    
        //[RelayCommand]
        //private async Task TimKiem()
        //{
        //    if (SelectedNhanVien != null)
        //    {
        //        TuKhoaTimKiem = TuKhoaTimKiem ?? "";
        //        NhanVienDTOs = new ObservableCollection<NhanVienDTO>(nhanVienBLL.Tim(TuKhoaTimKiem));
        //    }

        //}
   
