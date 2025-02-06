using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class KhachHangBLL
    {
        private KhachHangDAL khachHangDAL = new KhachHangDAL();

        public List<KhachHangDTO> GetAllKhachHang()
        {
            return khachHangDAL.GetAllKhachHang();
        }


        public bool UpdateKhachHang(KhachHangDTO khachHang)
        {
            if (string.IsNullOrEmpty(khachHang.MaKhachHang))
                return false;

            return khachHangDAL.SuaKhachHang(khachHang);
        }

        public bool DeleteKhachHang(string maKhachHang)
        {
            if (string.IsNullOrEmpty(maKhachHang))
                return false;

            return khachHangDAL.XoaKhachHang(maKhachHang);
        }
        public List<KhachHangDTO> SearchKhachHang(string maKhachHang, string tenKhachHang)
        {
            return khachHangDAL.SearchKhachHang(maKhachHang, tenKhachHang);
        }
        public KhachHangBLL()
        {
            khachHangDAL = new KhachHangDAL();
        }

        public KhachHangDTO? GetKhachHangById(string maKhachHang)
        {
            return khachHangDAL.GetKhachHangById(maKhachHang);
        }
    }
}
