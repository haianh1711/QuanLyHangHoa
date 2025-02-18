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
    /// Interaction logic for TrangChuMenu.xaml
    /// </summary>
    public partial class TrangChuMenu : UserControl
    {
        public TrangChuMenu()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult thoat = MessageBox.Show("Bạn có chắc muốn thoát", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (thoat == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        private void btnQuayLai_Click(object sender, RoutedEventArgs e)
        {
             btnQuayLai.Visibility = Visibility.Collapsed;
        }

        private Button currentSelectedButton = null;
        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            btnQuayLai.Visibility = Visibility.Visible;
            if (sender is Button clickedButton)
            {
                // Reset màu của nút trước đó
                if (currentSelectedButton != null)
                {
                    currentSelectedButton.Background = new SolidColorBrush(Colors.Transparent);
                }

                // Đổi màu nút vừa click
                clickedButton.Background = new SolidColorBrush(Color.FromRgb(112, 144, 221)); // Màu khi click

                // Cập nhật nút đang được chọn
                currentSelectedButton = clickedButton;
            }
        }

        private void ClickMouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Button hoveredButton)
            {
                // Nếu nút này không phải nút đang được chọn, đổi màu khi hover
                if (hoveredButton != currentSelectedButton)
                {
                    hoveredButton.Background = new SolidColorBrush(Color.FromRgb(112, 144, 221)); // Màu hover
                }
            }

        }

        private void ClickMouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Button hoveredButton && hoveredButton != currentSelectedButton) // Không đổi màu nếu nút đang được chọn
            {
                hoveredButton.Background = new SolidColorBrush(Colors.Transparent); // Trở về màu cũ
            }

        }

        // Bắt sự kiện khi click vào cửa sổ
        private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // Kiểm tra nếu click vào bên ngoài Expander
            if (!expanderThongKe.IsMouseOver)
            {
                expanderThongKe.IsExpanded = false; // Đóng Expander
            }
        }



    }
}
