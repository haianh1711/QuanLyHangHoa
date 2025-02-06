using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace GUI.Views.UserControls
{
    public partial class GmailNhapForm : UserControl
    {
        public GmailNhapForm()
        {
            InitializeComponent();
        }

        private void InputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PlaceholderText.Visibility = Visibility.Collapsed;
        }

        private void InputTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            // Ẩn Placeholder khi nhấp chuột vào TextBox
            PlaceholderText.Visibility = Visibility.Collapsed;
        }

        private void InputTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            // Hiện lại Placeholder nếu TextBox trống khi mất focus
            if (string.IsNullOrWhiteSpace(InputTieuDe.Text))
            {
                PlaceholderText.Visibility = Visibility.Visible;
            }
        }

        private void InputTexBox_MouseEnter(object sender, MouseEventArgs e)
        {
            // Khi chuột di chuyển vào TextBox, ẩn viền
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                textBox.BorderBrush = Brushes.Transparent;
                textBox.BorderThickness = new Thickness(0);
            }
        }


        private void InputTextBox_TextChangedNDGmail(object sender, TextChangedEventArgs e)
        {
            PlaceholderTextNDGamil.Visibility = Visibility.Collapsed;
        }

        private void InputNDGmail_GotFocus(object sender, RoutedEventArgs e)
        {
            // Ẩn Placeholder khi nhấp chuột vào TextBox
            PlaceholderTextNDGamil.Visibility = Visibility.Collapsed;
        }

        private void InputNDGmail_LostFocus(object sender, RoutedEventArgs e)
        {
            // Hiện lại Placeholder nếu TextBox trống khi mất focus
            if (string.IsNullOrWhiteSpace(InputGmail.Text))
            {
                PlaceholderTextNDGamil.Visibility = Visibility.Visible;
            }
        }

        private void InputNDGmail_MouseEnter(object sender, MouseEventArgs e)
        {
            // Khi chuột di chuyển vào TextBox, ẩn viền
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                textBox.BorderBrush = Brushes.Transparent;
                textBox.BorderThickness = new Thickness(0);
            }
        }
    }
}
