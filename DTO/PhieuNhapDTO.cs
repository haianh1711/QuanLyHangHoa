using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class PhieuNhapDTO
    {
        public string? MaPhieuNhap {  get; set; }
        public string? MaNhanVien { get; set; }
        public string? MaHang {  get; set; }
        public string? NgayNhap { get; set; }
        public int? SoLuongNhap {  get; set; }
        public double? GiaNhap { get; set; }
        public decimal TongTien { get; set; }
    }
}
