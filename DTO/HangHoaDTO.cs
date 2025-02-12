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
        public string? TenHang { get; set; }
        public double? GiaNhap { get; set; }
        public int SoLuong { get; set; }
        public string? MoTa { get; set; }
        public string? HinhAnh { get; set; }

        public HangHoaDTO(string? hangHoaID, string? tenHangHoa,  int soLuong, string? moTa, string? hinhAnh = null)
        {
            MaHang = hangHoaID;
            TenHang = tenHangHoa;
            SoLuong = soLuong;
            MoTa = moTa;
            HinhAnh = hinhAnh;
        }

        public HangHoaDTO() { }
    }
}
