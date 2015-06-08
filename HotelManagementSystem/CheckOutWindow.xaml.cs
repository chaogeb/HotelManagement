using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HotelManagementSystem
{
    /// <summary>
    /// CheckOutWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CheckOutWindow : Window
    {
        public CheckOutWindow()
        {
            InitializeComponent();
        }

        private void CheckOutConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
