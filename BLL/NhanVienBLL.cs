using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Mail;
using DTO;
using DAL;


namespace BLL
{
    public class NhanVienBLL
    {
        private NhanVienDAL nhanVienDAL = new();

        public List<NhanVienDTO> HienThiDanhSachNV()
        {
            return nhanVienDAL.HienThiDanhSachNV();
        }

        public bool SuaNhanVien(NhanVienDTO nv)
        {
            if (nv == null || string.IsNullOrWhiteSpace(nv.MaNhanVien))
                return false; // Không thể update nếu không có mã nhân viên

            return nhanVienDAL.SuaNhanVien(nv);
        }

        public bool XoaNhanVien(string maNhanVien)
        {
            if (string.IsNullOrWhiteSpace(maNhanVien))
                return false; // Không xóa nếu mã nhân viên rỗng

            return nhanVienDAL.XoaNhanVien(maNhanVien);
        }
    }
}