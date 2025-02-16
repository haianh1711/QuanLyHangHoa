using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.ViewModels
{
    partial class TrangChuViewModel : ObservableObject
    {
        private readonly MainViewModel _mainViewModel;

        public TrangChuViewModel()
        {
        }

        public TrangChuViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }
    }
}
