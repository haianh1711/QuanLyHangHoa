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
using System.IO;
using DTO;
using GUI.ViewModels;

namespace GUI.Views
{
    /// <summary>
    /// Interaction logic for TrangChuHienThiTKForm.xaml
    /// </summary>
    public partial class TrangChuForm : UserControl
    {
        public TrangChuForm(NhanVienDTO? nhanVien)
        {
            InitializeComponent();
        }

        public TrangChuForm()
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DangNhapForm loginWindow = new DangNhapForm();
            loginWindow.Show();

            // Đóng cửa sổ cha chứa UserControl nà
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                parentWindow.Close();
            }
        }


    }
}
