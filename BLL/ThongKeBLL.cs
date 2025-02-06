using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;

namespace BLL
{
    public class ThongKeBLL
    {
        private ThongKeDAL thongKeDAL = new ThongKeDAL();

        public List <SanPhamDTO> GetSanPhamThongKe()
        {
            return thongKeDAL.GetSanPhamThongKe();
        }
        public List<SanPhamDTO> SearchTenNhanVien(string Info)
        {
            return thongKeDAL.SearchTenNhanVien(Info);
        }
        public List<PhieuNhapDTO> GetThongKePhieuNhapSanPhamData()
        {
            return thongKeDAL.GetThongKePhieuNhapSanPhamData();
        }
        public List<PhieuNhapDTO> GetThongKePhieuNhapHangThangData()
        {
            return thongKeDAL.GetThongKePhieuNhapHangThangData();
        }
    }
}
