using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class SanPhamDTO
    {
        public string? MaSanPham { get; set; }
        public string? TenSanPham { get; set; }
        public double? GiaNhap { get; set; }
        public int? SoLuong { get; set; }
        public string? MoTa { get; set; }
        public string? HinhAnh { get; set; }

        public SanPhamDTO(string? sanPhamID, string? tenSanPham, double? giaNhap, int? soLuong, string? moTa, string? hinhAnh = null)
        {
            MaSanPham = sanPhamID;
            TenSanPham = tenSanPham;
            GiaNhap = giaNhap;
            SoLuong = soLuong;
            MoTa = moTa;
            HinhAnh = hinhAnh;
        }

        public SanPhamDTO() { }
    }
}
