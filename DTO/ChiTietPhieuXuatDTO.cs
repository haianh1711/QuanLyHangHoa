using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ChiTietPhieuXuatDTO
    {
        public string? MaCTPX { get; set; }
        public string? MaHang { get; set; }
        public string? TenHang { get; set; }
        public decimal? GiaXuat { get; set; }
        public int SoLuongXuat { get; set; }
        public string? MaPhieuXuat { get; set; }
        public decimal ThanhTien { get; set; }
    }
}
