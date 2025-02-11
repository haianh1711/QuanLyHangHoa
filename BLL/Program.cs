using BLL;
using DAL;

SanPhamBLL sanPhamBLL = new SanPhamBLL();

var sanPhamList = sanPhamBLL.LayMaVaTenSP();
string message = string.Join(Environment.NewLine, sanPhamList.Select(sp => $"{sp.MaSanPham} - {sp.TenSanPham}"));
Console.WriteLine(message);