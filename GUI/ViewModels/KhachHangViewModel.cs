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
    partial class KhachHangViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<KhachHangDTO> data;

        public KhachHangViewModel() 
        {
            data = new ObservableCollection<KhachHangDTO>
            {
                new KhachHangDTO("KH001", "Trần Văn Sơn", "0907689918", ""),
                new KhachHangDTO("KH002", "Lý Uyên My", "0906784463", "My234@gmail.com"),
                new KhachHangDTO("KH003", "Nguyễn Ngọc Lan Anh", "090875666867", "AnhNguyen098@gmail.com"),
                new KhachHangDTO("KH004", "Lê Thị Ngọc Ánh", "0989765576", "Anh492@gmail.com"),
                new KhachHangDTO("KH005", "Lê Khánh Vân", "0989789576", "Van482@gmail.com")
            };

        }
    }
}
