using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DoiMatKhauDAL
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();

        public bool DoiMatKhau(string Gmail, string matKhauCu, string matKhauMoi)
        {
            string query = "UPDATE TaiKhoan SET MatKhau = @MatKhauMoi WHERE Gmail = @Gmail AND MatKhau = @MatKhauCu";
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MatKhauCu", SqlDbType.NVarChar) { Value = matKhauCu },
                    new SqlParameter("@MatKhauMoi", SqlDbType.NVarChar) { Value = matKhauMoi }
                };

                int rowsAffected = dbHelper.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
                return false;
            }
        }
    }
}
