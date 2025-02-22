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
    public class NhanVienDAL
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();

        public List<NhanVienDTO> HienThiDanhSachNV()
        {
            var list = new List<NhanVienDTO>();
            try
            {
                DataTable table = dbHelper.ExecuteQuery("SELECT * FROM NhanVien");
                foreach (DataRow row in table.Rows)
                {
                    list.Add(new NhanVienDTO()
                    {
                        MaNhanVien = row["MaNhanVien"]?.ToString() ?? "",
                        TenNhanVien = row["TenNhanVien"]?.ToString() ?? "",
                        NgayBatDau = row["NgayBatDau"]?.ToString() ?? "",
                        ChucVu = row["ChucVu"]?.ToString() ?? "",
                        HinhAnh = row["HinhAnh"]?.ToString() ?? ""
                    });

                }
            }
            catch (Exception ex)
            {
                throw new Exception($"[DAL ERROR] GetAllNhanVien: {ex.Message}");
            }
            return list;
        }
        public bool SuaNhanVien(NhanVienDTO nv)
        {
            if (nv == null) return false;
            try
            {
                // Loại bỏ Gmail khỏi câu lệnh UPDATE
                string query = "UPDATE NhanVien SET TenNhanVien=@Ten, NgayBatDau=@Ngay, ChucVu=@ChucVu, HinhAnh=@HinhAnh WHERE MaNhanVien=@Ma";
                SqlParameter[] parameters = {
            new SqlParameter("@Ma", nv.MaNhanVien),
            new SqlParameter("@Ten", nv.TenNhanVien),
            new SqlParameter("@Ngay", nv.NgayBatDau),
            new SqlParameter("@ChucVu", nv.ChucVu),
            new SqlParameter("@HinhAnh", nv.HinhAnh ?? "")
        };
                return dbHelper.ExecuteNonQuery(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"[DAL ERROR] UpdateNhanVien: {ex.Message}");
            }
        }


        public bool XoaNhanVien(string maNhanVien)
        {
            if (string.IsNullOrEmpty(maNhanVien)) return false;
            try
            {
                // Xóa tài khoản trước
                string queryTaiKhoan = "DELETE FROM TaiKhoan WHERE MaNhanVien=@Ma";
                SqlParameter[] parametersTaiKhoan = { new SqlParameter("@Ma", maNhanVien) };
                dbHelper.ExecuteNonQuery(queryTaiKhoan, parametersTaiKhoan);

                // Xóa nhân viên
                string queryNhanVien = "DELETE FROM NhanVien WHERE MaNhanVien=@Ma";
                SqlParameter[] parametersNhanVien = { new SqlParameter("@Ma", maNhanVien) };
                return dbHelper.ExecuteNonQuery(queryNhanVien, parametersNhanVien) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"[DAL ERROR] XoaNhanVien: {ex.Message}");
            }
        }
        public bool CapNhatHinhAnh(string maNhanVien, string hinhAnhPath)
        {
            string query = "UPDATE NhanVien SET HinhAnh=@HinhAnh WHERE MaNhanVien=@Ma";
            SqlParameter[] parameters = {
        new SqlParameter("@Ma", maNhanVien),
        new SqlParameter("@HinhAnh", hinhAnhPath)
    };
            return dbHelper.ExecuteNonQuery(query, parameters) > 0;
        }
        public NhanVienDTO LayNhanVienBangMa(string maNhanVien)
        {
            string query = "SELECT * FROM NhanVien WHERE MaNhanVien = @MaNhanVien";

            SqlParameter[] parameters = {
        new SqlParameter("@MaNhanVien", maNhanVien)
    };

            DataTable dt = dbHelper.ExecuteQuery(query, parameters);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new NhanVienDTO
                {
                    MaNhanVien = row["MaNhanVien"].ToString(),
                    TenNhanVien = row["TenNhanVien"].ToString(),
                    NgayBatDau = row["NgayBatDau"].ToString(),
                    ChucVu = row["ChucVu"].ToString(),
                    HinhAnh = row["HinhAnh"].ToString()
                };
            }

            return null;
        }



    }
}