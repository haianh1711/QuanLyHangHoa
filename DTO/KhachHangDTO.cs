using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class KhachHangDTO
    {
        public string? MaKhachHang { get; set; }
        public string? TenKhachHang { get; set; }
        public string? SoDienThoai { get; set; }
        public string? Gmail { get; set; }  
        public string? DiaChi { get; set; }

        
        public KhachHangDTO(string maKhachHang, string tenKhachHang, string soDienThoai, string diaChi, string gmail)
        {
            MaKhachHang = maKhachHang;
            TenKhachHang = tenKhachHang;
            SoDienThoai = soDienThoai;
            Gmail = gmail;
            DiaChi = diaChi;
        }

        public KhachHangDTO() { }
    }
}
