using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class HuongDanDAL
    {
        private readonly Dictionary<string, List<string>> _imagePathsByFunction;


        public HuongDanDAL()
        {
            // Danh sách hình ảnh tương ứng với từng chức năng
            _imagePathsByFunction = new Dictionary<string, List<string>>
            {

                { "DoiMatKhau", new List<string> { "14.png", "15.png" } },
                { "QLNhanVien", new List<string> { "7.png", "8.png","9.png" } },
                { "QLKhachHang", new List<string> { "3.png", "4.png" } },
                { "QLHangHoa", new List<string> { "1.png", "2.png"} },
                { "Nhap", new List<string> { "5.png", "6.png" } },
                { "ThongKeTonKho", new List<string> { "10.png", "11.png" } },
                { "ThongKeNhap", new List<string> { "12.png", "13.png" } },
            };
        }

        // Lấy danh sách hình ảnh cho một chức năng cụ thể
            public List<string> GetImagesByFunction(string functionName)
            {
                if (_imagePathsByFunction.ContainsKey(functionName))
                {
                    return _imagePathsByFunction[functionName];
                }
                return new List<string>(); // Trả về danh sách rỗng nếu không có dữ liệu
            }
    }
}
