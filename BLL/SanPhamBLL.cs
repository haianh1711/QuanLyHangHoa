using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class SanPhamBLL
    {
        private SanPhamDAL sanPhamDAL = new();

        public List<SanPhamDTO> LayMaVaTenSP()
        {
            return sanPhamDAL.LayMaVaTenSP();
        }
        public bool ThemSanPham(SanPhamDTO sanPham)
        {
            return sanPhamDAL.ThemSanPham(sanPham);
        }
        public List<SanPhamDTO> TimSanPham(string tukhoa)
        {
            return sanPhamDAL.TimSanPham(tukhoa);
        }
        public bool CapnhatSanPham(SanPhamDTO sanPham)
        {
            if (string.IsNullOrEmpty(sanPham.MaSanPham))
                return false;
            return sanPhamDAL.CapNhatSanPham(sanPham);
        }
        public bool XoaSanPham(string maSanPham)
        {
            if (string.IsNullOrEmpty(maSanPham))
                return false;
            return sanPhamDAL.XoaSanPham(maSanPham);
        }
        public int LaySoLuongSanPham(string maSanPham)
        {
            return sanPhamDAL.LaySoLuongSanPham(maSanPham);
        }
        public bool CapNhatSoLuongSanPham(string maSanPham, int soLuong)
        {
            return sanPhamDAL.CapNhatSoLuongSanPham(maSanPham, soLuong);
        }
    }
}
