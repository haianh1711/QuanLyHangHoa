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
    public class ThongKeNhapBLL
    {
        private ThongKeNhapDAL thongKeNhapDAL = new ThongKeNhapDAL();

        public List<HangHoaDTO> GetHangHoaThongKe()
        {
            return thongKeNhapDAL.GetHangHoaThongKe();
        }
        public List<HangHoaDTO> SearchHangHoa(string Info)
        {
           return thongKeNhapDAL.SearchHangHoa(Info);
        }
        public List<PhieuNhapDTO> GetThongKePhieuNhapHangHoaData()
        {
            return thongKeNhapDAL.GetThongKePhieuNhapHangHoaData();
        }
        public List<PhieuNhapDTO> GetThongKePhieuNhapHangHoaTheoTuanData(string tuan, string thang, string nam)
        {
            return thongKeNhapDAL.GetThongKePhieuNhapHangHoaTheoTuanData(tuan, thang, nam);
        }
        public List<PhieuNhapDTO> GetThongKePhieuNhapHangHoaTheoThangData(string thang, string nam)
        {
            return thongKeNhapDAL.GetThongKePhieuNhapHangHoaTheoThangData(thang, nam);
        }
        public List<PhieuNhapDTO> GetThongKePhieuNhapHangHoaTheoNamData(string nam)
        {
            return thongKeNhapDAL.GetThongKePhieuNhapHangHoaTheoNamData(nam);
        }


        public List<ThongKePhieuNhapDTO> GetThongKePhieuNhapHangTuanData()
        {
            return thongKeNhapDAL.GetThongKePhieuNhapHangTuanData();
        }
        public List<ThongKePhieuNhapDTO> GetThongKePhieuNhapHangThangData()
        {
            return thongKeNhapDAL.GetThongKePhieuNhapHangThangData();
        }
        public List<ThongKePhieuNhapDTO> GetThongKePhieuNhapHangNamData()
        {
            return thongKeNhapDAL.GetThongKePhieuNhapHangNamData();
        }
    }
}
