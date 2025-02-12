using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class HangHoaBLL
    {
        private HangHoaDAL HangHoaDAL = new();

        public List<HangHoaDTO> LayMaVaTenSP()
        {
            return HangHoaDAL.LayMaVaTenSP();
        }
        public bool ThemHangHoa(HangHoaDTO HangHoa)
        {
            return HangHoaDAL.ThemHangHoa(HangHoa);
        }
        public List<HangHoaDTO> TimHangHoa(string tukhoa)
        {
            return HangHoaDAL.TimHangHoa(tukhoa);
        }
        public bool CapnhatHangHoa(HangHoaDTO HangHoa)
        {
            if (string.IsNullOrEmpty(HangHoa.MaHang))
                return false;
            return HangHoaDAL.CapNhatHangHoa(HangHoa);
        }
        public bool XoaHangHoa(string MaHang)
        {
            if (string.IsNullOrEmpty(MaHang))
                return false;
            return HangHoaDAL.XoaHangHoa(MaHang);
        }
        public int LaySoLuongHangHoa(string MaHang)
        {
            return HangHoaDAL.LaySoLuongHangHoa(MaHang);
        }
        public bool CapNhatSoLuongHangHoa(string MaHang, int soLuong)
        {
            return HangHoaDAL.CapNhatSoLuongHangHoa(MaHang, soLuong);
        }
    }
}
