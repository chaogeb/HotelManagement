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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Interface;

namespace HotelManagementSystem
{
    /// <summary>
    /// Made by Chaoge Zheng
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool registerEnabled;
        DateTime CurrentTime;

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                CenterWindowOnScreen();
                MainTab.SelectedIndex = 1;
            }
            catch (Exception)
            {
                MessageBox.Show("ERROR");
            }
        }

        // Center MainWindow
        private void CenterWindowOnScreen()
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }

        DispatcherTimer timer;
        private void ShowTimeBlock(object sender, EventArgs e)
        {
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10); //50
            timer.Start();
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            if (TimeProgressBar.Value++ >= 160)
            {
                TimeProgressBar.Value = 0;
                IClock.Time = IClock.Time.AddMinutes(20);
            }
            TimeBlock.Text = IClock.Time.ToLocalTime().ToString();
        }

        #region Main Hall
        // HallTab
        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            Button bt = new Button()
            {
                Width = 80,
                Height = 60
            };
            bt.Margin = new Thickness(5,5,5,5);
            RoomsBox.Children.Add(bt);
        }
        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            RoomsBox.Children.Clear();
        }
        #endregion

        #region New Reservation Tab

        #region Availibility
        // New Reservation Tab - Availibility
        private void CheckAvailabilityBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AvailabilityFromDateDpr.SelectedDate.Value >= AvailabilityToDateDpr.SelectedDate.Value)
                {
                    MessageBox.Show("Please select a check in date before the checkout date");
                }
                else if (AvailabilityFromDateDpr.SelectedDate < DateTime.Today)
                {
                    MessageBox.Show("Please select a date after todays date");
                }
                else
                {
                    //显示可用房间列表
                    //AvailabilityDataGrid.ItemsSource = reservationController.GetAvailableRooms("single room", AvailabilityFromDateDpr.SelectedDate.Value, AvailabilityToDateDpr.SelectedDate.Value);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("You need to choose a room, check in date and check out date first!");
            }
        }

        private void RoomsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        { }

        private void GoToCustomerDetailsBtn_Click(object sender, RoutedEventArgs e)
        {
            BookingTab.SelectedIndex = 1;
        }
        #endregion

        #region Customer Details Tab
        // NewReservationTab - CustomerDetailsTab
        // 预订 - 旅客信息
        private void GoToReceiptBtn_Click(object sender, RoutedEventArgs e)
        {
            BookingTab.SelectedIndex = 2;
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        { }

        private void BackToAvailibiltyBtn_Click(object sender, RoutedEventArgs e)
        {
            BookingTab.SelectedIndex = 0;
        }

        private void CheckBoxRegister_Checked(object sender, RoutedEventArgs e)
        {
            ContractsDetailsSearchBtn.IsEnabled = false;
            ClearContractsDetails();
            EnableDisabletextBoxes(true);
            registerEnabled = true;
        }

        private void ClearContractsDetails()
        {
            ContractsDetailsNameTbx.Text = "";
            ContractsDetailsCreditCardNoTbx.Text = "";
            ContractsDetailsPhoneCountryCodeTbx.Text = "";
            ContractsDetailsPhoneNoTbx.Text = "";
        }

        private void CheckBoxRegister_Unchecked(object sender, RoutedEventArgs e)
        {
            ContractsDetailsSearchBtn.IsEnabled = true;
            EnableDisabletextBoxes(false);
            registerEnabled = false;
        }

        private void EnableDisabletextBoxes(bool enabled)
        {
            ContractsDetailsNameTbx.IsEnabled = enabled;
            ContractsDetailsCreditCardNoTbx.IsEnabled = enabled;
            ContractsDetailsPhoneCountryCodeTbx.IsEnabled = enabled;
            ContractsDetailsPhoneNoTbx.IsEnabled = enabled;
        }
        #endregion

        #region Reciept Tab
        private void RecieptConfirmBtn_Click(object sender, RoutedEventArgs e)
        { }
        #endregion

        #endregion

        #region CheckIn/CheckOut Tab
        private void SearchByNameBtn_Click(object sender, RoutedEventArgs e)
        { }
        private void SearchByResNoBtn_Click(object sender, RoutedEventArgs e)
        { }
        private void CheckInBtn_Click(object sender, RoutedEventArgs e)
        { }
        private void CheckOutBtn_Click(object sender, RoutedEventArgs e)
        { }
        #endregion
    }
}
