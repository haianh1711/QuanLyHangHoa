using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Tests.DAL
{
    [TestClass]
    public class KhachHangDALTests
    {
        KhachHangDAL dal = new KhachHangDAL();

        [TestMethod]
        public void TimKiem_TimKiemTenCoTonTai_TraVeDanhSachTrungVoiTuKhoa()
        {
            // arange
            string tuKhoa = "Vũ";

            // act
            var result = dal.TimKiem(tuKhoa);

            // assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() > 0);
        }

        [TestMethod]
        public void TimKiem_TimKiemMaCoTonTai_TraVeDanhSachTrungVoiTuKhoa()
        {
            // arange
            string tuKhoa = "KH001";

            // act
            var result = dal.TimKiem(tuKhoa);

            // assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() > 0);
        }

        [TestMethod]
        public void TimKiem_TimKiemTenKhôngTonTai_TraVeDanhSachRong()
        {
            // arange
            string tuKhoa = "KH009";

            // act
            var result = dal.TimKiem(tuKhoa);

            // assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() == 0);
        }

        [TestMethod]
        public void SuaKhachHang_DuLieuHopLe_TraVeTrue()
        {
            // arange
            KhachHangDTO khachHangDTO = new KhachHangDTO()
            {
                MaKhachHang = "KH001",
                TenKhachHang = "Nam",
                DiaChi = "ĐN",
                Gmail = "Nam@gmail.com",
                SoDienThoai = "09939020"
            };

            using (TransactionScope transaction = new TransactionScope())
            {
                // act
                var result = dal.SuaKhachHang(khachHangDTO);
                // assert
                Assert.IsTrue(result);

            }
        }

        [TestMethod]
        public void XoaKhachHang_KhachHangKhongSuDungOChoKhac_TraVeTrue()
        {
            // arange
            string maKH = "KH002";

            using (TransactionScope transaction = new TransactionScope())
            {
                // act
                var result = dal.XoaKhachHang(maKH);
                // assert
                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void HienThiDanhSachKH_CoDuLieu_HienThiDanhSach()
        {
            // arange


            // act
            var result = dal.HienThiDanhSachKH();

            // assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() > 0);
        }
    }
}
