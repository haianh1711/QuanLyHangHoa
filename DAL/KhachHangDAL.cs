using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DTO;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public class KhachHangDAL
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();
        // Lấy tất cả khách hàng từ cơ sở dữ liệu
        public List<KhachHangDTO> HienThiDanhSachKH()
        {
            List<KhachHangDTO> danhSachKhachHang = new List<KhachHangDTO>();
            string query = "SELECT MaKhachHang, TenKhachHang, SoDienThoai,DiaChi, Gmail FROM KhachHang";

            DataTable dt = dbHelper.ExecuteQuery(query);
            foreach (DataRow row in dt.Rows)
            {
                KhachHangDTO kh = new KhachHangDTO
                {
                    MaKhachHang = row["MaKhachHang"].ToString() ??"",
                    TenKhachHang = row["TenKhachHang"].ToString() ?? "",
                    SoDienThoai = row["SoDienThoai"].ToString() ?? "",
                    Gmail = row["Gmail"].ToString() ?? "",
                    DiaChi = row["DiaChi"].ToString() ?? ""
                };
                danhSachKhachHang.Add(kh);
            }
            return danhSachKhachHang;
        }

        // Cập nhật thông tin khách hàng
        public bool SuaKhachHang(KhachHangDTO khachHang)
        {
            try
            {
                // Kiểm tra dữ liệu đầu vào, nếu sai thì return false ngay lập tức
                if (khachHang == null) return false;

                if (string.IsNullOrWhiteSpace(khachHang.TenKhachHang))
                {
                    Console.WriteLine("Tên khách hàng không được để trống!");
                    return false;
                }

                if (string.IsNullOrWhiteSpace(khachHang.SoDienThoai))
                {
                    Console.WriteLine("Số điện thoại không được để trống!");
                    return false;
                }

                // Loại bỏ khoảng trắng
                khachHang.SoDienThoai = khachHang.SoDienThoai.Trim();

                // Kiểm tra số điện thoại hợp lệ
                string pattern = @"^\d{10,11}$";
                if (!Regex.IsMatch(khachHang.SoDienThoai, pattern))
                {
                    Console.WriteLine("Số điện thoại không hợp lệ! Chỉ chứa 10-11 chữ số.");
                    return false;
                }

                // Nếu tất cả dữ liệu hợp lệ thì thực hiện cập nhật
                string query = "UPDATE KhachHang SET TenKhachHang = @TenKhachHang, SoDienThoai = @SoDienThoai, Gmail = @Gmail, DiaChi = @DiaChi WHERE MaKhachHang = @MaKhachHang";
                SqlParameter[] parameters =
                {
            new SqlParameter("@MaKhachHang", khachHang.MaKhachHang),
            new SqlParameter("@TenKhachHang", khachHang.TenKhachHang),
            new SqlParameter("@SoDienThoai", khachHang.SoDienThoai),
            new SqlParameter("@Gmail", khachHang.Gmail),
            new SqlParameter("@DiaChi", khachHang.DiaChi)
        };

                bool result = dbHelper.ExecuteNonQuery(query, parameters) > 0;

                if (result)
                {
                    // Chỉ load lại danh sách nếu cập nhật thành công
                    HienThiDanhSachKH();
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi DAL: " + ex.Message);
                return false;
            }
        }



        // Xóa khách hàng
        public bool XoaKhachHang(string maKhachHang)
        {
            string query = "DELETE FROM KhachHang WHERE MaKhachHang = @MaKhachHang";
            SqlParameter[] parameters =
            {
                new SqlParameter("@MaKhachHang", maKhachHang)
            };

            return dbHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        // Tìm kiếm khách hàng theo mã và tên
        public List<KhachHangDTO> TimKiem(string tuKhoa)
        {
            List<KhachHangDTO> danhSachKhachHang = new List<KhachHangDTO>();
            string query = "SELECT MaKhachHang, TenKhachHang, SoDienThoai, Gmail, DiaChi FROM KhachHang WHERE MaKhachHang LIKE @TuKhoa OR TenKhachHang LIKE @TuKhoa";

            SqlParameter[] parameters =
            {
        new SqlParameter("@TuKhoa", $"%{tuKhoa}%")
    };

            try
            {
                DataTable dt = dbHelper.ExecuteQuery(query, parameters);
                foreach (DataRow row in dt.Rows)
                {
                    KhachHangDTO kh = new KhachHangDTO
                    {
                        MaKhachHang = row["MaKhachHang"].ToString(),
                        TenKhachHang = row["TenKhachHang"].ToString(),
                        SoDienThoai = row["SoDienThoai"].ToString(),
                        Gmail = row["Gmail"].ToString(),
                        DiaChi = row["DiaChi"].ToString()
                    };
                    danhSachKhachHang.Add(kh);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi tìm kiếm khách hàng: " + ex.Message);
            }

            return danhSachKhachHang;
        }

        public List<string> LayDanhSachMaKhachHang()
        {
            List<string> danhSachMaKH = new List<string>();
            string query = "SELECT MaKhachHang FROM KhachHang";

            DataTable dt = dbHelper.ExecuteQuery(query);
            foreach (DataRow row in dt.Rows)
            {
                string? maKH = row["MaKhachHang"]?.ToString();
                if (!string.IsNullOrEmpty(maKH)) 
                {
                    danhSachMaKH.Add(maKH);
                }
            }

            return danhSachMaKH;
        }
    }
}
