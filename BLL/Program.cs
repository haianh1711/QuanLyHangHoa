using System;
using System.Text;
using BLL;
using DAL;
using DTO;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        SanPhamBLL sanPhamBLL = new SanPhamBLL();
        Console.Write("Nhập mã sản phẩm cần xóa: ");
        string maSanPham = Console.ReadLine();

        // Kiểm tra xem sản phẩm có tồn tại không trước khi xóa
        List<SanPhamDTO> sanPhams = sanPhamBLL.TimSanPham(maSanPham);
        if (sanPhams.Count > 0)
        {
            var sanPham = sanPhams[0];
            Console.WriteLine($" Đã tìm thấy sản phẩm: Mã: {sanPham.MaSanPham} - Tên: {sanPham.TenSanPham} - Số lượng: {sanPham.SoLuong}");

            Console.Write(" Bạn có chắc chắn muốn xóa sản phẩm này? (Y/N): ");
            string confirm = Console.ReadLine().Trim().ToLower();

            if (confirm == "y")
            {
                bool daXoa = sanPhamBLL.XoaSanPham(maSanPham);
                if (daXoa)
                {
                    Console.WriteLine(" Xóa sản phẩm thành công!");
                }
                else
                {
                    Console.WriteLine(" Xóa sản phẩm thất bại.");
                }
            }
            else
            {
                Console.WriteLine(" Hủy thao tác xóa.");
            }
        }
        else
        {
            Console.WriteLine(" Không tìm thấy sản phẩm nào với mã này.");
        }
    }
}
