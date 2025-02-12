//using System;
//using System.Text;
//using System.Collections.Generic;
//using BLL;
//using DTO;

//class Program
//{
//    static void Main()
//    {
//        Console.OutputEncoding = Encoding.UTF8;

//        SanPhamBLL sanPhamBLL = new SanPhamBLL();

//        Console.Write("Nhập mã hoặc tên sản phẩm cần tìm: ");
//        string tukhoa = Console.ReadLine();

//        List<HangHoaDTO> danhSachSanPham = sanPhamBLL.TimSanPham(tukhoa);

//        if (danhSachSanPham.Count > 0)
//        {
//            Console.WriteLine("Danh sách sản phẩm tìm thấy:");
//            foreach (var sp in danhSachSanPham)
//            {
//                Console.WriteLine($"Mã: {sp.MaSanPham} - Tên: {sp.TenSanPham} - Số lượng: {sp.SoLuong}");
//            }
//        }
//        else
//        {
//            Console.WriteLine("Không tìm thấy sản phẩm nào.");
//        }
//    }
//}

Console.Write("d");