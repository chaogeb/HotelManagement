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
    /// CheckInWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CheckInWindow : Window
    {
        public CheckInWindow()
        {
            InitializeComponent();
        }

        private void CheckInConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CheckBoxContracts_Checked(object sender, RoutedEventArgs e)
        { }
        private void CheckBoxContracts_Unchecked(object sender, RoutedEventArgs e)
        { }

        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        { }
        private void lstTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Pro WPF 4.5 in C#\Chapter17\ControlTemplateBrowser
        }
    }
}
