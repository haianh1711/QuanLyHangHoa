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
    public sealed class PhieuXuatDALTests
    {
        PhieuXuatDAL dal = new PhieuXuatDAL();

        [TestMethod]
        public void HienThiDanhSachPX_CoDuLieu_HienThiDanhSach()
        {
            // arange
            

            // act
            var result = dal.HienThiDanhSachPX();

            // assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() > 0);
        }

        [TestMethod]
        public void ThemPhieuXuat_DuLieuHopLe_TraVeTrue()
        {
            // arange
            PhieuXuatDTO phieuXuatDTO = new PhieuXuatDTO()
            {
                MaPhieuXuat = dal.TaoMaPXMoi(),
                MaKhachHang = "KH001",
                MaNhanVien = "NV001",
                NgayXuat = "2024/12/20"
            };

            using (TransactionScope transaction = new TransactionScope())
            {
                // act
                var result = dal.ThemPhieuXuat(phieuXuatDTO);
                // assert
                Assert.AreEqual(result, true);
            }
        }

        [TestMethod]
        public void XoaPhieuXuat_MaTonTaiVaKhongCoChiTiet_TraVeTrue()
        {
            // arange
            string maPhieuXuat = "PX002";
            using (TransactionScope transaction = new TransactionScope())
            {

                // act
                var result = dal.XoaPhieuXuat(maPhieuXuat);

                // assert
                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void TaoMaPXMoi_DaCoPhieuXuatTrongDB_TraVeMaMoi()
        {
            // arange

            // act
            var result = dal.TaoMaPXMoi();

            // assert
            using (TransactionScope transaction = new TransactionScope())
            { 
                Assert.IsNotNull(result);
                Assert.IsTrue(result.StartsWith("PX"));
                Assert.AreEqual(result.Length, 5);
                Assert.AreEqual(result, "PX004");
            }
        }
    }
}
