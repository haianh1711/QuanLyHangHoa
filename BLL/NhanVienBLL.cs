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

        public string SaveImage(string imagePath, string maNhanVien)
        {
            try
            {
                // Đường dẫn lưu ảnh
                string imagesFolder = "C:\\Images\\NhanVien\\AnhVN"; // Thư mục chứa ảnh
                if (!Directory.Exists(imagesFolder))
                {
                    Directory.CreateDirectory(imagesFolder);
                }

                // Tạo tên file mới để tránh trùng
                string fileName = $"{maNhanVien}_{Path.GetFileName(imagePath)}";
                string destinationPath = Path.Combine(imagesFolder, fileName);

                // Copy ảnh vào thư mục (ghi đè nếu đã có)
                File.Copy(imagePath, destinationPath, true);

                // Cập nhật đường dẫn vào database
                bool isUpdated = nhanVienDAL.CapNhatHinhAnh(maNhanVien, fileName);

                if (isUpdated)
                {
                    return fileName; // Trả về tên file mới để binding hiển thị
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi lưu ảnh: {ex.Message}");
                return string.Empty;
            }
        }




        public NhanVienDTO GetNhanVienByMa(string maNhanVien)
        {
            return nhanVienDAL.LayNhanVienBangMa(maNhanVien);
        }




    }
}