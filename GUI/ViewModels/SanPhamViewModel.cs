using DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.ViewModels
{
    partial class SanPhamViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<SanPhamDTO> data;

        [ObservableProperty]
        private SanPhamDTO? spDuocChon;

        [ObservableProperty]
        private string? sanPhamID;

        [ObservableProperty]
        private string? tenSanPham;

        [ObservableProperty]
        private int? soLuong;

        [ObservableProperty]
        private string? moTa;
        public SanPhamViewModel()
        {
            Data = new ObservableCollection<SanPhamDTO>
            {
                new SanPhamDTO("SP001", "Sản phẩm 1", 10000.50, 50, "Mô tả sản phẩm 1"),
                new SanPhamDTO("SP002", "Sản phẩm 2", 20000.75, 30, "Mô tả sản phẩm 2"),
                new SanPhamDTO("SP003", "Sản phẩm 3", 15000.00, 20, "Mô tả sản phẩm 3"),
                new SanPhamDTO("SP004", "Sản phẩm 4", 25000.60, 40, "Mô tả sản phẩm 4"),
                new SanPhamDTO("SP005", "Sản phẩm 5", 25000.60, 42, "Mô tả sản phẩm 5"),
            };

            PropertyChanged += (s, e) =>
            {
                if(e.PropertyName == nameof(SpDuocChon) && SpDuocChon != null)
                {
                    SanPhamID = SpDuocChon.MaSanPham;
                    TenSanPham = SpDuocChon.TenSanPham;
                    SoLuong = SpDuocChon.SoLuong;
                    MoTa = SpDuocChon.MoTa;
                }
            };

        }
    }
}
