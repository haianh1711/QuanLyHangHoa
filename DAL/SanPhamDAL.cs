using DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class SanPhamDAL
    {
        DatabaseHelper dbHelper = new DatabaseHelper();

        public List<SanPhamDTO> LayMaVaTenSP()
        {
            List<SanPhamDTO> sanPhamDTOs = new List<SanPhamDTO>();

            string query = "Select MaHangHoa, TenHangHoa from SanPham";

            try
            {
                var dataTable = dbHelper.ExecuteQuery(query);

                foreach (DataRow row in dataTable.Rows)
                {
                    SanPhamDTO sanPham = new SanPhamDTO
                    {
                        MaHangHoa = row["MaHangHoa"].ToString(),
                        TenHangHoa = row["TenHangHoa"].ToString(),
                    };
                    sanPhamDTOs.Add(sanPham);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }

            return sanPhamDTOs;
        }

        public List<SanPhamDTO> HienThiDanhSachHH()
        {
            List<SanPhamDTO> sanPhamDTOs = new List<SanPhamDTO>();

            string query = "Select MaHangHoa, TenHangHoa from SanPham";

            try
            {
                var dataTable = dbHelper.ExecuteQuery(query);

                foreach (DataRow row in dataTable.Rows)
                {
                    SanPhamDTO sanPham = new SanPhamDTO
                    {
                        MaHangHoa = row["MaHangHoa"].ToString(),
                        TenHangHoa = row["TenHangHoa"].ToString(),
                    };
                    sanPhamDTOs.Add(sanPham);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }

            return sanPhamDTOs;
        }

        public bool ThemSanPham(SanPhamDTO sanPham)
        {
            string query = "INSERT INTO HangHoa (MaHang, TenHang, SoLuong, MoTa, HinhAnh) VALUES (@MaHangHoa, @TenHangHoa, @SoLuong, @MoTa, @HinhAnh)";
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
            {
            new SqlParameter("@MaHangHoa", sanPham.MaHangHoa),
            new SqlParameter("@TenHangHoa", sanPham.TenHangHoa),
           // new SqlParameter("@DonGia", sanPham.GiaNhap),
            new SqlParameter("@SoLuong", sanPham.SoLuong),
            new SqlParameter("@MoTa", sanPham.MoTa ?? (object)DBNull.Value), // Tránh lỗi nếu null
            new SqlParameter("@HinhAnh", sanPham.HinhAnh ?? (object)DBNull.Value)
            };
                int rowsAffected = dbHelper.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm sản phẩm: " + ex.Message);
            }
        }
        public List<SanPhamDTO> TimSanPham(string tukhoa)
        {
            List<SanPhamDTO> sanPhamDTOs = new List<SanPhamDTO>();
            string query = "SELECT MaHang, TenHang, SoLuong, HinhAnh, Mota FROM HangHoa WHERE MaHang LIKE @tukhoa or TenHang LIKE @tukhoa";
            SqlParameter[] parameters = 
                {
            new SqlParameter("@tukhoa", $"%{tukhoa}%")
                };
            try
            {
                var dataTable = dbHelper.ExecuteQuery(query,parameters);
                foreach (DataRow row in dataTable.Rows)
                {
                    SanPhamDTO sanPham = new SanPhamDTO
                    {
                        MaHangHoa = row["MaHang"].ToString(),
                        TenHangHoa = row["TenHang"].ToString(),
                        SoLuong = Convert.ToInt32(row["SoLuong"]),
                        HinhAnh = row["HinhAnh"].ToString(),
                        MoTa = row["MoTa"].ToString()
                    };
                    sanPhamDTOs.Add(sanPham);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }
            return sanPhamDTOs;
        }

        public bool CapNhatSanPham(SanPhamDTO sanPham)
        {
            string query = $"UPDATE HangHoa SET MaHang = @MaHang, TenHang = @TenHang, SoLuong = @SoLuong, HinhAnh = @HinhAnh, MoTa = @MoTa Where MaHang = @MaHang";
            SqlParameter[] parameters =
            {
                new SqlParameter("@MaHang", sanPham.MaHangHoa),
                new SqlParameter ("@TenHang", sanPham.TenHangHoa),
                new SqlParameter("@SoLuong", sanPham.SoLuong),
                new SqlParameter("@HinhAnh", sanPham.HinhAnh),
                new SqlParameter("@MoTa", sanPham.MoTa)
            };
            try
            {
                int rowsAffected = dbHelper.ExecuteNonQuery(query,parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật sản phẩm: " + ex.Message);
            }
        }

        public bool XoaSanPham(string MaHangHoa)
        {
                string query = $"DELETE FROM HangHoa where Mahang = @MaHang";
                SqlParameter[] parameters =
                {
                new SqlParameter("@MaHang", MaHangHoa)
            };
                try
                {
                    int rowsAffected = dbHelper.ExecuteNonQuery(query, parameters);
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi khi xóa sản phẩm: " + ex.Message);
                }
        }
        public int LaySoLuongSanPham(string MaHangHoa)
        {
            string query = $"GET SoLuong WHERE MaHangHoa = @MaHangHoa";
            SqlParameter[] parameters =
                [
                    new SqlParameter("@MaPhieuNhap",MaHangHoa),
                ];
            try
            {
                int soLuong = (int)dbHelper.ExecuteScalar(query);
                return soLuong;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy số lượng sản phẩm: " + ex.Message);
            }
        }

        public bool CapNhatSoLuongSanPham(string MaHangHoa, int soLuongCapNhap)
        {
            string query = "UPDATE SanPham SET SoLuong = @SoLuong WHERE MaHangHoa = @MaHangHoa";

            SqlParameter[] parameters =
            [
                    new SqlParameter("@SoLuong", soLuongCapNhap),
                    new SqlParameter("@MaHangHoa", MaHangHoa),
            ];

            try
            {
                int rowsAffected = dbHelper.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy số lượng sản phẩm: " + ex.ToString(), ex);
            }
        }
    }
}
