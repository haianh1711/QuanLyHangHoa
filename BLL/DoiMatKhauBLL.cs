using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BLL
{
    public class DoiMatKhauBLL
    {
        private DoiMatKhauDAL doiMatKhauDAL = new DoiMatKhauDAL();

        public bool DoiMatKhau(string tenTaiKhoan, string matKhauCu, string matKhauMoi, string xacNhanMatKhau)
        {
            // Kiểm tra mật khẩu mới và xác nhận mật khẩu
            if (string.IsNullOrEmpty(matKhauMoi) || string.IsNullOrEmpty(xacNhanMatKhau))
            {
                throw new ArgumentException("Mật khẩu mới và xác nhận mật khẩu không được để trống.");
            }

            if (matKhauMoi != xacNhanMatKhau)
            {
                throw new ArgumentException("Mật khẩu mới và xác nhận mật khẩu không khớp.");
            }

            if (matKhauMoi == matKhauCu)
            {
                throw new ArgumentException("Mật khẩu mới không được trùng với mật khẩu cũ.");
            }

            // Gửi yêu cầu xuống DAL
            return doiMatKhauDAL.DoiMatKhau(tenTaiKhoan, matKhauCu, matKhauMoi);
        }
    }
}
