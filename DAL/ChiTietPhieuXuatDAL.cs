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
    public class ChiTietPhieuXuatDAL
    {
        private readonly DatabaseHelper dbHelper = new ();

        public bool ThemCTPX(ChiTietPhieuXuatDTO chiTietPhieuXuatDTO)
        {
            try
            {
                string query = @"INSERT INTO ChiTietPhieuXuat (MaCTPX, MaHang, GiaXuat, SoLuongXuat, MaPhieuXuat)
                             VALUES (@MaCTPX, @MaHang, @GiaXuat, @SoLuongXuat, @MaPhieuXuat);";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MaCTPX", chiTietPhieuXuatDTO.MaCTPX),
                    new SqlParameter("@MaHang", chiTietPhieuXuatDTO.MaHang),
                    new SqlParameter("@GiaXuat", chiTietPhieuXuatDTO.GiaXuat),
                    new SqlParameter("@SoLuongXuat", chiTietPhieuXuatDTO.SoLuongXuat),
                    new SqlParameter("@MaPhieuXuat", chiTietPhieuXuatDTO.MaPhieuXuat)
                };

                var result = dbHelper.ExecuteNonQuery(query, parameters);

                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Thêm CTPX lỗi: " + ex.Message, ex);
            }
        }

        public bool SuaCTPX(ChiTietPhieuXuatDTO chiTietPhieuXuatDTO)
        {
            try
            {
                string query = @"UPDATE ChiTietPhieuXuat 
                         SET GiaXuat = @GiaXuat, 
                             SoLuongXuat = @SoLuongXuat
                         WHERE MaCTPX = @MaCTPX";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@GiaXuat", chiTietPhieuXuatDTO.GiaXuat),
                    new SqlParameter("@SoLuongXuat", chiTietPhieuXuatDTO.SoLuongXuat),
                    new SqlParameter("@MaCTPX", chiTietPhieuXuatDTO.MaCTPX)
                };

                var result = dbHelper.ExecuteNonQuery(query, parameters);

                return result > 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool XoaCTPX(string maCTPX)
        {
            try
            {
                string query = @"DELETE FROM ChiTietPhieuXuat WHERE MaCTPX = @MaCTPX";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MaCTPX", maCTPX)
                };

                var result = dbHelper.ExecuteNonQuery(query, parameters);

                return result > 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<ChiTietPhieuXuatDTO> HienThiDanhSachCTPX(string MaPhieuXuat)
        {
            try
            {
                List<ChiTietPhieuXuatDTO> list = new();

                string query = @"SELECT ct.MaCTPX, sp.MaHang, sp.TenHang, ct.GiaXuat, ct.SoLuongXuat, ct.MaPhieuXuat,
		                        ct.SoLuongXuat * ct.GiaXuat as ThanhTien
                                FROM ChiTietPhieuXuat ct INNER JOIN
                                                      HangHoa sp ON ct.MaHang = sp.MaHang
                                WHERE (ct.MaPhieuXuat = @MaPhieuXuat)";
                SqlParameter[] parameters =
                {
                    new SqlParameter("@MaPhieuXuat", MaPhieuXuat)
                };


                var dataTable = dbHelper.ExecuteQuery(query, parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    ChiTietPhieuXuatDTO thongke = new ChiTietPhieuXuatDTO
                    {
                        MaCTPX = row["MaCTPX"].ToString(),
                        MaPhieuXuat = row["MaPhieuXuat"].ToString(),
                        MaHang = row["MaHang"].ToString(),
                        TenHang = row["TenHang"].ToString(),
                        SoLuongXuat = (int)row["SoLuongXuat"],
                        GiaXuat = Convert.ToDecimal(row["GiaXuat"]),
                        ThanhTien = Convert.ToDecimal(row["ThanhTien"])
                    };
                    list.Add(thongke);
                }

                return list;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi hiển thị danh sách chi tiết phiếu nhập: {ex.Message}", ex);
            }

        }

        public bool XoaTatCaCTPXTHeoMaHH(string maHH)
        {
            try
            {
                string query = @"DELETE FROM ChiTietPhieuXuat WHERE MaHang = @maHH";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@maHH", maHH)
                };

                var result = dbHelper.ExecuteNonQuery(query, parameters);

                return result > 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public List<ChiTietPhieuXuatDTO> TimKiemCTPX(string info, string MaPhieuXuat)
        {
            try
            {
                List<ChiTietPhieuXuatDTO> list = new();

                string query = @"SELECT ct.MaCTPX, sp.MaHang, sp.TenHang, ct.GiaXuat, ct.SoLuongXuat, ct.MaPhieuXuat,
		                        ct.SoLuongXuat * ct.GiaXuat as ThanhTien
                                FROM ChiTietPhieuXuat ct INNER JOIN
                                                      HangHoa sp ON ct.MaHang = sp.MaHang
                                WHERE (sp.TenHang LIKE '%' + @Info + '%' or ct.MaHang = @Info) 
                                        AND ct.MaPhieuXuat = @MaPhieuXuat";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@Info", info),
                    new SqlParameter("@MaPhieuXuat", MaPhieuXuat),
                };

                var dataTable = dbHelper.ExecuteQuery(query, parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    ChiTietPhieuXuatDTO thongke = new ChiTietPhieuXuatDTO
                    {
                        MaCTPX = row["MaCTPX"].ToString(),
                        MaPhieuXuat = row["MaPhieuXuat"].ToString(),
                        MaHang = row["MaHang"].ToString(),
                        TenHang = row["TenHang"].ToString(),
                        GiaXuat = (decimal)row["GiaXuat"],
                        SoLuongXuat = (int)row["SoLuongXuat"],
                        ThanhTien = (decimal)row["ThanhTien"]
                    };
                    list.Add(thongke);
                }

                return list;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tìm kiếm chi tiết phiếu nhập: {ex.Message}", ex);
            }

        }

        public bool XoaTatCaChiTietCuaPhieuXuat(string MaPhieuXuat)
        {
            try
            {
                string query = @"DELETE FROM ChiTietPhieuXuat WHERE MaPhieuXuat = @MaPhieuXuat";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MaPhieuXuat", MaPhieuXuat)
                };

                var result = dbHelper.ExecuteNonQuery(query, parameters);

                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa chi tiết phiếu nhập: {ex.Message}", ex);
            }
        }


        public int LaySoLuongXuat(string MaCTPX)
        {
            string query = @"SELECT SoLuongXuat FROM ChiTietPhieuXuat
                            WHERE MaCTPX = @MaCTPX";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaCTPX", MaCTPX)
            };

            var result = dbHelper.ExecuteScalar(query, parameters);

            if (result != null && int.TryParse(result.ToString(), out int SoLuongXuat))
            {
                return SoLuongXuat;
            }
            return 0;
        }

        public string TaoMaCTPXMoi()
        {
            // Chưa chỉnh 
            try
            {
                string query = @"select SUBSTRING(MaCTPX, 5, LEN(MaCTPX) - 2) as LastID 
                                 from ChiTietPhieuXuat
                                 order by LastID desc";

                var result = dbHelper.ExecuteScalar(query);

                if (result != null && int.TryParse(result.ToString(), out int lastID))
                {
                    return "CTPX" + (lastID + 1).ToString("D3");
                }

                return "CTPX001";

            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tạo phiếu nhập: " + ex.Message, ex);
            }
        }
    }
}

