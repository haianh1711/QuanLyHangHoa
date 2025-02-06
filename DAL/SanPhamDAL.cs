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

        public List<SanPhamDTO> GetMaVaTenSP()
        {
            try
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
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
 