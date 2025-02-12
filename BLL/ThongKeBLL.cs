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

        public List<HangHoaDTO> GetHangHoaThongKe()
        {
            return thongKeDAL.GetHangHoaThongKe();
        }
        public List<HangHoaDTO> SearchTenNhanVien(string Info)
        {
            return thongKeDAL.SearchTenNhanVien(Info);
        }
        public NhanVienDTO GetThongTinNhanVien(string Info)
        {
            return thongKeDAL.GetThongTinNhanVien(Info);
        }
        public List<PhieuNhapDTO> GetThongKePhieuNhapHangHoaData()
        {
            return thongKeDAL.GetThongKePhieuNhapHangHoaData();
        }
        public List<PhieuXuatDTO> GetThongKePhieuXuatHangHoaData()
        {
            return thongKeDAL.GetThongKePhieuXuatHangHoaData();
        }
        public List<ThongKePhieuNhapDTO> GetThongKePhieuNhapHangTuanData()
        {
            return thongKeDAL.GetThongKePhieuNhapHangTuanData();
        }
        public List<ThongKePhieuXuatDTO> GetThongKePhieuXuatHangTuanData()
        {
            return thongKeDAL.GetThongKePhieuXuatHangTuanData();
        }
        public List<ThongKePhieuNhapDTO> GetThongKePhieuNhapHangThangData()
        {
            return thongKeDAL.GetThongKePhieuNhapHangThangData();
        }
        public List<ThongKePhieuXuatDTO> GetThongKePhieuXuatHangThangData()
        {
            return thongKeDAL.GetThongKePhieuXuatHangThangData();
        }
        public List<ThongKePhieuNhapDTO> GetThongKePhieuNhapHangNamData()
        {
            return thongKeDAL.GetThongKePhieuNhapHangNamData();
        }
        public List<ThongKePhieuXuatDTO> GetThongKePhieuXuatHangNamData()
        {
            return thongKeDAL.GetThongKePhieuXuatHangNamData();
        }
    }
}
