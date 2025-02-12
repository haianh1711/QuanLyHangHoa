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


namespace GUI.ViewModels
{
    partial class KhachHangViewModel : ObservableObject
    {
       private KhachHangBLL khachHangBLL = new KhachHangBLL();

        [ObservableProperty]
        private ObservableCollection<KhachHangDTO> data;

        [ObservableProperty]
        private KhachHangDTO selectedKhachHang;


        public ICommand XoaKhachHangCommand { get; set; }
        public KhachHangViewModel() 
        {
           
            Data = new ObservableCollection<KhachHangDTO>(khachHangBLL.GetAllKhachHang());
        }


        [RelayCommand]
        public void SuaKhachHang()
        {
            if(SelectedKhachHang != null)
            {
                bool daSua = khachHangBLL.UpdateKhachHang(SelectedKhachHang);

                if (daSua)
                {
                    MessageBox.Show("Đã sửa thành công!");
                    Data = new ObservableCollection<KhachHangDTO>(khachHangBLL.GetAllKhachHang());

                }
            }
        }

        [RelayCommand]
        public void XoakhachHang()
        {
            if (SelectedKhachHang != null)
            {
                bool daXoa = khachHangBLL.DeleteKhachHang(SearchKhachHang);

                if(daXoa) 
                {
                    MessageBox.Show("Đã xóa thành công!");
                    Data = new ObservableCollection<KhachHangDTO>(khachHangBLL.GetAllKhachHang());
                }
            }
        }

        [RelayCommand]
        public void SearchKhachHang()
        {
            if (SelectedKhachHang != null)
            {
                bool DaSearch = khachHangBLL.SearchKhachHang(SelectedKhachHang);

            }
        }

    }
    }

