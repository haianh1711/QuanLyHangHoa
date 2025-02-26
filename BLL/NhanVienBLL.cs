using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Mail;
using DTO;
using DAL;


namespace BLL
{
    public class NhanVienBLL
    {
        private NhanVienDAL nhanVienDAL = new();

        public List<NhanVienDTO> HienThiDanhSachNV()
        {
            return nhanVienDAL.HienThiDanhSachNV();
        }

        public bool SuaNhanVien(NhanVienDTO nv)
        {
            if (nv == null || string.IsNullOrWhiteSpace(nv.MaNhanVien))
                return false; // Không thể update nếu không có mã nhân viên

            return nhanVienDAL.SuaNhanVien(nv);
        }
        public List<NhanVienDTO> TimKiemNhanVien(string tuKhoa)
        {
            return nhanVienDAL.TimKiemNhanVien(tuKhoa);
        }

        public bool XoaNhanVien(string maNhanVien)
        {
            if (string.IsNullOrWhiteSpace(maNhanVien))
                return false; // Không xóa nếu mã nhân viên rỗng

            return nhanVienDAL.XoaNhanVien(maNhanVien);
        }
        public List<NhanVienDTO> TimKiemNhanvien(string tuKhoa)
        {
            if (string.IsNullOrWhiteSpace(tuKhoa))
            {
                return HienThiDanhSachNV(); // Trả về danh sách đầy đủ nếu không nhập từ khóa
            }

            return HienThiDanhSachNV().Where(nv =>
                nv.TenNhanVien.Contains(tuKhoa, StringComparison.OrdinalIgnoreCase) ||
                nv.MaNhanVien.ToString().Contains(tuKhoa) ||
                nv.ChucVu.Contains(tuKhoa, StringComparison.OrdinalIgnoreCase)
            ).ToList();
        }
        //public string SaveImage(string imagePath, string maNhanVien)
        //{
        //    string destinationPath = Path.Combine("Images", Path.GetFileName(imagePath));
        //    File.Copy(imagePath, destinationPath, true);
        //    return destinationPath;
        //}

        //public string LuuHinhAnh(string filePathGoc, string thuMucLuu, string maNV)
        //{
        //    try
        //    {
        //        if (!Directory.Exists(thuMucLuu))
        //        {
        //            Directory.CreateDirectory(thuMucLuu);
        //        }

        //        string tenFile = maNV + ".jpg";
        //        string duongDanMoi = Path.Combine(thuMucLuu, tenFile);

        //        // Nếu file đích đã tồn tại, giải phóng trước khi ghi đè
        //        if (File.Exists(duongDanMoi))
        //        {
        //            // Giải phóng file bằng cách thử xóa hoặc chờ
        //            try
        //            {
        //                File.Delete(duongDanMoi);
        //            }
        //            catch (IOException)
        //            {
        //                // Nếu file bị khóa, đợi một chút rồi thử lại
        //                System.Threading.Thread.Sleep(100); // Đợi 100ms
        //                File.Delete(duongDanMoi);
        //            }
        //        }

        //        // Sao chép file bằng stream để tránh khóa
        //        using (var sourceStream = new FileStream(filePathGoc, FileMode.Open, FileAccess.Read, FileShare.Read))
        //        using (var destStream = new FileStream(duongDanMoi, FileMode.Create, FileAccess.Write, FileShare.None))
        //        {
        //            sourceStream.CopyTo(destStream);
        //        }

        //        // Cập nhật đường dẫn ảnh vào DAL
        //        bool result = nhanVienDAL.CapNhatHinhAnh(maNV, duongDanMoi);
        //        if (result)
        //        {
        //            return duongDanMoi;
        //        }
        //        else
        //        {
        //            // Nếu DAL cập nhật thất bại, xóa file vừa tạo để tránh rác
        //            if (File.Exists(duongDanMoi))
        //            {
        //                File.Delete(duongDanMoi);
        //            }
        //            return string.Empty;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"Lỗi khi lưu ảnh: {ex.Message}");
        //    }
        //}

        public bool CapNhatHinhAnh(string maNhanVien, string hinhAnhPath)
        {
            return nhanVienDAL.CapNhatHinhAnh(maNhanVien, hinhAnhPath);
        }

        public async Task<string> LuuHinhAnh(string filePathGoc, string thuMucLuu, string maNV)
        {
            try
            {
                if (!Directory.Exists(thuMucLuu))
                {
                    Directory.CreateDirectory(thuMucLuu);
                }

                // Tạo tên file mới với timestamp để tránh ghi đè
                string fileExtension = Path.GetExtension(filePathGoc);
                string tenFile = $"{maNV}_{DateTime.Now.Ticks}{fileExtension}";
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





    }
}