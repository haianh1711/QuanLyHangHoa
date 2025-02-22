using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using DAL;
using DTO;

namespace Tests.DAL
{
    [TestClass]
    public sealed class PhieuNhapDALTests
    {
        PhieuNhapDAL dal = new PhieuNhapDAL();

        [TestMethod]
        public void HienThiDanhSachPN_CoDuLieu_HienThiDanhSach()
        {
            // arange
            

            // act
            var result = dal.HienThiDanhSachPN();

            // assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() > 0);
        }

        [TestMethod]
        public void ThemPhieuNhap_DuLieuHopLe_TraVeTrue()
        {
            // arange
            PhieuNhapDTO phieuNhapDTO = new PhieuNhapDTO()
            {
                MaPhieuNhap = dal.TaoMaPNMoi(),
                MaNhanVien = "NV001",
                NgayNhap = "2024/12/20"
            };

            using (TransactionScope transaction = new TransactionScope())
            {
                // act
                var result = dal.ThemPhieuNhap(phieuNhapDTO);
                // assert
                Assert.AreEqual(result, true);
            }
        }

        [TestMethod]
        public void XoaPhieuNhap_MaTonTai_TraVeTrue()
        {
            // arange
            string maPhieuNhap = "PN005";
            using (TransactionScope transaction = new TransactionScope())
            {

                // act
                var result = dal.XoaPhieuNhap(maPhieuNhap);

                // assert
                Assert.AreEqual(result, true);
            }
        }

        [TestMethod]
        public void TaoMaPNMoi_DaCoPhieuNhapTrongDB_TraVeMaMoi()
        {
            // arange

            // act
            var result = dal.TaoMaPNMoi();

            // assert
            using (TransactionScope transaction = new TransactionScope())
            { 
                Assert.IsNotNull(result);
                Assert.IsTrue(result.StartsWith("PN"));
                Assert.AreEqual(result.Length, 5);
                Assert.AreEqual(result, "PN008");
            }
        }
    }
}
