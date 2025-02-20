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

            // Duyệt qua tất cả các RadioButton trong menuContainer
            foreach (var child in menuContainer.Children)
            {
                if (child is RadioButton radioButton)
                {
                    radioButton.IsChecked = false; // Bỏ chọn tất cả các RadioButton
                }
                else if (child is Expander expander) // Kiểm tra trong Expander (Thống kê)
                {
                    foreach (var expChild in ((StackPanel)expander.Content).Children)
                    {
                        if (expChild is RadioButton expRadioButton)
                        {
                            expRadioButton.IsChecked = false; // Bỏ chọn các RadioButton trong Expander
                        }
                    }
                }
            }

            // Đóng Expander nếu nó đang mở
            expanderThongKe.IsExpanded = false;
        }

        private Button currentSelectedButton = null;
        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            btnQuayLai.Visibility = Visibility.Visible;

            if (sender is RadioButton clickedButton)
            {
                // Chỉ cho phép chọn một nút tại một thời điểm
                foreach (var child in menuContainer.Children)
                {
                    if (child is RadioButton radioButton)
                    {
                        radioButton.IsChecked = (radioButton == clickedButton);
                    }
                    else if (child is Expander expander) // Nếu là Expander (Thống kê)
                    {
                        foreach (var expChild in ((StackPanel)expander.Content).Children)
                        {
                            if (expChild is RadioButton expRadioButton)
                            {
                                expRadioButton.IsChecked = (expRadioButton == clickedButton);
                            }
                        }
                    }
                }
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
