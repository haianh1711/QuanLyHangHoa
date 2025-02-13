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
    public class PhieuXuatDAL
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();

        public List<PhieuXuatDTO> HienThiDanhSachPhieuXuat()
        {
            try
            {
                List<PhieuXuatDTO> ds = new List<PhieuXuatDTO>();
                string query = @"SELECT 
                                pn.MaNhanVien, 
                                pn.NgayXuat, 
                                pn.MaPhieuXuat, 
                                pn.MaKhachHang, 
                                ISNULL(SUM(ct.SoLuongNhap * ct.GiaNhap), 0) AS TongTien
                                FROM PhieuXuat pn
                                LEFT JOIN ChiTietPhieuXuat ct ON pn.MaPhieuXuat = ct.MaPhieuXuat
                                GROUP BY pn.MaNhanVien, pn.NgayXuat, pn.MaPhieuXuat;";

                DataTable dt = dbHelper.ExecuteQuery(query);
                foreach (DataRow row in dt.Rows)
                {
                    PhieuXuatDTO kh = new PhieuXuatDTO
                    {
                        MaNhanVien = row["MaNhanVien"].ToString(),
                        MaKhachHang = row["MaKhachHang"].ToString(),
                        NgayXuat = Convert.ToDateTime(row["NgayXuat"]).ToString("dd/MM/yyyy"),
                        MaPhieuXuat = row["MaPhieuXuat"].ToString(),
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

        public bool ThemPhieuXuat(PhieuXuatDTO PhieuXuatDTO)
        {
            try
            {
                string query = @"INSERT INTO PhieuXuat (MaPhieuXuat, MaNhanVien, NgayXuat, MaKhachHang)
                         VALUES (@MaPhieuXuat, @MaNhanVien, @NgayXuat, @MaKhachHang);";

                SqlParameter[] parameters =
                [
                    new SqlParameter("@MaPhieuXuat", PhieuXuatDTO.MaPhieuXuat),
                    new SqlParameter("@MaKhachHang", PhieuXuatDTO.MaKhachHang),
                    new SqlParameter("@MaNhanVien", PhieuXuatDTO.MaNhanVien),
                    new SqlParameter("@NgayXuat", SqlDbType.DateTime) { Value = DateTime.Parse(PhieuXuatDTO.MaPhieuXuat) },
                ];

                var result = dbHelper.ExecuteNonQuery(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thêm phiếu nhập: {ex.Message}", ex);
            }
        }

        public bool XoaPhieuXuat(string maPhieuXuat)
        {
            try
            {
                string query = @"DELETE FROM PhieuXuat WHERE MaPhieuXuat = @MaPhieuXuat";

                SqlParameter[] parameters =
                [
                    new SqlParameter("@MaPhieuXuat", maPhieuXuat),
                ];

                var result = dbHelper.ExecuteNonQuery(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa phiếu nhập: {ex.Message}", ex);
            }
        }

        public string? TaoMaPXMoi()
        {
            string query = @"select SUBSTRING(MaPhieuXuat, 3, LEN(MaPhieuXuat) - 2) as LastID 
                                 from PhieuXuat
                                 order by LastID desc";

            var result = dbHelper.ExecuteScalar(query);

            if (result != null && int.TryParse(result.ToString(), out int lastID))
            {
                return "PN" + (lastID + 1).ToString("D3");
            }

            return "PX001";
        }
    }
}
