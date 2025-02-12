using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ChiTietPhieuNhapDTO
    {
        public string? MaCTPN { get; set; }
        public string? MaHang { get; set; }
        public string? TenHang { get; set; }
        public decimal? GiaNhap { get; set; }
        public int SoLuongNhap { get; set; }
        public string? MaPhieuNhap { get; set; }
        public decimal ThanhTien { get; set; }
    }
}
