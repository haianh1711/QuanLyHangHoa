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

        public async Task<string> LuuHinhAnh(string filePathGoc, string thuMucLuu, string maHH)
        {
            try
            {
                if (!Directory.Exists(thuMucLuu))
                {
                    Directory.CreateDirectory(thuMucLuu);
                }

                // Tạo tên file mới với timestamp để tránh ghi đè
                string fileExtension = Path.GetExtension(filePathGoc);
                string tenFile = $"{maHH}_{DateTime.Now.Ticks}{fileExtension}";
                string duongDanMoi = Path.Combine(thuMucLuu, tenFile);

                // Sao chép file bằng stream bất đồng bộ
                using (var sourceStream = new FileStream(filePathGoc, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (var destStream = new FileStream(duongDanMoi, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await sourceStream.CopyToAsync(destStream);
                }

                // Cập nhật đường dẫn ảnh vào DAL
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
