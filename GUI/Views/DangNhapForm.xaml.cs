using GUI.ViewModels.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GUI.Views
{
    /// <summary>
    /// Interaction logic for DangNhapForm.xaml
    /// </summary>
    public partial class DangNhapForm : Window
    {

        public DangNhapForm()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult thoat = MessageBox.Show("Bạn có chắc muốn thoát", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (thoat ==MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        private void ThongBao_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
