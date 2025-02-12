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
            if (string.IsNullOrEmpty(HangHoa.MaHangHoa))
                return false;
            return HangHoaDAL.CapNhatHangHoa(HangHoa);
        }
        public bool XoaHangHoa(string maHangHoa)
        {
            if (string.IsNullOrEmpty(maHangHoa))
                return false;
            return HangHoaDAL.XoaHangHoa(maHangHoa);
        }
        public int LaySoLuongHangHoa(string maHangHoa)
        {
            return HangHoaDAL.LaySoLuongHangHoa(maHangHoa);
        }
        public bool CapNhatSoLuongHangHoa(string maHangHoa, int soLuong)
        {
            return HangHoaDAL.CapNhatSoLuongHangHoa(maHangHoa, soLuong);
        }
    }
}
