using DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PhieuNhapDAL
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();
        public bool ThemPhieuNhap(PhieuNhapDTO phieuNhapDTO)
        {
            try
            {
                string query = @"INSERT INTO PhieuNhap (MaPhieuNhap, MaNhanVien, NgayNhap, TongTien)
                             VALUES (@MaPhieuNhap, @MaNhanVien, @NgayNhap, @TongTien);";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MaPhieuNhap", phieuNhapDTO.MaPhieuNhap),
                    new SqlParameter("@MaNhanVien", phieuNhapDTO.MaNhanVien),
                    new SqlParameter("@NgayNhap", phieuNhapDTO.NgayNhap),
                    new SqlParameter("@TongTien", phieuNhapDTO.TongTien)
                };

                var result = dbHelper.ExecuteNonQuery(query, parameters);

                return result > 0;
            }catch (Exception ex)
            {
                throw;
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
