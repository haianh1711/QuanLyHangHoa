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
    class ThongKeXuatBLL
    {
        private ThongKeXuatDAL thongKeXuatDAL = new ThongKeXuatDAL();
        public List<HangHoaDTO> GetHangHoaThongKe()
        {
            return thongKeXuatDAL.GetHangHoaThongKe();
        }
        public List<HangHoaDTO> SearchHangHoa(string Info)
        {
            return thongKeXuatDAL.SearchHangHoa(Info);
        }


        public List<PhieuXuatDTO> GetThongKePhieuXuatHangHoaData()
        {
            return thongKeXuatDAL.GetThongKePhieuXuatHangHoaData();
        }
        public List<PhieuXuatDTO> GetThongKePhieuXuatHangHoaTheoTuanData(string tuan, string thang, string nam)
        {
            return thongKeXuatDAL.GetThongKePhieuXuatHangHoaTheoTuanData(tuan, thang, nam);
        }
        public List<PhieuXuatDTO> GetThongKePhieuXuatHangHoaTheoThangData(string thang, string nam)
        {
            return thongKeXuatDAL.GetThongKePhieuXuatHangHoaTheoThangData(thang, nam);
        }
        public List<PhieuXuatDTO> GetThongKePhieuXuatHangHoaTheoNamData(string nam)
        {
            return thongKeXuatDAL.GetThongKePhieuXuatHangHoaTheoNamData(nam);
        }


        public List<ThongKePhieuXuatDTO> GetThongKePhieuXuatHangTuanData()
        {
            return thongKeXuatDAL.GetThongKePhieuXuatHangTuanData();
        }
        public List<ThongKePhieuXuatDTO> GetThongKePhieuXuatHangThangData()
        {
            return thongKeXuatDAL.GetThongKePhieuXuatHangThangData();
        }
        public List<ThongKePhieuXuatDTO> GetThongKePhieuXuatHangNamData()
        {
            return thongKeXuatDAL.GetThongKePhieuXuatHangNamData();
        }
    }
}
