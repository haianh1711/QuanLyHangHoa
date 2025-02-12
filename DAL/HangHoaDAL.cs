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
    public class HangHoaDAL
    {
        DatabaseHelper dbHelper = new DatabaseHelper();

        public List<HangHoaDTO> LayMaVaTenSP()
        {
            List<HangHoaDTO> HangHoaDTOs = new List<HangHoaDTO>();

            string query = "Select MaHang, TenHang from HangHoa";

            try
            {
                var dataTable = dbHelper.ExecuteQuery(query);

                foreach (DataRow row in dataTable.Rows)
                {
                    HangHoaDTO HangHoa = new HangHoaDTO
                    {
                        MaHang = row["MaHang"].ToString(),
                        TenHang = row["TenHang"].ToString(),
                    };
                    HangHoaDTOs.Add(HangHoa);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }

            return HangHoaDTOs;
        }

        public List<HangHoaDTO> HienThiDanhSachHH()
        {
            List<HangHoaDTO> HangHoaDTOs = new List<HangHoaDTO>();

            string query = "Select MaHang, TenHang from HangHoa";

            try
            {
                var dataTable = dbHelper.ExecuteQuery(query);

                foreach (DataRow row in dataTable.Rows)
                {
                    HangHoaDTO HangHoa = new HangHoaDTO
                    {
                        MaHang = row["MaHang"].ToString(),
                        TenHang = row["TenHang"].ToString(),
                    };
                    HangHoaDTOs.Add(HangHoa);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }

            return HangHoaDTOs;
        }

        public bool ThemHangHoa(HangHoaDTO HangHoa)
        {
            string query = "INSERT INTO HangHoa (MaHang, TenHang, SoLuong, MoTa, HinhAnh) VALUES (@MaHang, @TenHang, @SoLuong, @MoTa, @HinhAnh)";
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
            {
            new SqlParameter("@MaHang", HangHoa.MaHang),
            new SqlParameter("@TenHang", HangHoa.TenHang),
           // new SqlParameter("@DonGia", HangHoa.GiaNhap),
            new SqlParameter("@SoLuong", HangHoa.SoLuong),
            new SqlParameter("@MoTa", HangHoa.MoTa ?? (object)DBNull.Value), // Tránh lỗi nếu null
            new SqlParameter("@HinhAnh", HangHoa.HinhAnh ?? (object)DBNull.Value)
            };
                int rowsAffected = dbHelper.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm sản phẩm: " + ex.Message);
            }
        }
        public List<HangHoaDTO> TimHangHoa(string tukhoa)
        {
            List<HangHoaDTO> HangHoaDTOs = new List<HangHoaDTO>();
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
                    HangHoaDTO HangHoa = new HangHoaDTO
                    {
                        MaHang = row["MaHang"].ToString(),
                        TenHang = row["TenHang"].ToString(),
                        SoLuong = Convert.ToInt32(row["SoLuong"]),
                        HinhAnh = row["HinhAnh"].ToString(),
                        MoTa = row["MoTa"].ToString()
                    };
                    HangHoaDTOs.Add(HangHoa);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }
            return HangHoaDTOs;
        }

        public bool CapNhatHangHoa(HangHoaDTO HangHoa)
        {
            string query = $"UPDATE HangHoa SET MaHang = @MaHang, TenHang = @TenHang, SoLuong = @SoLuong, HinhAnh = @HinhAnh, MoTa = @MoTa Where MaHang = @MaHang";
            SqlParameter[] parameters =
            {
                new SqlParameter("@MaHang", HangHoa.MaHang),
                new SqlParameter ("@TenHang", HangHoa.TenHang),
                new SqlParameter("@SoLuong", HangHoa.SoLuong),
                new SqlParameter("@HinhAnh", HangHoa.HinhAnh),
                new SqlParameter("@MoTa", HangHoa.MoTa)
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

        public bool XoaHangHoa(string MaHang)
        {
                string query = $"DELETE FROM HangHoa where Mahang = @MaHang";
                SqlParameter[] parameters =
                {
                new SqlParameter("@MaHang", MaHang)
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
        public int LaySoLuongHangHoa(string MaHang)
        {
            string query = $"GET SoLuong WHERE MaHang = @MaHang";
            SqlParameter[] parameters =
                [
                    new SqlParameter("@MaPhieuNhap",MaHang),
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

        public bool CapNhatSoLuongHangHoa(string MaHang, int soLuongCapNhap)
        {
            string query = "UPDATE HangHoa SET SoLuong = @SoLuong WHERE MaHang = @MaHang";

            SqlParameter[] parameters =
            [
                    new SqlParameter("@SoLuong", soLuongCapNhap),
                    new SqlParameter("@MaHang", MaHang),
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
