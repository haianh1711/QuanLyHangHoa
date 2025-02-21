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

            // Kiểm tra menuContainer có phải là Panel không
            if (menuContainer is Panel panel)
            {
                foreach (var child in panel.Children)
                {
                    if (child is RadioButton radioButton)
                    {
                        radioButton.IsChecked = false; // Bỏ chọn tất cả RadioButton
                    }
                    else if (child is Expander expander)
                    {
                        // Kiểm tra nếu nội dung của Expander là StackPanel
                        if (expander.Content is Panel expPanel)
                        {
                            foreach (var expChild in expPanel.Children)
                            {
                                if (expChild is RadioButton expRadioButton)
                                {
                                    expRadioButton.IsChecked = false; // Bỏ chọn các RadioButton trong Expander
                                }
                            }
                        }
                    }
                }
            }

            // Kiểm tra expanderThongKe có null không trước khi sử dụng
            if (expanderThongKe != null)
            {
                expanderThongKe.IsExpanded = false;
            }
        }

        private Button currentSelectedButton = null;
        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            btnQuayLai.Visibility = Visibility.Visible;

            if (sender is RadioButton clickedButton)
            {
                // Duyệt qua tất cả các RadioButton trong menuContainer để bỏ chọn
                foreach (var child in menuContainer.Children)
                {
                    if (child is RadioButton radioButton)
                    {
                        radioButton.IsChecked = (radioButton == clickedButton);
                    }
                    else if (child is Expander expander) // Nếu là Expander (Thống kê)
                    {
                        if (expander.Content is StackPanel expanderPanel)
                        {
                            foreach (var expChild in expanderPanel.Children)
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
