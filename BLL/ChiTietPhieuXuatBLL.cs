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
    public class ChiTietPhieuXuatBLL
    {
        private ChiTietPhieuXuatDAL ChiTietPhieuXuatDAL = new ChiTietPhieuXuatDAL();
        private HangHoaDAL HangHoaDAL = new();

        public bool ThemDanhSachCTPX(List<ChiTietPhieuXuatDTO> ChiTietPhieuXuatDTOs)
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {
                    foreach (ChiTietPhieuXuatDTO chiTiet in ChiTietPhieuXuatDTOs)
                    {
                        chiTiet.MaCTPX = ChiTietPhieuXuatDAL.TaoMaCTPXMoi();
                        bool result = ChiTietPhieuXuatDAL.ThemCTPX(chiTiet);
                        if (!result) throw new Exception("Thất bại trong việc thêm mã CPTN" + chiTiet.MaCTPX);
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

        public bool SuaDanhSachCTPX(List<ChiTietPhieuXuatDTO> ChiTietPhieuXuatDTOs)
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {
                    foreach (ChiTietPhieuXuatDTO chiTiet in ChiTietPhieuXuatDTOs)
                    {
                        bool result = ChiTietPhieuXuatDAL.SuaCTPX(chiTiet);
                        if (!result) throw new Exception("Thất bại trong việc sửa mã CPTN" + chiTiet.MaCTPX);
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

        public bool XoaTatCaCTPXTHeoMaHH(string maHh)
        {
            return ChiTietPhieuXuatDAL.XoaTatCaCTPXTHeoMaHH(maHh);
        }

        public bool XoaDanhSachCTPX(List<ChiTietPhieuXuatDTO> ChiTietPhieuXuatDTOs)
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {
                    foreach (ChiTietPhieuXuatDTO chiTiet in ChiTietPhieuXuatDTOs)
                    {
                        bool result = ChiTietPhieuXuatDAL.XoaCTPX(chiTiet.MaCTPX);
                        if (!result) throw new Exception("Thất bại trong việc xóa mã CPTN" + chiTiet.MaCTPX);
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

        private bool CapNhapSoLuongTon(ChiTietPhieuXuatDTO chiTiet)
        {
            int SLBanDau = ChiTietPhieuXuatDAL.LaySoLuongXuat(chiTiet.MaPhieuXuat);
            int SLChenhLech = chiTiet.SoLuongXuat - SLBanDau;

            int soLuongTon = HangHoaDAL.LaySoLuongHangHoa(chiTiet.MaHang);
            soLuongTon += SLChenhLech;

            return HangHoaDAL.CapNhatSoLuongHangHoa(chiTiet.MaHang, soLuongTon);
        }

        public List<ChiTietPhieuXuatDTO> HienThiDanhSachCTPX(string maPhieuNhap)
        {
            return ChiTietPhieuXuatDAL.HienThiDanhSachCTPX(maPhieuNhap);
        }

        public bool XoaTatCaChiTietCuaPhieuXuat(string maPhieuNhap)
        {
            return ChiTietPhieuXuatDAL.XoaTatCaChiTietCuaPhieuXuat(maPhieuNhap);
        }

        public decimal TinhThanhTien(ChiTietPhieuXuatDTO ChiTietPhieuXuatDTO)
        {
            return Convert.ToDecimal(ChiTietPhieuXuatDTO.SoLuongXuat * ChiTietPhieuXuatDTO.GiaXuat);
        }

        public string? TaoMaCTPXMoi()
        {
            return ChiTietPhieuXuatDAL.TaoMaCTPXMoi();
        }

        public List<ChiTietPhieuXuatDTO> TimKiemCTPX(string info, string maPhieuNhap)
        {
            return ChiTietPhieuXuatDAL.TimKiemCTPX(info, maPhieuNhap);
        }
    }
}
