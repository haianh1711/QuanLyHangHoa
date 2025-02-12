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
        public List<SanPhamDTO> TimSanPham(string maSanPham)
        {
            return sanPhamDAL.TimSanPham(maSanPham);
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
    }
}
