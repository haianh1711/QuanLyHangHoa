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
        private  DatabaseHelper dbHelper = new DatabaseHelper();

        public  List<NhanVienDTO> HienThiDanhSachNV()
        {
            var list = new List<NhanVienDTO>();
            try
            {
                DataTable table = dbHelper.ExecuteQuery("SELECT * FROM NhanVien");
                foreach (DataRow row in table.Rows)
                {
                    list.Add(new NhanVienDTO()
                    {
                        MaNhanVien = row["MaNhanVien"]?.ToString(),
                        TenNhanVien = row["TenNhanVien"]?.ToString(),
                        NgayBatDau = row["NgayBatDau"]?.ToString(),
                        ChucVu = row["ChucVu"]?.ToString(),
                        HinhAnh = row["HinhAnh"]?.ToString(),
                        Gmail = row["Gmail"]?.ToString()

                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"[DAL ERROR] GetAllNhanVien: {ex.Message}");
            }
            return list;
        }
        public  bool SuaNhanVien(NhanVienDTO nv)
        {
            if (nv == null) return false;
            try
            {
                string query = "UPDATE NhanVien SET TenNhanVien=@Ten, NgayBatDau=@Ngay, ChucVu=@ChucVu, HinhAnh=@HinhAnh, Gmail=@Gmail WHERE MaNhanVien=@Ma";
                SqlParameter[] parameters = {
                    new SqlParameter("@Ma", nv.MaNhanVien),
                    new SqlParameter("@Ten", nv.TenNhanVien),
                    new SqlParameter("@Ngay", nv.NgayBatDau),
                    new SqlParameter("@ChucVu", nv.ChucVu),
                    new SqlParameter("@HinhAnh", nv.HinhAnh ?? ""),
                    new SqlParameter("@Gmail", nv.Gmail ?? "")
                };
                return dbHelper.ExecuteNonQuery(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"[DAL ERROR] UpdateNhanVien: {ex.Message}");
            }
        }

        public  bool XoaNhanVien(string maNhanVien)
        {
            if (string.IsNullOrEmpty(maNhanVien)) return false;
            try
            {
                string query = "DELETE FROM NhanVien WHERE MaNhanVien=@Ma";
                SqlParameter[] parameters = {
                    new SqlParameter("@Ma", maNhanVien)
                };
                return dbHelper.ExecuteNonQuery(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"[DAL ERROR] DeleteNhanVien: {ex.Message}");
            }
        }
    }

}
