

using DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace DAL
{
    public class DangNhapDAL
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();

        public TaiKhoanDTO? DangNhapGmail(string gmail)
        {
            string query = "SELECT * FROM TaiKhoan WHERE Gmail = @Gmail";
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@Gmail", SqlDbType.NVarChar) { Value = gmail }
                };

                using (SqlDataReader reader = dbHelper.ExecuteReader(query, parameters))
                {
                    if (reader.Read())
                    {
                        return new TaiKhoanDTO
                        {
                            MaTaiKhoan = reader["MaTaiKhoan"].ToString(),
                            Gmail = reader["Gmail"].ToString(),
                            Quyen = reader["Quyen"].ToString(),
                            MaNhanVien = reader["MaNhanVien"].ToString(),
                        };
                    }
                }
                return null; // Không tìm thấy tài khoản
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi đăng nhập bằng gmail (DAL)" + ex.Message);
            }
        }
    }
}