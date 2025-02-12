using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
     public class HangHoaDTO
    {
        public string? MaHang { get; set; }
        public string? TenHangHoa { get; set; }
        public double? GiaNhap { get; set; }
        public int? SoLuong { get; set; }
        public string? MoTa { get; set; }
        public string? HinhAnh { get; set; }

        public HangHoaDTO(string? hangHoaID, string? tenHangHoa, double? giaNhap, int? soLuong, string? moTa, string? hinhAnh = null)
        {
            MaHang = hangHoaID;
            TenHangHoa = tenHangHoa;
            GiaNhap = giaNhap;
            SoLuong = soLuong;
            MoTa = moTa;
            HinhAnh = hinhAnh;
        }

        public HangHoaDTO() { }
    }
}
