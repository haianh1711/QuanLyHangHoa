

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
                            TaiKhoanID = reader["TaiKhoanID"].ToString(),
                            Gmail = reader["Gmail"].ToString(),
                            MatKhau = reader["MatKhau"].ToString() // Giữ lại nếu cần kiểm tra
                        };
                    }
                }
                return null; // Không tìm thấy tài khoản
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi, kiểm tra lại email hoặc mật khẩu: " + ex.Message);
                return null;
            }
        }
    }
}