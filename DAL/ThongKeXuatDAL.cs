using Azure.Core;
using DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace DAL
{
    public class ThongKeXuatDAL
    {
        DatabaseHelper dbHelper = new DatabaseHelper();
        public List<HangHoaDTO> GetHangHoaThongKe()
        {
            List<HangHoaDTO> thongKeList = new List<HangHoaDTO>();

            string query = "Select MaHang, TenHang, SoLuong, MoTa from HangHoa";

            try
            {
                var dataTable = dbHelper.ExecuteQuery(query);

                foreach (DataRow row in dataTable.Rows)
                {
                    HangHoaDTO thongke = new HangHoaDTO
                    {
                        MaHang = row["MaHang"].ToString(),
                        TenHang = row["TenHang"].ToString(),
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
        public List<HangHoaDTO> SearchHangHoa(string Info)
        {
            List<HangHoaDTO> phieuNhapList = new List<HangHoaDTO>();

            SqlParameter[] Parameters = new SqlParameter[]
            {
                new SqlParameter("@Info", Info)
            };
            string query = @"Select top 1 sp.MaHang, sp.TenHang, pn.SoLuong, sp.MoTa 
                             From PhieuNhap pn inner join HangHoa sp 
                             On pn.MaHang = sp.MaHang
                             Where MaHang = @Info";
            try
            {
                var dataTable = dbHelper.ExecuteQuery(query);
                foreach (DataRow row in dataTable.Rows)
                {
                    HangHoaDTO thongKeNhap = new HangHoaDTO
                    {
                        MaHang = row["MaHang"].ToString(),
                        TenHang = row["TenHang"].ToString(),
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
        // biểu đồ cột  
        // biểu đồ cột ban đầu
        public List<PhieuXuatDTO> GetThongKePhieuXuatHangHoaData()
        {
            List<PhieuXuatDTO> data = new List<PhieuXuatDTO>();
            string query = @"Select ct.MaHang, SUM(ISNULL(ct.SoLuongXuat, 0)) AS TongSoLuongXuat from PhieuXuat px inner join ChiTietPhieuXuat ct
                            On px.MaPhieuXuat = ct.MaPhieuXuat
                             Group By MaHang";
            try
            {
                var reader = dbHelper.ExecuteReader(query);
                while (reader.Read())
                {
                    data.Add(new PhieuXuatDTO
                    {
                        MaHang = reader["MaHang"].ToString(),
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
        public List<PhieuXuatDTO> GetThongKePhieuXuatHangHoaTheoTuanData(string tuan, string thang , string nam)
        {
            List<PhieuXuatDTO> data = new List<PhieuXuatDTO>();
            string query = @"Select ct.MaHang, SUM(ISNULL(ct.SoLuongXuat, 0)) AS TongSoLuongXuat from PhieuXuat px inner join ChiTietPhieuXuat ct
                            On px.MaPhieuXuat = ct.MaPhieuXuat
                             Where @Tuan = DATEPART(WEEK,px.NgayXuat) and DATEPART(MONTH, px.NgayXuat) and @Nam = DATEPART(YEAR, px.NgayXuat)                         
                             Group By MaHang";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter ("@Tuan", tuan),
                new SqlParameter ("@Thang", thang),
                new SqlParameter ("Nam", nam)
            };
            try
            {
                var reader = dbHelper.ExecuteReader(query, parameters);
                while (reader.Read())
                {
                    data.Add(new PhieuXuatDTO
                    {
                        MaHang = reader["MaHang"].ToString(),
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
        public List<PhieuXuatDTO> GetThongKePhieuXuatHangHoaTheoThangData(string thang, string nam)
        {
            List<PhieuXuatDTO> data = new List<PhieuXuatDTO>();
            string query = @"Select ct.MaHang, SUM(ISNULL(ct.SoLuongXuat, 0)) AS TongSoLuongXuat from PhieuXuat px inner join ChiTietPhieuXuat ct
                            On px.MaPhieuXuat = ct.MaPhieuXuat
                             Where @Thang = DATEPART(MONTH,px.NgayXuat) and @Nam = DATEPART(YEAR, px.NgayXuat)
                             Group By MaHang";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter ("@Thang", thang),
                new SqlParameter("@Nam", nam)
            };
            try
            {
                var reader = dbHelper.ExecuteReader(query, parameters);
                while (reader.Read())
                {
                    data.Add(new PhieuXuatDTO
                    {
                        MaHang = reader["MaHang"].ToString(),
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
        public List<PhieuXuatDTO> GetThongKePhieuXuatHangHoaTheoNamData(string nam)
        {
            List<PhieuXuatDTO> data = new List<PhieuXuatDTO>();
            string query = @"Select ct.MaHang, SUM(ISNULL(ct.SoLuongXuat, 0)) AS TongSoLuongXuat from PhieuXuat px inner join ChiTietPhieuXuat ct
                            On px.MaPhieuXuat = ct.MaPhieuXuat
                            Where @Nam = DATEPART(YEAR,px.NgayXuat)
                            Group By MaHang";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter ("@Nam", nam)
            };
            try
            {
                var reader = dbHelper.ExecuteReader(query, parameters);
                while (reader.Read())
                {
                    data.Add(new PhieuXuatDTO
                    {
                        MaHang = reader["MaHang"].ToString(),
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

        //biểu đồ đường
        public List<ThongKePhieuXuatDTO> GetThongKePhieuXuatHangTuanData()
        {
            List<ThongKePhieuXuatDTO> data = new List<ThongKePhieuXuatDTO>();
            string query = @"Select DATEPART(YEAR,px.NgayXuat) AS Nam,
                            DATEPART(MONTH,px.NgayXuat) AS Thang,
                            DATEPART(WEEK,px.NgayXuat) AS Tuan, 
                            SUM(ISNULL(ct.SoLuongXuat, 0)) AS TongSoLuongXuat
                            from PhieuXuat px left join ChiTietPhieuXuat ct
                            On px.MaPhieuXuat = ct.MaPhieuXuat
                            Group By DATEPART(YEAR, px.NgayXuat), DATEPART(MONTH,px.NgayXuat), DATEPART(WEEK, px.NgayXuat)
                            Order By Nam ASC , Thang ASC ,Tuan ASC";
            try
            {
                var reader = dbHelper.ExecuteReader(query);
                while (reader.Read())
                {
                    int nam = reader.GetInt32(0);
                    int thang = reader.GetInt32(1);
                    int tuan = reader.GetInt32(2);
                    int tongSoLuong = reader.GetInt32(3);
                    data.Add(new ThongKePhieuXuatDTO
                    {
                        Tuan = tuan,
                        Thang = thang,
                        Nam = nam,
                        HienThi = $"Tuần {tuan}",
                        TongSoLuongXuat = tongSoLuong
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
            string query = @"Select DATEPART(YEAR,px.NgayXuat) AS Nam,
                            DATEPART(MONTH,px.NgayXuat) AS Thang, 
                            SUM(ISNULL(ct.SoLuongXuat, 0)) AS TongSoLuongXuat
                            from PhieuXuat px left join ChiTietPhieuXuat ct
                            On px.MaPhieuXuat = ct.MaPhieuXuat
                            Group By DATEPART(YEAR, px.NgayXuat), DATEPART(MONTH, px.NgayXuat)
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
                        Thang = thang,
                        Nam = nam,
                        HienThi = $"Tháng {thang:D2}/{nam}",
                        TongSoLuongXuat = tongSoLuong,
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
            string query = @"Select DATEPART(YEAR,px.NgayXuat) AS Nam,
                            SUM(ISNULL(ct.SoLuongXuat, 0)) AS TongSoLuongXuat
                            from PhieuXuat px left join ChiTietPhieuXuat ct
                            On px.MaPhieuXuat = ct.MaPhieuXuat
                            Group By DATEPART(YEAR, px.NgayXuat)
                            Order By Nam ASC";
            try
            {
                var reader = dbHelper.ExecuteReader(query);
                while (reader.Read())
                {
                    int nam = reader.GetInt32(0);
                    int tongSoLuong = reader.GetInt32(1);
                    data.Add(new ThongKePhieuXuatDTO
                    {
                        Nam = nam,
                        HienThi = $"Năm {nam}",
                        TongSoLuongXuat = tongSoLuong,
                    });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return data;
        }
    }
}
