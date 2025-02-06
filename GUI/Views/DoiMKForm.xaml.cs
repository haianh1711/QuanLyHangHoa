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
    /// Interaction logic for DoiMKForm.xaml
    /// </summary>
    public partial class DoiMKForm : Window
    {
        public DoiMKForm()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void Button_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Btn_back(object sender, RoutedEventArgs e)
        {

        }

        private void txtMKCu_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtMKmoi_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtMKxacnhan_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Btn_Xacnhan(object sender, RoutedEventArgs e)
        {

        }
    }
}
