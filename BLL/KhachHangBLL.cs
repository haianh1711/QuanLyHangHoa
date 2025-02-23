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
            try
            {
                // Kiểm tra Mã Khách Hàng không được để trống
                if (string.IsNullOrEmpty(khachHang.MaKhachHang))
                {
                    throw new Exception("Mã khách hàng không được để trống!");
                }

                return khachHangDAL.SuaKhachHang(khachHang);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi BLL: " + ex.Message);
                throw; // Ném lỗi ra ngoài để giao diện xử lý
            }
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


        public KhachHangBLL()
        {
            khachHangDAL = new KhachHangDAL();
        }
        public List<string> LayDanhSachMaKhachHang()
        {
            return khachHangDAL.LayDanhSachMaKhachHang();
        }
    }
}
