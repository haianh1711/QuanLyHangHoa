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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GUI.Views.UserControls
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : UserControl
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void OnQuayLaiClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Đã nhấn nút Quay lại!");
        }

      

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult thoat = MessageBox.Show("Bạn có chắc muốn thoát", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (thoat == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();

            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
          
        }
    }
 }

