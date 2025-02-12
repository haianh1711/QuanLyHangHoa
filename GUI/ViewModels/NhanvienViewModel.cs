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
            
        }
    }
}
