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

            string query = "Select MaSanPham, TenSanPham from SanPham";

            try
            {
                var dataTable = dbHelper.ExecuteQuery(query);

                foreach (DataRow row in dataTable.Rows)
                {
                    SanPhamDTO sanPham = new SanPhamDTO
                    {
                        MaSanPham = row["MaSanPham"].ToString(),
                        TenSanPham = row["TenSanPham"].ToString(),
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

            string query = "Select MaSanPham, TenSanPham from SanPham";

            try
            {
                var dataTable = dbHelper.ExecuteQuery(query);

                foreach (DataRow row in dataTable.Rows)
                {
                    SanPhamDTO sanPham = new SanPhamDTO
                    {
                        MaSanPham = row["MaSanPham"].ToString(),
                        TenSanPham = row["TenSanPham"].ToString(),
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
            string query = "INSERT INTO HangHoa (MaHang, TenHang, SoLuong, MoTa, HinhAnh) VALUES (@MaSanPham, @TenSanPham, @SoLuong, @MoTa, @HinhAnh)";
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
            {
            new SqlParameter("@MaSanPham", sanPham.MaSanPham),
            new SqlParameter("@TenSanPham", sanPham.TenSanPham),
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
                        MaSanPham = row["MaHang"].ToString(),
                        TenSanPham = row["TenHang"].ToString(),
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
                new SqlParameter("@MaHang", sanPham.MaSanPham),
                new SqlParameter ("@TenHang", sanPham.TenSanPham),
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

        public bool XoaSanPham(string maSanPham)
        {
                string query = $"DELETE FROM HangHoa where Mahang = @MaHang";
                SqlParameter[] parameters =
                {
                new SqlParameter("@MaHang", maSanPham)
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
        public int LaySoLuongSanPham(string maSanPham)
        {
            string query = $"GET SoLuong WHERE MaSanPham = @MaSanPham";
            SqlParameter[] parameters =
                [
                    new SqlParameter("@MaPhieuNhap",maSanPham),
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

        public bool CapNhatSoLuongSanPham(string maSanPham, int soLuongCapNhap)
        {
            string query = "UPDATE SanPham SET SoLuong = @SoLuong WHERE MaSanPham = @MaSanPham";

            SqlParameter[] parameters =
            [
                    new SqlParameter("@SoLuong", soLuongCapNhap),
                    new SqlParameter("@MaSanPham", maSanPham),
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
