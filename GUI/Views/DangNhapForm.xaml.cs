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
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult thoat = MessageBox.Show("Bạn có chắc muốn thoát", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (thoat == MessageBoxResult.Yes)
            {
                Close();
            }
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void txtUser_TextChanged(object sender, TextChangedEventArgs e)
        {
            var focus = sender as TextBox;
            if (focus != null && !focus.IsKeyboardFocused)
            {
                focus.Focus();
                focus.CaretIndex = 0;
                e.Handled = true;
            }
        }
        private void txtPass_PasswordChanged(object sender, RoutedEventArgs e)
        {

        }
        private void Btn_Login(object sender, RoutedEventArgs e)
        {
            //Chưa qua xử lý chức năng
            MessageBox.Show("Đăng nhập thành công");
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Bạn đã chọn đăng nhập bằng email");
        }
    }
}
