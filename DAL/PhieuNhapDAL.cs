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
    public class PhieuNhapDAL
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();

        public List<PhieuNhapDTO> HienThiDanhSachPhieuNhap()
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
                string query = @"DELETE FROM PhieuNhap WHERE MaPhieuNhap = @MaPhieuNhap";

                SqlParameter[] parameters =
                [
                    new SqlParameter("@MaPhieuNhap", maPhieuNhap),
                ];

                var result = dbHelper.ExecuteNonQuery(query, parameters);
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

            return "SP001";
        }
    }
}
