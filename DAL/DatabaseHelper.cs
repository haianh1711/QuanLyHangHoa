using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public class DatabaseHelper
    {
        private string connectionString = "Data Source=.;Initial Catalog=QlHangHoa;Integrated Security=True;Trust Server Certificate=True";

        public SqlConnection GetSqlConnection()     
        {
            return new SqlConnection(connectionString);
        }

        // thực hiện các câu lệnh không trả về truy vấn (delete, update, insert,..)
        public int ExecuteNonQuery(string query, SqlParameter[]? parameters = null)
        {
            using (SqlConnection conn = GetSqlConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    return cmd.ExecuteNonQuery();
                }
            }
        }

        // Trả về bảng đối với câu truy vấn (SELECT,...)
        public DataTable ExecuteQuery(string query, SqlParameter[]? parameters = null)
        {
            using (SqlConnection conn = GetSqlConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters); // Đảm bảo thêm tham số vào query
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        // trả về giá trị đơn lẻ (Count, Max, Average,...)
        public object ExecuteScalar(string query, SqlParameter[]? parameters = null)
        {
            using (SqlConnection conn = GetSqlConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    return cmd.ExecuteScalar();
                }
            }
        }


        public SqlDataReader ExecuteReader(string query, SqlParameter[]? parameters = null)
        {
            SqlConnection conn = GetSqlConnection(); // Không dùng `using` để giữ kết nối mở cho SqlDataReader.
            conn.Open();

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                // Trả về SqlDataReader (CommandBehavior.CloseConnection đảm bảo đóng kết nối khi reader bị dispose).
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }
    }
}
