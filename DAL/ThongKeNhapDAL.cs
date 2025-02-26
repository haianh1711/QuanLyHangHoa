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
    public class ThongKeNhapDAL
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
        public List<PhieuNhapDTO> GetThongKePhieuNhapHangHoaData()
        {
            List<PhieuNhapDTO> data = new List<PhieuNhapDTO>();
            string query = @"Select ct.MaHang, SUM(ISNULL(ct.SoLuongNhap, 0)) AS TongSoLuongNhap from PhieuNhap pn inner join ChiTietPhieuXuat ct
                            On pn.MaPhieuNhap = ct.MaPhieuNhap
                            Group By MaHang";
            try
            {
                var reader = dbHelper.ExecuteReader(query);
                while (reader.Read())
                {
                    data.Add(new PhieuNhapDTO
                    {
                        MaHang = reader["MaHang"].ToString(),
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
        public List<PhieuNhapDTO> GetThongKePhieuNhapHangHoaTheoTuanData(string tuan, string thang,string nam )
        {
            List<PhieuNhapDTO> data = new List<PhieuNhapDTO>();
            string query = @"Select ct.MaHang, SUM(ISNULL(ct.SoLuongNhap, 0)) AS TongSoLuongNhap from PhieuNhap pn inner join ChiTietPhieuNhap ct
                             On pn.MaPhieuNhap = ct.MaPhieuNhap
                             Where @Tuan = DATEPART(WEEK,pn.NgayNhap) and @Thang = DATEPART(MONTH, pn.NgayNhap) and @nam = DATEPART(YEAR, pn.NgayNhap)
                             Group By MaHang";
            SqlParameter[] parameter = new SqlParameter[]
            {
                new SqlParameter ("@Tuan", tuan),
                new SqlParameter("@Thang",thang),
                new SqlParameter("@Nam",nam)
            };
            try
            {
                var reader = dbHelper.ExecuteReader(query, parameter);
                while (reader.Read())
                {
                    data.Add(new PhieuNhapDTO
                    {

                        MaHang = reader["MaHang"].ToString(),
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
        public List<PhieuNhapDTO> GetThongKePhieuNhapHangHoaTheoThangData(string thang, string nam )
        {
            List<PhieuNhapDTO> data = new List<PhieuNhapDTO>();
            string query = @"Select ct.MaHang, SUM(ISNULL(ct.SoLuongNhap, 0)) AS TongSoLuongNhap FROM PhieuNhap pn inner join ChiTietPhieuNhap ct
                            On pn.MaPhieuNhap = ct.MaPhieuNhap
                            Where @Thang = DATEPART(MONTH, pn.NgayNhap) and @Nam = DATEPART(YEAR, pn.NgayNhap)
                            Group By MaHang";
            SqlParameter[] Parameter = new SqlParameter[]
            {
                new SqlParameter("@Thang", thang),
                new SqlParameter("@Nam",nam)
            };
            try
            {
                var reader = dbHelper.ExecuteReader(query, Parameter);
                while (reader.Read())
                {
                    data.Add(new PhieuNhapDTO
                    {
                        MaHang = reader["MaHang"].ToString(),
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
        public List<PhieuNhapDTO> GetThongKePhieuNhapHangHoaTheoNamData(string nam)
        {
            List<PhieuNhapDTO> data = new List<PhieuNhapDTO>();
            string query = @"Select ct.MaHang, SUM(ISNULL(ct.SoLuongNhap, 0)) AS TongSoLuongNhap FROM PhieuNhap pn inner join ChiTietPhieuNhap ct
                            On pn.MaPhieuNhap = ct.MaPhieuNhap
                            Where @Nam = DATEPART(YEAR, pn.NgayNhap)
                            Group By MaHang";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Nam", nam)
            };

            try
            {
                var reader = dbHelper.ExecuteReader(query, parameters);
                while (reader.Read())
                {
                    data.Add(new PhieuNhapDTO
                    {
                        MaHang = reader["MaHang"].ToString(),
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
        // biểu đồ đường
        public List<ThongKePhieuNhapDTO> GetThongKePhieuNhapHangTuanData()
        {
            List<ThongKePhieuNhapDTO> data = new List<ThongKePhieuNhapDTO>();
            string query = @"Select DATEPART(YEAR,pn.NgayNhap) AS Nam,
                            DATEPART(MONTH,pn.NgayNhap) AS Thang,
                            DATEPART(WEEK,pn.NgayNhap) AS Tuan,
                            SUM(ISNULL(ct.SoLuongNhap, 0)) AS TongSoLuongNhap
                            from PhieuNhap pn left join ChiTietPhieuNhap ct
                            On pn.MaPhieuNhap = ct.MaPhieuNhap
                            Group By DATEPART(YEAR, pn.NgayNhap),DATEPART(MONTH, pn.NgayNhap) , DATEPART(WEEK, pn.NgayNhap)
                            Order By Nam ASC, Thang ASC, Tuan ASC";
            try
            {
                var reader = dbHelper.ExecuteReader(query);
                while (reader.Read())
                {
                    int nam = reader.GetInt32(0);
                    int thang = reader.GetInt32(1);
                    int tuan = reader.GetInt32(2);
                    int tongSoLuong = reader.GetInt32(3);
                    data.Add(new ThongKePhieuNhapDTO
                    {   
                        Tuan = tuan,
                        Thang = thang,
                        Nam = nam,
                        HienThi = $"Tuần {tuan}",
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
       
        public List<ThongKePhieuNhapDTO> GetThongKePhieuNhapHangThangData()
        {
            List<ThongKePhieuNhapDTO> data = new List<ThongKePhieuNhapDTO>();
            string query = @"Select DATEPART(YEAR,pn.NgayNhap) AS Nam,
                             DATEPART(MONTH, pn.NgayNhap) AS Thang,
                             SUM(ISNULL(ct.SoLuongNhap, 0)) AS TongSoLuongNhap
                             from PhieuNhap pn left join ChiTietPhieuNhap ct 
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
                        Thang = thang,
                        Nam = nam,
                        HienThi = $"Tháng {thang:D2}/{nam}",
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

        public List<ThongKePhieuNhapDTO> GetThongKePhieuNhapHangNamData()
        {
            List<ThongKePhieuNhapDTO> data = new List<ThongKePhieuNhapDTO>();
            string query = @"Select DATEPART(YEAR,pn.NgayNhap) AS Nam, 
                            SUM(ISNULL(ct.SoLuongNhap, 0)) AS TongSoLuongNhap
                            from PhieuNhap pn left join ChiTietPhieuNhap ct
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
                        Nam = nam,
                        HienThi = $"Năm {nam}",
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
    }
}
