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
using ControllerLayer;
using Interface;

namespace HotelManagementSystem
{
    /// <summary>
    /// Made by Chaoge Zheng
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        FacadeController facade;
        private bool registerEnabled;

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                CenterWindowOnScreen();
                MainTab.SelectedIndex = 1;
                facade = FacadeController.GetInstance();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Window Init Error" + ex.Message);
            }
            InitializeWindowContent();
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
        /// <summary>
        /// Running time of the program
        /// Load from database if exist
        /// else load System time
        /// </summary>
        private void ShowTimeBlock(object sender, EventArgs e)
        {
            facade.GetClock();
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
                IClock.RunClock();
                facade.SetClock();
            }
            TimeBlock.Text = IClock.GetTime;
        }

        private void InitializeWindowContent()
        {
            RoomTypeCombo.ItemsSource = Enum.GetValues(typeof(RoomType));
            ManageRoomTypeCombo.ItemsSource = Enum.GetValues(typeof(RoomType));
            ManageRoomPriceCombo.ItemsSource = Enum.GetValues(typeof(RoomType));
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
        // 预订 - 查询可用房间

        /// <summary>
        /// Made by chaogeb
        /// refresh Room List
        /// </summary>
        private void updateRoomsList()
        {
            RoomType? roomType;
            if (RoomTypeCombo.SelectedIndex == -1)
                roomType = null;
            else
                roomType = (RoomType)Enum.Parse(typeof(RoomType), RoomTypeCombo.SelectedValue.ToString());

            RoomsDataGrid.ItemsSource = facade.GetAvailableRooms(roomType, StartDatePicker.SelectedDate, EndDatePicker.SelectedDate);
        }
        private void CheckAvailabilityBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (StartDatePicker.SelectedDate == null || EndDatePicker.SelectedDate == null)
                {
                    MessageBox.Show("请选确认已择入住日期和离店日期！");
                }
                else if (StartDatePicker.SelectedDate.Value >= EndDatePicker.SelectedDate.Value)
                {
                    MessageBox.Show("请选择一个在入住日期后的离店日期！");
                }
                else if (StartDatePicker.SelectedDate < DateTime.Today)
                {
                    MessageBox.Show("请选择一个不早于今天的日期！");
                }
                else
                {
                    //显示可用房间列表
                    updateRoomsList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Update Room List Error" + ex.Message);
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

        /// <summary>
        /// Search for Customer information via phone
        /// Fill in TextBox
        /// </summary>
        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ContractsDetailsPhoneNoTbx.Text == "")
                MessageBox.Show("请输入搜索的电话号码！");
            else
                try
                {
                    var customer = facade.GetCustomerViaPhone(ContractsDetailsPhoneNoTbx.Text);
                    ContractsDetailsNameTbx.Text = customer.Name;
                    ContractsDetailsCreditCardNoTbx.Text = customer.IDcard;
                }
                catch (Exception)
                {
                    MessageBox.Show("找不到旅客信息！请添加新旅客！");
                }
        }

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
        // New Reservation Tab - Reciept Tab
        // 预订 - 生成订单

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
        {
            CheckInWindow checkinWin = new CheckInWindow();
            checkinWin.ShowDialog();
        }
        private void CheckOutBtn_Click(object sender, RoutedEventArgs e)
        {
            CheckOutWindow checkoutWin = new CheckOutWindow();
            checkoutWin.ShowDialog();
        }
        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        { }
        #endregion

        #region Manage Tab

        private void ManageRoomAdd_Click(object sender, RoutedEventArgs e)
        {
            ManageRoomDelete.IsEnabled = false;
        }
        private void ManageRoomChange_Click(object sender, RoutedEventArgs e)
        {
            ManageRoomDelete.IsEnabled = true;
        }

        #endregion
    }
}
