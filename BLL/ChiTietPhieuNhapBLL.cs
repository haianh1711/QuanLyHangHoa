using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ChiTietPhieuNhapBLL
    {
        private ChiTietPhieuNhapDAL ChiTietPhieuNhapDAL = new ChiTietPhieuNhapDAL();

        public bool ThemCTPN(ChiTietPhieuNhapDTO chiTietPhieuNhapDTO)
        {
            return ChiTietPhieuNhapDAL.ThemCTPN(chiTietPhieuNhapDTO);
        }

        public decimal? TinhThanhTien(ChiTietPhieuNhapDTO chiTietPhieuNhapDTO)
        {
            return chiTietPhieuNhapDTO.SoLuongNhap * chiTietPhieuNhapDTO.GiaNhap;
        }

        public string? TaoMaCTPNMoi()
        {
            return ChiTietPhieuNhapDAL.TaoMaCTPNMoi();
        }
    }
}
