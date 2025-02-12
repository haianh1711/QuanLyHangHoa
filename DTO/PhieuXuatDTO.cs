using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class PhieuXuatDTO
    {
        public string? MaPhieuXuat { get; set; }
        public string? MaNhanVien { get; set; }
        public string? MaHang { get; set; }
        public string? NgayXuat { get; set; }
        public int? SoLuongXuat { get; set; }
        public double? GiaXuat { get; set; }
        public decimal? TongTien { get; set; }
    }
}
