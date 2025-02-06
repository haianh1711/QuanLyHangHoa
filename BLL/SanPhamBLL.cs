using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class SanPhamBLL
    {
        private SanPhamDAL sanPhamDAL = new();

        public List<SanPhamDTO> GetMaVaTenSP()
        {
            return sanPhamDAL.GetMaVaTenSP();
        }
    }
}
