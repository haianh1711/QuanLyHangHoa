using DAL;
using DTO;

TaiKhoanDTO? taiKhoanDTO = new TaiKhoanDTO();
taiKhoanDTO = new DangNhapDAL().DangNhap("admin", "111");
Console.WriteLine(taiKhoanDTO);
Console.WriteLine("taiKhoanDTO");

