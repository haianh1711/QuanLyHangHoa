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

        public bool ThemPhieuNhap(PhieuNhapDTO phieuNhapDTO)
        {
            return phieuNhapDAL.ThemPhieuNhap(phieuNhapDTO);
        }

        public bool LuuPhieu(PhieuNhapDTO phieuNhap, List<ChiTietPhieuNhapDTO> chiTietPhieuNhaps)
        {
            using (TransactionScope transaction = new())
            {
                try
                {
                    if (phieuNhap != null && ThemPhieuNhap(phieuNhap))
                    {
                        foreach (var chiTiet in chiTietPhieuNhaps)
                        {
                            bool saved = chiTietPhieuNhapBLL.ThemCTPN(chiTiet);
                            if (!saved) return false;
                        }
                    }

                    transaction.Complete();
                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi khi lưu phiếu nhập: " + ex.Message, ex);
                }
            }
        }

        public decimal? TinhTongTien(List<ChiTietPhieuNhapDTO> chiTietPhieuNhapDTOs)
        {
            decimal? sum = chiTietPhieuNhapDTOs.Sum(chiTiet => chiTiet.GiaNhap * chiTiet.SoLuongNhap);
            return sum;
        }

        public string? TaoMaPNMoi()
        {
            return phieuNhapDAL.TaoMaPNMoi();
        }
    }
}
