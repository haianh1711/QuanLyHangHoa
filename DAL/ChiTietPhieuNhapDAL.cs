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
    public class ChiTietPhieuNhapDAL
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();

        public bool ThemCTPN(ChiTietPhieuNhapDTO chiTietPhieuNhapDTO)
        {
            try
            {
                string query = @"INSERT INTO ChiTietPhieuNhap (MaCTPN, MaSanPham, GiaNhap, SoLuongNhap, MaPhieuNhap)
                             VALUES (@MaCTPN, @MaSanPham, @GiaNhap, @SoLuongNhap, @MaPhieuNhap);";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MaCTPN", chiTietPhieuNhapDTO.MaCTPN),
                    new SqlParameter("@MaSanPham", chiTietPhieuNhapDTO.MaSanPham),
                    new SqlParameter("@GiaNhap", chiTietPhieuNhapDTO.GiaNhap),
                    new SqlParameter("@SoLuongNhap", chiTietPhieuNhapDTO.SoLuongNhap),
                    new SqlParameter("@MaPhieuNhap", chiTietPhieuNhapDTO.MaPhieuNhap)
                };

                var result = dbHelper.ExecuteNonQuery(query, parameters);

                return result > 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool SuaCTPN(ChiTietPhieuNhapDTO chiTietPhieuNhapDTO)
        {
            try
            {
                string query = @"UPDATE ChiTietPhieuNhap 
                         SET GiaNhap = @GiaNhap, 
                             SoLuongNhap = @SoLuongNhap
                         WHERE MaCTPN = @MaCTPN";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@GiaNhap", chiTietPhieuNhapDTO.GiaNhap),
                    new SqlParameter("@SoLuongNhap", chiTietPhieuNhapDTO.SoLuongNhap),
                    new SqlParameter("@MaCTPN", chiTietPhieuNhapDTO.MaCTPN)
                };

                var result = dbHelper.ExecuteNonQuery(query, parameters);

                return result > 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool XoaCTPN(string maCTPN)
        {
            try
            {
                string query = @"DELETE FROM ChiTietPhieuNhap WHERE MaCTPN = @MaCTPN";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MaCTPN", maCTPN)
                };

                var result = dbHelper.ExecuteNonQuery(query, parameters);

                return result > 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<ChiTietPhieuNhapDTO> HienThiDanhSachCTPN(string maPhieuNhap)
        {
            try
            {
                List<ChiTietPhieuNhapDTO> list = new();

                string query = @"SELECT ct.MaCTPN, sp.MaSanPham, sp.TenSanPham, ct.GiaNhap, ct.SoLuongNhap, ct.MaPhieuNhap,
		                        ct.SoLuongNhap * ct.GiaNhap as ThanhTien
                                FROM ChiTietPhieuNhap ct INNER JOIN
                                                      SanPham sp ON ct.MaSanPham = sp.MaSanPham
                                WHERE (ct.MaPhieuNhap = @MaPhieuNhap)";
                SqlParameter[] parameters =
                {
                    new SqlParameter("@MaPhieuNhap", maPhieuNhap)
                };


                var dataTable = dbHelper.ExecuteQuery(query, parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    ChiTietPhieuNhapDTO thongke = new ChiTietPhieuNhapDTO
                    {
                        MaCTPN = row["MaCTPN"].ToString(),
                        MaPhieuNhap = row["MaPhieuNhap"].ToString(),
                        MaSanPham = row["MaSanPham"].ToString(),
                        TenSanPham = row["TenSanPham"].ToString(),
                        GiaNhap = (decimal?)row["GiaNhap"],
                        SoLuongNhap = (int?)row["SoLuongNhap"],
                        ThanhTien = (decimal?)row["ThanhTien"]

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


        public List<ChiTietPhieuNhapDTO> TimKiemCTPN(string info, string maPhieuNhap)
        {
            try
            {
                List<ChiTietPhieuNhapDTO> list = new();

                string query = @"SELECT ct.MaCTPN, sp.MaSanPham, sp.TenSanPham, ct.GiaNhap, ct.SoLuongNhap, ct.MaPhieuNhap,
		                        ct.SoLuongNhap * ct.GiaNhap as ThanhTien
                                FROM ChiTietPhieuNhap ct INNER JOIN
                                                      SanPham sp ON ct.MaSanPham = sp.MaSanPham
                                WHERE (sp.TenSanPham LIKE '%' + @Info + '%' or ct.MaSanPham = @Info) 
                                        AND ct.MaPhieuNhap = @MaPhieuNhap";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@Info", info),
                    new SqlParameter("@MaPhieuNhap", maPhieuNhap),
                };

                var dataTable = dbHelper.ExecuteQuery(query, parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    ChiTietPhieuNhapDTO thongke = new ChiTietPhieuNhapDTO
                    {
                        MaCTPN = row["MaCTPN"].ToString(),
                        MaPhieuNhap = row["MaPhieuNhap"].ToString(),
                        MaSanPham = row["MaSanPham"].ToString(),
                        TenSanPham = row["TenSanPham"].ToString(),
                        GiaNhap = (decimal?)row["GiaNhap"],
                        SoLuongNhap = (int?)row["SoLuongNhap"],
                        ThanhTien = (decimal?)row["ThanhTien"]
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

        public bool XoaTatCaChiTietCuaPhieuNhap(string maPhieuNhap)
        {
            try
            {
                string query = @"DELETE FROM ChiTietPhieuNhap WHERE MaPhieuNhap = @MaPhieuNhap";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MaPhieuNhap", maPhieuNhap)
                };

                var result = dbHelper.ExecuteNonQuery(query, parameters);

                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa chi tiết phiếu nhập: {ex.Message}", ex);
            }
        }

        public string? TaoMaCTPNMoi()
        {
            // Chưa chỉnh 
            try
            {
                string query = @"select SUBSTRING(MaCTPN, 5, LEN(MaCTPN) - 2) as LastID 
                                 from ChiTietPhieuNhap
                                 order by LastID desc";

                var result = dbHelper.ExecuteScalar(query);

                if (result != null && int.TryParse(result.ToString(), out int lastID))
                {
                    return "CTPN" + (lastID + 1).ToString("D3");
                }

                return "CTPN001";

            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi " + ex.ToString());
                return null;
            }
        }
    }
}

