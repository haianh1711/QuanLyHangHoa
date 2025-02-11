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

        public bool CapNhatSanPham(string toanTu, int soLuongThayDoi, string maSanPham)
        {
            int soLuongBanDau = sanPhamDAL.LaySoLuongSanPham(maSanPham);

            if (toanTu == "+")
            {
                return sanPhamDAL.CapNhatSoLuongSanPham(maSanPham, soLuongBanDau + soLuongThayDoi);
            }
            else if (toanTu == "-")
            {
                return sanPhamDAL.CapNhatSoLuongSanPham(maSanPham, soLuongBanDau - soLuongThayDoi);
            }

            return false;

        }



    }
}
