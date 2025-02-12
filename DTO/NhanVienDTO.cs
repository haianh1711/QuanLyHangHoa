using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class NhanVienDTO
    {
        public string MaNhanVien { get; set; }
        public string TenNhanVien { get; set; }
        public string NgayBatDau { get; set; }
        public string ChucVu { get; set; }
        public string HinhAnh { get; set; }
        public string Gmail { get; set; }

        // Constructor mặc định (bắt buộc nếu muốn dùng object initializer)
        public NhanVienDTO() { }

        // Constructor có tham số
        public NhanVienDTO(string maNhanVien, string tenNhanVien, string ngayBatDau, string chucVu, string hinhAnh, string gmail)
        {
            MaNhanVien = maNhanVien;
            TenNhanVien = tenNhanVien;
            NgayBatDau = ngayBatDau;
            ChucVu = chucVu;
            HinhAnh = hinhAnh;
            Gmail = gmail;
        }
    }

}