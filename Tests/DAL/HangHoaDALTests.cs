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
    public class HangHoaDALTests
    {
        HangHoaDAL dal = new HangHoaDAL();

        [TestMethod]
        public void HienThiDanhSachHH_CoDuLieu_HienThiDanhSach()
        {
            // arange


            // act
            var result = dal.HienThiDanhSachHH();

            // assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() > 0);
        }

        [TestMethod]
        public void ThemHangHoa_DuLieuHopLe_TraVeTrue()
        {
            // arange
            HangHoaDTO hangHoaDTO = new HangHoaDTO()
            {
                MaHang = "HH004",
                TenHang = "Áo khoác",
                SoLuong = 0,
            };

            using (TransactionScope transaction = new TransactionScope())
            {
                // act
                var result = dal.ThemHangHoa(hangHoaDTO);
                // assert
                Assert.AreEqual(result, true);
            }
        }


        [TestMethod]
        public void TimHangHoa_TimKiemTenCoTonTai_TraVeDanhSachTrungVoiTuKhoa()
        {
            // arange
            string tuKhoa = "Áo";

            // act
            var result = dal.TimHangHoa(tuKhoa);

            // assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() > 0);
        }

        [TestMethod]
        public void TimHangHoa_TimKiemTenKhôngTonTai_TraVeDanhSachRong()
        {
            // arange
            string tuKhoa = "ABCIZ";

            // act
            var result = dal.TimHangHoa(tuKhoa);

            // assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() == 0);
        }

        [TestMethod]
        public void SuaHangHoa_DuLieuHopLe_TraVeTrue()
        {
            // arange
            HangHoaDTO hangHoaDTO = new HangHoaDTO()
            {
                MaHang = "HH003",
                TenHang = "Áo khoác",
                SoLuong = 0,
            };

            using (TransactionScope transaction = new TransactionScope())
            {
                // act
                var result = dal.SuaHangHoa(hangHoaDTO);
                // assert
                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void XoaHangHoa_HangHoaKhongSuDungOChoKhac_TraVeTrue()
        {
            // arange
            string maHH = "HH001";

            using (TransactionScope transaction = new TransactionScope())
            {
                // act
                var result = dal.XoaHangHoa(maHH);
                // assert
                Assert.IsTrue(result);
            }
        }

        //[TestMethod]
        //public void XoaHangHoa_HangHoaDangDuocSuDung_TraVeLoi()
        //{
        //    // arange
        //    string maHH = "HH001";

        //    using (TransactionScope transaction = new TransactionScope())
        //    {
        //        // act
        //        var result = dal.XoaHangHoa(maHH);
        //        // assert
        //        Assert.IsTrue(result);
        //    }
        //}

        [TestMethod]
        public void LaySoLuongHangHoa_MaTonTai_TraVeSoLuong()
        {
            // arange
            string maHH = "HH001";

            using (TransactionScope transaction = new TransactionScope())
            {
                // act
                var result = dal.LaySoLuongHangHoa(maHH);
                // assert
                Assert.IsNotNull(result);
                Assert.AreEqual(result, 247);
            }
        }


    }
}
