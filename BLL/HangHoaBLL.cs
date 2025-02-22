using DAL;
using DTO;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BLL
{
    public class HangHoaBLL
    {
        private HangHoaDAL HangHoaDAL = new();
        private ChiTietPhieuNhapBLL ChiTietPhieuNhapBLL = new();

        public string LuuHinhAnh(string filePathGoc, string thuMucLuu, string maSP)
        {
            try
            {
                if (!Directory.Exists(thuMucLuu))
                {
                    Directory.CreateDirectory(thuMucLuu);
                }

                string tenFile = maSP + ".jpg";
                string duongDanMoi = Path.Combine(thuMucLuu, tenFile);

                File.Copy(filePathGoc, duongDanMoi, true);

                return duongDanMoi;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lưu ảnh: {ex.Message}");
            }
        }

        public List<HangHoaDTO> LayMaVaTenSP()
        {
            return HangHoaDAL.LayMaVaTenSP();
        }
        public List<HangHoaDTO> HienThiDanhSachHH()
        {
            return HangHoaDAL.HienThiDanhSachHH();
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
        public bool XoaHangHoa(string? maHangHoa)
        {
            try
            {
                if (string.IsNullOrEmpty(maHangHoa))
                    throw new Exception("Vui lòng nhập chọn hàng hóa muốn xóa");

                using (TransactionScope transaction = new())
                {
                    ChiTietPhieuNhapBLL.XoaTatCaCTPNTHeoMaHH(maHangHoa);

                    if (HangHoaDAL.XoaHangHoa(maHangHoa))
                    {
                        transaction.Complete();
                        return true;
                    }else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Xảy ra lỗi trong quá trình xóa hàng hóa: " + ex.Message, ex);
            }
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
