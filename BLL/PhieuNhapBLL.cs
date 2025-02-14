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
    public class PhieuNhapBLL
    {
        private PhieuNhapDAL phieuNhapDAL = new PhieuNhapDAL();
        private ChiTietPhieuNhapBLL chiTietPhieuNhapBLL = new ChiTietPhieuNhapBLL();

        public bool ThemPhieuNhap(PhieuNhapDTO phieuNhap)
        {
            return phieuNhapDAL.ThemPhieuNhap(phieuNhap);

        }

        public bool XoaPhieuNhap(string maPhieuNhap)
        {
            using (TransactionScope transaction = new())
            {
                chiTietPhieuNhapBLL.XoaTatCaChiTietCuaPhieuNhap(maPhieuNhap);
                
                if (phieuNhapDAL.XoaPhieuNhap(maPhieuNhap))
                {
                            transaction.Complete(); return true;
                }
                

                return false;
            }
        }

        public List<PhieuNhapDTO> HienThiDanhSachPhieuNhap()
        {
            return phieuNhapDAL.HienThiDanhSachPhieuNhap();
        }


        public decimal TinhTongTien(List<ChiTietPhieuNhapDTO> chiTietPhieuNhapDTOs)
        {
            decimal sum = (decimal)chiTietPhieuNhapDTOs.Sum(chiTiet => chiTiet.GiaNhap * chiTiet.SoLuongNhap);
            return sum;
        }

        public string? TaoMaPNMoi()
        {
            return phieuNhapDAL.TaoMaPNMoi();
        }
    }
}
