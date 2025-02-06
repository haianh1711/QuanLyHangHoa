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

        public TaiKhoanDTO? DangNhap(string tenTaiKhoan, string matKhau)
        {
            string query = "SELECT * FROM TaiKhoan WHERE TenTaiKhoan = @TenTaiKhoan AND MatKhau = @MatKhau";
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@TenTaiKhoan", SqlDbType.NVarChar) { Value = tenTaiKhoan },
                    new SqlParameter("@MatKhau", SqlDbType.NVarChar) { Value = matKhau }
                };

                using (SqlDataReader reader = dbHelper.ExecuteReader(query, parameters))
                {
                    if (reader.Read())
                    {
                        return new TaiKhoanDTO
                        {
                            TaiKhoanID = reader["TaiKhoanID"].ToString(),
                            TenTaiKhoan = reader["TenTaiKhoan"].ToString(),
                            MatKhau = reader["MatKhau"].ToString(),
                            MaNhanVien = reader["MaNhanVien"].ToString(),
                            Gmail = reader["Gmail"].ToString()
                        };
                    }
                }

                return null; // Không tìm thấy tài khoản
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
                return null;
            }
        }

        public TaiKhoanDTO? DangNhapGmail(string gmail)
        {
            string query = "SELECT * FROM TaiKhoan WHERE Gmail = @Gmail";
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Gmail", SqlDbType.NVarChar) { Value = gmail },
                };

                using (SqlDataReader reader = dbHelper.ExecuteReader(query, parameters))
                {
                    if (reader.Read())
                    {
                        return new TaiKhoanDTO
                        {
                            TaiKhoanID = reader["TaiKhoanID"].ToString(),
                            TenTaiKhoan = reader["TenTaiKhoan"].ToString(),
                            MatKhau = reader["MatKhau"].ToString(),
                            MaNhanVien = reader["MaNhanVien"].ToString(),
                            Gmail = reader["Gmail"].ToString()
                        };
                    }
                }

                return null; // Không tìm thấy tài khoản
            }
            catch (Exception ex)
            {                Console.WriteLine("Lỗi: " + ex.Message);
                return null;
            }
        }

        

    }


}