using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using DAL;
using DTO;

namespace BLL
{
    public class PhieuXuatBLL
    {
        private PhieuXuatDAL phieuXuatDAL = new PhieuXuatDAL();
        private ChiTietPhieuXuatBLL chiTietPhieuXuatBLL = new ChiTietPhieuXuatBLL();

        public bool ThemPhieuXuat(PhieuXuatDTO phieuXuat)
        {
            return phieuXuatDAL.ThemPhieuXuat(phieuXuat);

        }

        public bool XoaPhieuXuat(string maPhieuXuat)
        {
            using (TransactionScope transaction = new())
            {
                chiTietPhieuXuatBLL.XoaTatCaChiTietCuaPhieuXuat(maPhieuXuat);


                if (phieuXuatDAL.XoaPhieuXuat(maPhieuXuat))
                { transaction.Complete(); return true; }
                

                return false;
            }
        }

        public List<PhieuXuatDTO> HienThiDanhSachPhieuXuat()
        {
            return phieuXuatDAL.HienThiDanhSachPhieuXuat();
        }


        public decimal TinhTongTien(List<ChiTietPhieuXuatDTO> chiTietPhieuXuatDTOs)
        {
            decimal sum = (decimal)chiTietPhieuXuatDTOs.Sum(chiTiet => chiTiet.GiaXuat * chiTiet.SoLuongXuat);
            return sum;
        }

        public string? TaoMaPXMoi()
        {
            return phieuXuatDAL.TaoMaPXMoi();
        }
    }
}
