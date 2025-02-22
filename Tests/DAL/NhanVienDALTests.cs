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
    public class NhanVienDALTests
    {
        NhanVienDAL dal = new NhanVienDAL();

        //[TestMethod]
        //public void TimKiem_TimKiemTenCoTonTai_TraVeDanhSachTrungVoiTuKhoa()
        //{
        //    // arange
        //    string tuKhoa = "Vũ";

        //    // act
        //    var result = dal.TimKiem(tuKhoa);

        //    // assert
        //    Assert.IsNotNull(result);
        //    Assert.IsTrue(result.Count() > 0);
        //}

        //[TestMethod]
        //public void TimKiem_TimKiemMaCoTonTai_TraVeDanhSachTrungVoiTuKhoa()
        //{
        //    // arange
        //    string tuKhoa = "KH001";

        //    // act
        //    var result = dal.TimKiem(tuKhoa);

        //    // assert
        //    Assert.IsNotNull(result);
        //    Assert.IsTrue(result.Count() > 0);
        //}

        //[TestMethod]
        //public void TimKiem_TimKiemTenKhôngTonTai_TraVeDanhSachRong()
        //{
        //    // arange
        //    string tuKhoa = "KH009";

        //    // act
        //    var result = dal.TimKiem(tuKhoa);

        //    // assert
        //    Assert.IsNotNull(result);
        //    Assert.IsTrue(result.Count() == 0);
        //}

        [TestMethod]
        public void SuaNhanVien_DuLieuHopLe_TraVeTrue()
        {
            // arange
            NhanVienDTO nv = new NhanVienDTO()
            {
                MaNhanVien = "NV001",
                TenNhanVien = "Nam",
                ChucVu = "nhân viên",
                NgayBatDau = "2024/2/10",
                Gmail = "Nam@gmail.com"
            };

            using (TransactionScope transaction = new TransactionScope())
            {
                // act
                var result = dal.SuaNhanVien(nv);
                // assert
                Assert.IsTrue(result);

            }
        }

        [TestMethod]
        public void XoaNhanVien_NhanVienCoSuDungOChoKhac_TraVeTrue()
        {
            // arange
            string maNV = "NV001";

            using (TransactionScope transaction = new TransactionScope())
            {
                // act
                var result = dal.XoaNhanVien(maNV);
                // assert
                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void HienThiDanhSachNV_CoDuLieu_HienThiDanhSach()
        {
            // arange


            // act
            var result = dal.HienThiDanhSachNV();

            // assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() > 0);
        }
    }
}
