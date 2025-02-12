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

        public List<HangHoaDTO> GetHangHoaThongKe()
        {
            List<HangHoaDTO> thongKeList = new List<HangHoaDTO>();

            string query = "Select MaHangHoa, TenHangHoa, SoLuong, MoTa from HangHoa";

            try
            {
                var dataTable = dbHelper.ExecuteQuery(query);

                foreach (DataRow row in dataTable.Rows)
                {
                    HangHoaDTO thongke = new HangHoaDTO
                    {
                        MaHangHoa = row["MaHangHoa"].ToString(),
                        TenHangHoa = row["TenHangHoa"].ToString(),
                        SoLuong = Convert.ToInt32(row["SoLuong"]),
                        MoTa = row["MoTa"].ToString()
                    };
                    thongKeList.Add(thongke);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return thongKeList;
        }
        public List<HangHoaDTO> SearchTenNhanVien(string Info)
        {
            List<HangHoaDTO> phieuNhapList = new List<HangHoaDTO>();

            SqlParameter[] Parameters = new SqlParameter[]
            {
                new SqlParameter("@Info", Info)
            };
            string query = @"Select top 1 sp.MaHangHoa, sp.TenHangHoa, pn.SoLuong, sp.MoTa 
                             From PhieuNhap pn inner join HangHoa sp 
                             On pn.MaHangHoa = sp.MaHangHoa
                             inner join NhanVien nv
                             On pn.MaNhanVien = nv.MaNhanVien
                             Where pn.MaNhanVien = @Info
                             Or nv.TenNhanVien = @Info";
            try
            {
                var dataTable = dbHelper.ExecuteQuery(query);
                foreach (DataRow row in dataTable.Rows)
                {
                    HangHoaDTO thongKeNhap = new HangHoaDTO
                    {
                        MaHangHoa = row["MaHangHoa"].ToString(),
                        TenHangHoa = row["TenHangHoa"].ToString(),
                        SoLuong = Convert.ToInt32(row["SoLuong"]),
                        MoTa = row["MoTa"].ToString()
                    };
                    phieuNhapList.Add(thongKeNhap);
                }
            }
            catch (Exception ex)
            {
                throw;
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
            catch (Exception ex)
            {
                throw;
            }
            return null;
        }
        public List<PhieuNhapDTO> GetThongKePhieuNhapHangHoaData()
        {
            List<PhieuNhapDTO> data = new List<PhieuNhapDTO>();
            string query = @"Select MaHangHoa, Sum(SoLuongNhap) AS TongSoLuongNhap from ChiTietPhieuNhap
                             Group By MaHangHoa";
            try
            {
                var reader = dbHelper.ExecuteReader(query);
                while (reader.Read())
                {
                    data.Add(new PhieuNhapDTO
                    {
                        MaHangHoa = reader["MaHangHoa"].ToString(),
                        SoLuongNhap = Convert.ToInt32(reader["TongSoLuongNhap"])
                    });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return data;
        }
        public List<PhieuXuatDTO> GetThongKePhieuXuatHangHoaData()
        {
            List<PhieuXuatDTO> data = new List<PhieuXuatDTO>();
            string query = @"Select MaHangHoa, Sum(SoLuongXuat) AS TongSoLuongXuat from ChiTietPhieuXuat
                             Group By MaHangHoa";
            try
            {
                var reader = dbHelper.ExecuteReader(query);
                while (reader.Read())
                {
                    data.Add(new PhieuXuatDTO
                    {
                        MaHangHoa = reader["MaHangHoa"].ToString(),
                        SoLuongXuat = Convert.ToInt32(reader["TongSoLuongXuat"])
                    });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return data;
        }
        public List<ThongKePhieuNhapDTO> GetThongKePhieuNhapHangTuanData()
        {
            List<ThongKePhieuNhapDTO> data = new List<ThongKePhieuNhapDTO>();
            string query = @"Select DATEPART(YEAR,pn.NgayNhap) AS Nam,
                            DATEPART(WEEK,pn.NgayNhap) AS Tuan, 
                            SUM(ct.SoLuongNhap) AS TongSoLuongNhap
                            from PhieuNhap pn inner join ChiTietPhieuNhap ct
                            On pn.MaPhieuNhap = ct.MaPhieuNhap
                            Group By DATEPART(YEAR, pn.NgayNhap), DATEPART(WEEK, pn.NgayNhap)
                            Order By Nam ASC , Tuan ASC";
            try
            {
                var reader = dbHelper.ExecuteReader(query);
                while (reader.Read())
                {
                    int nam = reader.GetInt32(0);
                    int tuan = reader.GetInt32(1);
                    int tongSoLuong = reader.GetInt32(2);
                    data.Add(new ThongKePhieuNhapDTO
                    {
                        ThangNam = $"Tuần {tuan}",
                        TongSoLuongNhap = tongSoLuong
                    });
                }
            }
            catch (Exception ex) 
            {
                throw;
            }
            return data;
        }
        public List<ThongKePhieuXuatDTO> GetThongKePhieuXuatHangTuanData()
        {
            List<ThongKePhieuXuatDTO> data = new List<ThongKePhieuXuatDTO>();
            string query = @"Select DATEPART(YEAR,px.NgayNhap) AS Nam,
                            DATEPART(WEEK,px.NgayNhap) AS Tuan, 
                            SUM(ct.SoLuongXuat) AS TongSoLuongNhap
                            from PhieuXuat px inner join ChiTietPhieuXuat ct
                            On px.MaPhieuXuat = ct.MaPhieuXuat
                            Group By DATEPART(YEAR, px.NgayNhap), DATEPART(WEEK, px.NgayNhap)
                            Order By Nam ASC , Tuan ASC";
            try
            {
                var reader = dbHelper.ExecuteReader(query);
                while (reader.Read())
                {
                    int nam = reader.GetInt32(0);
                    int tuan = reader.GetInt32(1);
                    int tongSoLuong = reader.GetInt32(2);
                    data.Add(new ThongKePhieuXuatDTO
                    {
                        ThangNam = $"Tuần {tuan}",
                        TongSoLuongXuat = tongSoLuong
                    });
                }
            }
            catch(Exception ex) 
            { 
                throw;
            }
            return data;
        }
        public List<ThongKePhieuNhapDTO> GetThongKePhieuNhapHangThangData()
        {
            List<ThongKePhieuNhapDTO> data = new List<ThongKePhieuNhapDTO>();
            string query = @"Select DATEPART(YEAR,pn.NgayNhap) AS Nam,
                             DATEPART(MONTH, pn.NgayNhap) AS Thang,
                             SUM(ct.SoLuongNhap) AS TongSoLuongNhap 
                             from PhieuNhap pn inner join ChiTietPhieuNhap ct 
                             On pn.MaPhieuNhap = ct.MaPhieuNhap
                             Group By DATEPART(YEAR,NgayNhap), DATEPART(MONTH, pn.NgayNhap)
                             Order By Nam ASC, Thang ASC";
            try
            {
                var reader = dbHelper.ExecuteReader(query);
                while (reader.Read())
                {
                    int nam = reader.GetInt32(0);
                    int thang = reader.GetInt32(1);
                    int tongSoLuong = reader.GetInt32(2);
                    data.Add(new ThongKePhieuNhapDTO
                    {
                        ThangNam = $"Tháng {thang:D2}/{nam}",
                        TongSoLuongNhap = tongSoLuong,
                    });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return data;
        }
        public List<ThongKePhieuXuatDTO> GetThongKePhieuXuatHangThangData()
        {
            List<ThongKePhieuXuatDTO> data = new List<ThongKePhieuXuatDTO>();
            string query = @"Select DATEPART(YEAR,px.NgayNhap) AS Nam,
                            DATEPART(MONTH,px.NgayNhap) AS Thang, 
                            SUM(ct.SoLuongXuat) AS TongSoLuongNhap
                            from PhieuXuat px inner join ChiTietPhieuXuat ct
                            On px.MaPhieuXuat = ct.MaPhieuXuat
                            Group By DATEPART(YEAR, px.NgayNhap), DATEPART(MONTH, px.NgayNhap)
                            Order By Nam ASC , Thang ASC";
            try
            {
                var reader = dbHelper.ExecuteReader(query);
                while (reader.Read())
                {
                    int nam = reader.GetInt32(0);
                    int thang = reader.GetInt32(1);
                    int tongSoLuong = reader.GetInt32(2);
                    data.Add(new ThongKePhieuXuatDTO
                    {
                        ThangNam = $"Tháng {thang:D2}/{nam}",
                        TongSoLuongXuat = tongSoLuong,
                    });
                }
            }
            catch(Exception ex) 
            { 
                throw; 
            }
            return data;
        }
        public List<ThongKePhieuNhapDTO> GetThongKePhieuNhapHangNamData()
        {
            List<ThongKePhieuNhapDTO> data = new List<ThongKePhieuNhapDTO>();
            string query = @"Select DATEPART(YEAR,pn.NgayNhap) AS Nam, 
                            SUM(ct.SoLuongNhap) AS TongSoLuongNhap
                            from PhieuNhap pn inner join ChiTietPhieuNhap ct
                            On pn.MaPhieuNhap = ct.MaPhieuNhap
                            Group By DATEPART(YEAR ,pn.NgayNhap)
                            Order By Nam ASC";

            try
            {
                var reader = dbHelper.ExecuteReader(query);
                while (reader.Read())
                {
                    int nam = reader.GetInt32(0);
                    int tongSoLuong = reader.GetInt32(1);
                    data.Add(new ThongKePhieuNhapDTO
                    {
                        ThangNam = $"{nam}",
                        TongSoLuongNhap = tongSoLuong
                    });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return data;
        }
        public List<ThongKePhieuXuatDTO> GetThongKePhieuXuatHangNamData()
        {
            List<ThongKePhieuXuatDTO> data = new List<ThongKePhieuXuatDTO>();
            string query = @"Select DATEPART(YEAR,px.NgayNhap) AS Nam,
                            SUM(ct.SoLuongXuat) AS TongSoLuongNhap
                            from PhieuXuat px inner join ChiTietPhieuXuat ct
                            On px.MaPhieuXuat = ct.MaPhieuXuat
                            Group By DATEPART(YEAR, px.NgayNhap)
                            Order By Nam ASC";
            try
            {
                var reader = dbHelper.ExecuteReader(query);
                while (reader.Read())
                {
                    int nam = reader.GetInt32(0);
                    int thang = reader.GetInt32(1);
                    int tongSoLuong = reader.GetInt32(2);
                    data.Add(new ThongKePhieuXuatDTO
                    {
                        ThangNam = $"Tháng {thang:D2}/{nam}",
                        TongSoLuongXuat = tongSoLuong,
                    });
                }
            }
            catch(Exception ex) 
            { 
                throw; 
            } 
            return data;
        }
    }
}
