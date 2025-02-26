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
        private readonly DatabaseHelper dbHelper = new ();

        public bool ThemCTPN(ChiTietPhieuNhapDTO chiTietPhieuNhapDTO)
        {
            try
            {
                string query = @"INSERT INTO ChiTietPhieuNhap (MaCTPN, MaHang, GiaNhap, SoLuongNhap, MaPhieuNhap)
                             VALUES (@MaCTPN, @MaHang, @GiaNhap, @SoLuongNhap, @MaPhieuNhap);";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MaCTPN", chiTietPhieuNhapDTO.MaCTPN),
                    new SqlParameter("@MaHang", chiTietPhieuNhapDTO.MaHang),
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

                string query = @"SELECT ct.MaCTPN, sp.MaHang, sp.TenHang, ct.GiaNhap, ct.SoLuongNhap, ct.MaPhieuNhap,
		                        ct.SoLuongNhap * ct.GiaNhap as ThanhTien
                                FROM ChiTietPhieuNhap ct INNER JOIN
                                                      HangHoa sp ON ct.MaHang = sp.MaHang
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
                        MaHang = row["MaHang"].ToString(),
                        TenHang = row["TenHang"].ToString(),
                        SoLuongNhap = (int)row["SoLuongNhap"],
                        GiaNhap = Convert.ToDouble(row["GiaNhap"]),
                        ThanhTien = Convert.ToDouble(row["ThanhTien"])
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

                string query = @"SELECT ct.MaCTPN, sp.MaHang, sp.TenHang, ct.GiaNhap, ct.SoLuongNhap, ct.MaPhieuNhap,
		                        ct.SoLuongNhap * ct.GiaNhap as ThanhTien
                                FROM ChiTietPhieuNhap ct INNER JOIN
                                                      HangHoa sp ON ct.MaHang = sp.MaHang
                                WHERE (sp.TenHang LIKE '%' + @Info + '%' or ct.MaHang = @Info) 
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
                        MaHang = row["MaHang"].ToString(),
                        TenHang = row["TenHang"].ToString(),
                        GiaNhap = (double)row["GiaNhap"],
                        SoLuongNhap = (int)row["SoLuongNhap"],
                        ThanhTien = (double)row["ThanhTien"]
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

        public int LaySoLuongNhap(string MaCTPN)
        {
            string query = @"SELECT SoLuongNhap FROM ChiTietPhieuNhap
                            WHERE MaCTPN = @MaCTPN";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaCTPN", MaCTPN)
            };

            var result = dbHelper.ExecuteScalar(query, parameters);

            if (result != null && int.TryParse(result.ToString(), out int soLuongNhap))
            {
                return soLuongNhap;
            }
            return 0;
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

