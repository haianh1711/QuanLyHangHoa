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

        public List<KhachHangDTO> HienThiDanhSachKH()
        {
            return khachHangDAL.HienThiDanhSachKH();
        }


        public bool SuaKhachHang(KhachHangDTO khachHang)
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
        public List<KhachHangDTO> TimKiem(string tuKhoa)
        {
            return khachHangDAL.TimKiem(tuKhoa);
        }

        public bool UpdateKhachHang(KhachHangDTO selectedKhachHang)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KhachHangDTO> GetAllKhachHang()
        {
            throw new NotImplementedException();
        }

        public bool SearchKhachHang(KhachHangDTO selectedKhachHang)
        {
            throw new NotImplementedException();
        }

        public bool DeleteKhachHang(Action searchKhachHang)
        {
            throw new NotImplementedException();
        }

        public KhachHangBLL()
        {
            khachHangDAL = new KhachHangDAL();
        }
    }
}
