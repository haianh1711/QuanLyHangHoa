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
using System.Windows.Threading;
using System.Xml.Serialization;

namespace GUI.Views
{
    /// <summary>
    /// Interaction logic for KhoiDongForm.xaml
    /// </summary>
    public partial class KhoiDongForm : Window
    {
        public KhoiDongForm()
        {
            InitializeComponent();

            IntializeTimer();
        }
        private DispatcherTimer timer;
        private void IntializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(70);
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            ProgressBarLoading.Value += 1;

            if (ProgressBarLoading.Value >= ProgressBarLoading.Maximum)
            {
                timer.Stop();
                OpenMainWindow();
            }
        }
       private void OpenMainWindow()
        {
            // để tạm cái này cho tới khi có form đăng nhập rồi thay :)))
            DangNhapForm dangNhapForm = new DangNhapForm();
            
            dangNhapForm.Show();

            this.Close();
        }

        private void ProgressBarLoading_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
    }
}
