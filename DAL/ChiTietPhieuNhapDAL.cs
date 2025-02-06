using DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
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

        

        public string? TaoMaCTPNMoi()
        {
            // Chưa chỉnh 
            try
            {
                string query = @"select SUBSTRING(MaCTPN, 5, LEN(MaCTPN) - 2) as LastID 
                                 from ChiTietPhieuNhap
                                 order by LastID desc";

                var result = dbHelper.ExecuteScalar(query);

                if (result != null && int.TryParse(result.ToString(), out int lastID)) {
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

