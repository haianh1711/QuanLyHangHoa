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

        public List<PhieuXuatDTO> HienThiDanhSachPX()
        {
            try
            {
                List<PhieuXuatDTO> ds = new List<PhieuXuatDTO>();
                string query = @"SELECT 
                                pn.MaNhanVien, 
                                pn.NgayXuat, 
                                pn.MaPhieuXuat, 
                                pn.MaKhachHang, 
                                ISNULL(SUM(ct.SoLuongXuat * ct.GiaXuat), 0) AS TongTien
                                FROM PhieuXuat pn
                                LEFT JOIN ChiTietPhieuXuat ct ON pn.MaPhieuXuat = ct.MaPhieuXuat
                                GROUP BY pn.MaNhanVien,pn.MaKhachHang, pn.NgayXuat, pn.MaPhieuXuat;";

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
                    new SqlParameter("@NgayXuat", PhieuXuatDTO.NgayXuat),
                ];

                var result = dbHelper.ExecuteNonQuery(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thêm phiếu nhập: {ex.Message}", ex);
            }
        }

        public PhieuXuatDTO TimKiemPX(string maPX)
        {
            try
            {
                string query = @"SELECT MaPhieuXuat, MaNhanVien, NgayXuat, MaKhachHang
                         FROM PhieuXuat
                         WHERE MaPhieuXuat = @MaPhieuXuat;";

                SqlParameter[] parameters =
                [
                    new SqlParameter("@MaPhieuXuat", maPX) 
                ];

                DataTable dataTable = dbHelper.ExecuteQuery(query, parameters);


                if (dataTable.Rows.Count > 0)
                {
                    DataRow row = dataTable.Rows[0];
                    return new PhieuXuatDTO
                    {
                        MaPhieuXuat = row["MaPhieuXuat"].ToString(),
                        MaNhanVien = row["MaNhanVien"].ToString(),
                        NgayXuat = Convert.ToString(row["NgayXuat"]),
                        MaKhachHang = row["MaKhachHang"].ToString()
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tìm kiếm phiếu xuat: {ex.Message}", ex);
            }
        }

        public bool XoaPhieuXuat(string maPhieuXuat)
        {
            try
            {
                string query1 = @"DELETE FROM ChiTietPhieuXuat WHERE MaPhieuXuat = @MaPhieuXuat";

                SqlParameter[] parameters1 = new SqlParameter[]
                {
                    new SqlParameter("@MaPhieuXuat", maPhieuXuat)
                };

                dbHelper.ExecuteNonQuery(query1, parameters1);

                string query2 = @"DELETE FROM PhieuXuat WHERE MaPhieuXuat = @MaPhieuXuat";

                SqlParameter[] parameters2 =
                [
                    new SqlParameter("@MaPhieuXuat", maPhieuXuat),
                ];

                var result = dbHelper.ExecuteNonQuery(query2, parameters2);
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
                return "PX" + (lastID + 1).ToString("D3");
            }

            return "PX001";
        }
    }
}
