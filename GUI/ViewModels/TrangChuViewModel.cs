using CommunityToolkit.Mvvm.ComponentModel;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.ViewModels
{
    partial class TrangChuViewModel : ObservableObject
    {
        private MainViewModel _mainViewModel;


        public TrangChuViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

    }
}
