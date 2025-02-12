using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using DTO;

namespace GUI.ViewModels
{
    internal partial class NhanvienViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<NhanVienDTO> data;
        public NhanvienViewModel()
        {
            //data = new ObservableCollection<NhanVienDTO>
            //{
            //    new NhanVienDTO("NV001", "Nguyễn Văn A", "01/01/2021", "Nhân viên", ""),
            //    new NhanVienDTO("NV002", "Trần Thị B", "01/01/2021", "Nhân viên", ""),
            //    new NhanVienDTO("NV003", "Lê Văn C", "01/01/2021", "Nhân viên", ""),
            //    new NhanVienDTO("NV004", "Nguyễn Thị D", "01/01/2021", "Nhân viên", ""),
            //    new NhanVienDTO("NV005", "Trần Văn E", "01/01/2021", "Nhân viên", "")
            //};
        }
    }
}
