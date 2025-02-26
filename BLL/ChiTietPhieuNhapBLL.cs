using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BLL
{
    public class ChiTietPhieuNhapBLL
    {
        private ChiTietPhieuNhapDAL ChiTietPhieuNhapDAL = new ChiTietPhieuNhapDAL();
        private HangHoaDAL HangHoaDAL = new();

        public bool ThemDanhSachCTPN(List<ChiTietPhieuNhapDTO> chiTietPhieuNhapDTOs)
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {
                    foreach (ChiTietPhieuNhapDTO chiTiet in chiTietPhieuNhapDTOs)
                    {
                        chiTiet.MaCTPN = ChiTietPhieuNhapDAL.TaoMaCTPNMoi();
                        bool result = ChiTietPhieuNhapDAL.ThemCTPN(chiTiet);
                        if (!result) throw new Exception("Thất bại trong việc thêm mã CPTN" + chiTiet.MaCTPN);
                        if (!CapNhapSoLuongTon(chiTiet)) return false;
                    }

                    transaction.Complete();
                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception("Thêm danh sách thất bại: " + ex.Message, ex);
                }
            }
        }

        public bool SuaDanhSachCTPN(List<ChiTietPhieuNhapDTO> chiTietPhieuNhapDTOs)
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {
                    foreach (ChiTietPhieuNhapDTO chiTiet in chiTietPhieuNhapDTOs)
                    {
                        bool result = ChiTietPhieuNhapDAL.SuaCTPN(chiTiet);
                        if (!result) throw new Exception("Thất bại trong việc sửa mã CPTN" + chiTiet.MaCTPN);
                        if (!CapNhapSoLuongTon(chiTiet)) return false;
                    }

                    transaction.Complete();
                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception("Sửa danh sách thất bại: " + ex.Message, ex);
                }
            }
        }

        public bool XoaTatCaCTPNTHeoMaHH(string maHh)
        {
            return ChiTietPhieuNhapDAL.XoaTatCaCTPNTHeoMaHH(maHh);
        }

        public bool XoaDanhSachCTPN(List<ChiTietPhieuNhapDTO> chiTietPhieuNhapDTOs)
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {
                    foreach (ChiTietPhieuNhapDTO chiTiet in chiTietPhieuNhapDTOs)
                    {
                        bool result = ChiTietPhieuNhapDAL.XoaCTPN(chiTiet.MaCTPN);
                        if (!result) throw new Exception("Thất bại trong việc xóa mã CPTN" + chiTiet.MaCTPN);
                        if (!CapNhapSoLuongTon(chiTiet)) return false;
                    }

                    transaction.Complete();
                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception("Xóa danh sách thất bại: " + ex.Message, ex);
                }
            }
        }

        private bool CapNhapSoLuongTon(ChiTietPhieuNhapDTO chiTiet)
        {
            int SLBanDau = ChiTietPhieuNhapDAL.LaySoLuongNhap(chiTiet.MaPhieuNhap);
            int SLChenhLech = chiTiet.SoLuongNhap - SLBanDau;

            int soLuongTon = HangHoaDAL.LaySoLuongHangHoa(chiTiet.MaHang);
            soLuongTon += SLChenhLech;

            return HangHoaDAL.CapNhatSoLuongHangHoa(chiTiet.MaHang, soLuongTon);
        }

        public List<ChiTietPhieuNhapDTO> HienThiDanhSachCTPN(string maPhieuNhap)
        {
            return ChiTietPhieuNhapDAL.HienThiDanhSachCTPN(maPhieuNhap);
        }

        public bool XoaTatCaChiTietCuaPhieuNhap(string maPhieuNhap)
        {
            return ChiTietPhieuNhapDAL.XoaTatCaChiTietCuaPhieuNhap(maPhieuNhap);
        }

        public double TinhThanhTien(ChiTietPhieuNhapDTO chiTietPhieuNhapDTO)
        {
            return Convert.ToDouble(chiTietPhieuNhapDTO.SoLuongNhap * chiTietPhieuNhapDTO.GiaNhap);
        }

        public string? TaoMaCTPNMoi()
        {
            return ChiTietPhieuNhapDAL.TaoMaCTPNMoi();
        }

        public List<ChiTietPhieuNhapDTO> TimKiemCTPN(string info, string maPhieuNhap)
        {
            return ChiTietPhieuNhapDAL.TimKiemCTPN(info, maPhieuNhap);
        }
    }
}
