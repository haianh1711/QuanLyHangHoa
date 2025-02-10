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
    public class ThongKeDAL
    {
        DatabaseHelper dbHelper = new DatabaseHelper();

        public List<SanPhamDTO> GetSanPhamThongKe()
        {
            List<SanPhamDTO> thongKeList = new List<SanPhamDTO>(); 

            string query = "Select MaSanPham, TenSanPham, SoLuong, MoTa from SanPham";

            try
            {
                var dataTable = dbHelper.ExecuteQuery(query);

                foreach(DataRow row in dataTable.Rows)
                {
                    SanPhamDTO thongke = new SanPhamDTO
                    {
                        MaSanPham = row["MaSanPham"].ToString(),
                        TenSanPham = row["TenSanPham"].ToString(),
                        SoLuong = Convert.ToInt32(row["SoLuong"]),
                        MoTa = row["MoTa"].ToString()
                    };
                    thongKeList.Add(thongke);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }
            return thongKeList;
        }
        public List<SanPhamDTO> SearchTenNhanVien(string Info)
        {
            List<SanPhamDTO> phieuNhapList = new List<SanPhamDTO>();

            SqlParameter[] Parameters = new SqlParameter[]
            {
                new SqlParameter("@Info", Info)
            }; 
            string query = @"Select top 1 sp.MaSanPham, sp.TenSanPham, pn.SoLuong, sp.MoTa 
                             From PhieuNhap pn inner join SanPham sp 
                             On pn.MaSanPham = sp.MaSanPham
                             inner join NhanVien nv
                             On pn.MaNhanVien = nv.MaNhanVien
                             Where pn.MaNhanVien = @Info
                             Or nv.TenNhanVien = @Info";
            try
            {
                var dataTable = dbHelper.ExecuteQuery(query);
                foreach(DataRow row in dataTable.Rows)
                {
                    SanPhamDTO thongKeNhap = new SanPhamDTO
                    {
                        MaSanPham = row["MaSanPham"].ToString(),
                        TenSanPham = row["TenSanPham"].ToString(),
                        SoLuong = Convert.ToInt32(row["SoLuong"]),
                        MoTa = row["MoTa"].ToString()
                    };
                    phieuNhapList.Add(thongKeNhap);
                }
            }
            catch(Exception ex) 
            { 
                Console.WriteLine("Lỗi: " + ex.Message);
            }
            return phieuNhapList;
        }
        public NhanVienDTO GetThongTinNhanVien(string Info)
        {
            string query = @"Select top 1 MaNhanVien, TenNhanVien from NhanVien
                            Where MaNhanVien = @Info or TenNhanVien = @Info";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter ("@Info", Info)
            };
            try
            {
                var dataTable = dbHelper.ExecuteQuery(query);
                if (dataTable.Rows.Count > 0)
                {
                    DataRow row = dataTable.Rows[0];
                    return new NhanVienDTO
                    {
                        MaNhanVien = row["MaNhanVien"].ToString().ToUpper(),
                        TenNhanVien = row["TenNhanVien"].ToString()
                    };
                }
            }
            catch(Exception ex) 
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }
            return null;
        }
        public List<PhieuNhapDTO> GetThongKePhieuNhapSanPhamData()
        {
            List<PhieuNhapDTO> data = new List<PhieuNhapDTO>();
            string query = "Select MaSanPham, SoLuongNhap from ChiTietPhieuNhap";
            try
            {
                var reader = dbHelper.ExecuteReader(query);
                while (reader.Read())
                {
                    data.Add(new PhieuNhapDTO
                    {
                        MaSanPham = reader["MaSanPham"].ToString(),
                        SoLuongNhap = Convert.ToInt32(reader["SoLuongNhap"])
                    });    
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }
            return data;
        }
        public List<PhieuNhapDTO> GetThongKePhieuNhapHangThangData()
        {
            List<PhieuNhapDTO> data = new List<PhieuNhapDTO>();
            string query = @"Select pn.NgayNhap, ct.SoLuongNhap 
                             from PhieuNhap pn inner join ChiTietPhieuNhap ct 
                             On pn.MaPhieuNhap = ct.MaPhieuNhap";
            try
            {
                var reader = dbHelper.ExecuteReader(query);
                while (reader.Read())
                {
                    DateTime date = Convert.ToDateTime(reader["NgayNhap"]);
                    data.Add(new PhieuNhapDTO
                    {
                        NgayNhap =  date.ToString("MM"),
                        SoLuongNhap = Convert.ToInt32(reader["SoLuongNhap"])
                    });
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }
            return data;
        }
    }
}
