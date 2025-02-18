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
    public class ChiTietPhieuNhapDALTests
    {
        ChiTietPhieuNhapDAL dal = new ChiTietPhieuNhapDAL();


        [TestMethod]
        public void HienThiDanhSachCTPN_CoDuLieu_HienThiDanhSach()
        {
            // arange
            string maPN = "PN002";

            // act
            var result = dal.HienThiDanhSachCTPN(maPN);

            // assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() > 0);
        }

        [TestMethod]
        public void ThemCTPN_DuLieuHopLe_TraVeTrue()
        {
            // arange
            ChiTietPhieuNhapDTO chiTiet = new ChiTietPhieuNhapDTO()
            {
                MaCTPN = dal.TaoMaCTPNMoi(),
                MaPhieuNhap = "PN002",
                MaHang = "HH001",
                GiaNhap = 20000,
                SoLuongNhap = 20
            };

            using (TransactionScope transaction = new TransactionScope())
            {
                // act
                var result = dal.ThemCTPN(chiTiet);
                // assert
                Assert.AreEqual(result, true);
            }
        }

        [TestMethod]
        public void SuaCTPN_DuLieuHopLe_TraVeTrue()
        {
            // arange
            ChiTietPhieuNhapDTO chiTiet = new ChiTietPhieuNhapDTO()
            {
                MaCTPN = "CTPN001",
                MaPhieuNhap = "PN002",
                MaHang = "HH002",
                GiaNhap = 230,
                SoLuongNhap = 20
            };

            using (TransactionScope transaction = new TransactionScope())
            {
                // act
                var result = dal.SuaCTPN(chiTiet);
                // assert
                Assert.AreEqual(result, true);
            }
        }

        [TestMethod]
        public void XoaCTPN_MaTonTai_TraVeTrue()
        {
            // arange
            string ma = "CTPN001";
            using (TransactionScope transaction = new TransactionScope())
            {

                // act
                var result = dal.XoaCTPN(ma);

                // assert
                Assert.AreEqual(result, true);
            }
        }

        [TestMethod]
        public void TaoMaCTPNMoi_DaCoCTPNTrongDB_TraVeMaMoi()
        {
            // arange

            // act
            var result = dal.TaoMaCTPNMoi();

            // assert
            using (TransactionScope transaction = new TransactionScope())
            {
                Assert.IsNotNull(result);
                Assert.IsTrue(result.StartsWith("CTPN"));
                Assert.AreEqual(result.Length, 7);
                Assert.AreEqual(result, "CTPN004");
            }
        }
    }
}
