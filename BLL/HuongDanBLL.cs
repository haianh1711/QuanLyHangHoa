using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BLL
{
    public class HuongDanBLL
    {
        private readonly HuongDanDAL dal; // Sử dụng HuongDanDAL thay vì ImageRepository

        public HuongDanBLL()
        {
            dal = new HuongDanDAL();
        }

        // Kiểm tra xem có hình ảnh cho chức năng không
        public bool HasImagesForFunction(string functionName)
        {
            var images = dal.GetImagesByFunction(functionName);
            return images.Count > 0; // Kiểm tra số lượng hình ảnh
        }

        // Lấy danh sách hình ảnh cho một chức năng
        public List<string> GetImagesForFunction(string functionName)
        {
            return dal.GetImagesByFunction(functionName);
        }
    }
}
