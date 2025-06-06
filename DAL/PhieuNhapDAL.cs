﻿using DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PhieuNhapDAL
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();

        public List<PhieuNhapDTO> HienThiDanhSachPN()
        {
            try
            {
                List<PhieuNhapDTO> ds = new List<PhieuNhapDTO>();
                string query = @"SELECT 
                                pn.MaNhanVien, 
                                pn.NgayNhap, 
                                pn.MaPhieuNhap, 
                                ISNULL(SUM(ct.SoLuongNhap * ct.GiaNhap), 0) AS TongTien
                                FROM PhieuNhap pn
                                LEFT JOIN ChiTietPhieuNhap ct ON pn.MaPhieuNhap = ct.MaPhieuNhap
                                GROUP BY pn.MaNhanVien, pn.NgayNhap, pn.MaPhieuNhap;";

                DataTable dt = dbHelper.ExecuteQuery(query);
                foreach (DataRow row in dt.Rows)
                {
                    PhieuNhapDTO kh = new PhieuNhapDTO
                    {
                        MaNhanVien = row["MaNhanVien"].ToString(),
                        NgayNhap = Convert.ToDateTime(row["NgayNhap"]).ToString("dd/MM/yyyy"),
                        MaPhieuNhap = row["MaPhieuNhap"].ToString(),
                        TongTien = Convert.ToDecimal(row["TongTien"])
                    };
                    ds.Add(kh);
                }

                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi hiển thị danh sách phiếu nhập:" + ex.ToString());
            }
        }

        public PhieuNhapDTO TimKiemPN(string maPN)
        {
            try
            {
                string query = @"SELECT MaPhieuNhap, MaNhanVien, NgayNhap
                         FROM PhieuNhap
                         WHERE MaPhieuNhap = @MaPhieuNhap;";

                SqlParameter[] parameters =
                [
                    new SqlParameter("@MaPhieuNhap", maPN)
                ];

                DataTable dataTable = dbHelper.ExecuteQuery(query, parameters);


                if (dataTable.Rows.Count > 0)
                {
                    DataRow row = dataTable.Rows[0];
                    return new PhieuNhapDTO
                    {
                        MaPhieuNhap = row["MaPhieuNhap"].ToString(),
                        MaNhanVien = row["MaNhanVien"].ToString(),
                        NgayNhap = Convert.ToString(row["NgayNhap"]),
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tìm kiếm phiếu nhap: {ex.Message}", ex);
            }
        }

        public bool ThemPhieuNhap(PhieuNhapDTO phieuNhapDTO)
        {
            try
            {
                string query = @"INSERT INTO PhieuNhap (MaPhieuNhap, MaNhanVien, NgayNhap)
                         VALUES (@MaPhieuNhap, @MaNhanVien, @NgayNhap       );";

                SqlParameter[] parameters =
                [
                    new SqlParameter("@MaPhieuNhap", phieuNhapDTO.MaPhieuNhap),
                    new SqlParameter("@MaNhanVien", phieuNhapDTO.MaNhanVien),
                    new SqlParameter("@NgayNhap", SqlDbType.DateTime) { Value = DateTime.Parse(phieuNhapDTO.NgayNhap) },
                ];

                var result = dbHelper.ExecuteNonQuery(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thêm phiếu nhập: {ex.Message}", ex);
            }
        }

        public bool XoaPhieuNhap(string maPhieuNhap)
        {
            try
            {
                string query1 = @"DELETE FROM ChiTietPhieuNhap WHERE MaPhieuNhap = @MaPhieuNhap";

                SqlParameter[] parameters1 = new SqlParameter[]
                {
                    new SqlParameter("@MaPhieuNhap", maPhieuNhap)
                };

                dbHelper.ExecuteNonQuery(query1, parameters1);

                string query2 = @"DELETE FROM PhieuNhap WHERE MaPhieuNhap = @MaPhieuNhap";

                SqlParameter[] parameters2 =
                [
                    new SqlParameter("@MaPhieuNhap", maPhieuNhap),
                ];

                var result = dbHelper.ExecuteNonQuery(query2, parameters2);
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa phiếu nhập: {ex.Message}", ex);
            }
        }

        public string? TaoMaPNMoi()
        {
            string query = @"select SUBSTRING(MaPhieuNhap, 3, LEN(MaPhieuNhap) - 2) as LastID 
                                 from PhieuNhap
                                 order by LastID desc";

            var result = dbHelper.ExecuteScalar(query);

            if (result != null && int.TryParse(result.ToString(), out int lastID))
            {
                return "PN" + (lastID + 1).ToString("D3");
            }

            return "PN001";
        }
    }
}
