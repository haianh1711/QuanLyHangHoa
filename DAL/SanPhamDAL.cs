using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class SanPhamDAL
    {
        DatabaseHelper dbHelper = new DatabaseHelper();

        public List<SanPhamDTO> LayMaVaTenSP()
        {
            List<SanPhamDTO> sanPhamDTOs = new List<SanPhamDTO>();

            string query = "Select MaSanPham, TenSanPham from SanPham";

            try
            {
                var dataTable = dbHelper.ExecuteQuery(query);

                foreach (DataRow row in dataTable.Rows)
                {
                    SanPhamDTO sanPham = new SanPhamDTO
                    {
                        MaSanPham = row["MaSanPham"].ToString(),
                        TenSanPham = row["TenSanPham"].ToString(),
                    };
                    sanPhamDTOs.Add(sanPham);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }

            return sanPhamDTOs;
        }

        public List<SanPhamDTO> HienThiDanhSachHH()
        {
            List<SanPhamDTO> sanPhamDTOs = new List<SanPhamDTO>();

            string query = "Select MaSanPham, TenSanPham from SanPham";

            try
            {
                var dataTable = dbHelper.ExecuteQuery(query);

                foreach (DataRow row in dataTable.Rows)
                {
                    SanPhamDTO sanPham = new SanPhamDTO
                    {
                        MaSanPham = row["MaSanPham"].ToString(),
                        TenSanPham = row["TenSanPham"].ToString(),
                    };
                    sanPhamDTOs.Add(sanPham);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }

            return sanPhamDTOs;
        }

        public bool ThemSanPham(SanPhamDTO sanPham)
        {
            string query = $"INSERT INTO SanPham (MaSanPham, TenSanPham, DonGia, SoLuong, MoTa, HinhAnh) VALUES ('{sanPham.MaSanPham}', '{sanPham.TenSanPham}', {sanPham.GiaNhap}, {sanPham.SoLuong}, {sanPham.MoTa}, {sanPham.HinhAnh})";

            try
            {
                int rowsAffected = dbHelper.ExecuteNonQuery(query);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm sản phẩm: " + ex.Message);
            }
        }


        public bool CapNhatSanPham(SanPhamDTO sanPham)
        {
            string query = $"UPDATE SanPham SET TenSanPham = '{sanPham.TenSanPham}', DonGia = {sanPham.GiaNhap}, SoLuong = {sanPham.SoLuong}, MoTa = {sanPham.MoTa}, HinhAnh = {sanPham.HinhAnh} WHERE MaSanPham = '{sanPham.MaSanPham}'";

            try
            {
                int rowsAffected = dbHelper.ExecuteNonQuery(query);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật sản phẩm: " + ex.Message);
            }
        }

        public int LaySoLuongSanPham(string maSanPham)
        {
            string query = $"GET SoLuong WHERE MaSanPham = '{maSanPham}'";

            try
            {
                int soLuong = (int)dbHelper.ExecuteScalar(query);
                return soLuong;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật số lượng sản phẩm: " + ex.Message);
            }
        }

        public bool CapNhatSoLuongSanPham(string maSanPham, int soLuongCapNhap)
        {
            string query = $"UPDATE SanPham SET SoLuong = {soLuongCapNhap} WHERE MaSanPham = '{maSanPham}'";

            try
            {
                int rowsAffected = dbHelper.ExecuteNonQuery(query);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy số lượng sản phẩm: " + ex.Message);
            }
        }

        public bool XoaSanPham(string maSanPham)
        {
            string query = $"DELETE FROM SanPham WHERE MaSanPham = '{maSanPham}'";

            try
            {
                int rowsAffected = dbHelper.ExecuteNonQuery(query);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa sản phẩm: " + ex.Message);
            }
        }

        public SanPhamDTO GetChiTietSanPham(string maSanPham)
        {
            string query = $"SELECT * FROM SanPham WHERE MaSanPham = '{maSanPham}'";

            try
            {
                var dataTable = dbHelper.ExecuteQuery(query);

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    DataRow row = dataTable.Rows[0];
                    return new SanPhamDTO
                    {
                        MaSanPham = row["MaSanPham"].ToString(),
                        TenSanPham = row["TenSanPham"].ToString(),
                        GiaNhap = (double?)Convert.ToDecimal(row["GiaNhap"]),
                        SoLuong = Convert.ToInt32(row["SoLuong"])
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy chi tiết sản phẩm: " + ex.Message);
            }

            return null;
        }
    }
}
