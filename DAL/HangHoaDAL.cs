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
        private readonly DatabaseHelper dbHelper = new();

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

            string query = "Select MaHang, TenHang, SoLuong, MoTa, HinhAnh from HangHoa";

            try
            {
                var dataTable = dbHelper.ExecuteQuery(query);

                foreach (DataRow row in dataTable.Rows)
                {
                    HangHoaDTO HangHoa = new HangHoaDTO
                    {
                        MaHang = row["MaHang"].ToString(),
                        TenHang = row["TenHang"].ToString(),
                        SoLuong = Convert.ToInt32(row["SoLuong"]),
                        MoTa = row["MoTa"].ToString(),
                        HinhAnh = row["HinhAnh"].ToString(),
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
            string query = "INSERT INTO HangHoa (MaHang, TenHang, SoLuong, MoTa, HinhAnh) VALUES (@MaHangHoa, @TenHangHoa, @SoLuong, @MoTa, @HinhAnh)";
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MaHangHoa", HangHoa.MaHang),
                    new SqlParameter("@TenHangHoa", HangHoa.TenHang),
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
                Console.WriteLine("Lỗi khi tìm kiếm hàng hóa: " + ex.Message);
            }
            return HangHoaDTOs;
        }

        public bool SuaHangHoa(HangHoaDTO HangHoa)
        {
            string query = @"UPDATE HangHoa 
                            SET MaHang = @MaHang, TenHang = @TenHang, SoLuong = @SoLuong, HinhAnh = @HinhAnh, MoTa = @MoTa 
                            Where MaHang = @MaHang";
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

        public bool XoaHangHoa(string MaHangHoa)
        {
            // xóa phiếu nhập
            string query1 = @"DELETE FROM ChiTietPhieuNhap WHERE MaHang = @maHH";

            SqlParameter[] parameters1 = new SqlParameter[]
            {
                    new SqlParameter("@maHH", MaHangHoa)
            };

            dbHelper.ExecuteNonQuery(query1, parameters1);

            // phiếu xuất
            string query2 = @"DELETE FROM ChiTietPhieuXuat WHERE MaHang = @maHH";

            SqlParameter[] parameters2 = new SqlParameter[]
            {
                    new SqlParameter("@maHH", MaHangHoa)
            };

            dbHelper.ExecuteNonQuery(query2, parameters2);

            // xóa hàng hóa
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
                    throw new Exception("Lỗi khi xóa hàng hóa: " + ex.Message);
                }
        }
        public int LaySoLuongHangHoa(string MaHangHoa)
        {
            string query = $"SELECT SoLuong FROM HangHoa WHERE MaHang = @MaHang";
            SqlParameter[] parameters =
                [
                    new SqlParameter("@MaHang",MaHangHoa),
                ];
            try
            {
                int soLuong = (int)dbHelper.ExecuteScalar(query, parameters);
                return soLuong;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy số lượng sản phẩm: " + ex.Message);
            }
        }

        public bool CapNhatSoLuongHangHoa(string MaHangHoa, int soLuongCapNhap)
        {
            string query = "UPDATE HangHoa SET SoLuong = @SoLuong WHERE MaHang = @MaHang";

            SqlParameter[] parameters =
            [
                    new SqlParameter("@SoLuong", soLuongCapNhap),
                    new SqlParameter("@MaHang", MaHangHoa),
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
