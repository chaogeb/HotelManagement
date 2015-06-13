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
        IRoom selectedRoom;
        List<IAvaliableRoom> selectedRoomList = new List<IAvaliableRoom>();
        ICustomer customer;
        IReservation reservation;
        private bool searchbyRooms = false;

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                CenterWindowOnScreen();
                MainTab.SelectedIndex = 0;
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
            timer.Interval = new TimeSpan(0, 0, 0, 0, 50); //50
            timer.Start();
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                TimeProgressBar.Value = 0;
            else if (this.IsActive == false) return;
            else if (TimeProgressBar.Value++ >= 160)
            {
                TimeProgressBar.Value = 0;
                IClock.RunClock();
                facade.SetClock();
                facade.TimeLine();
                LoadFloor(0);
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
        Dictionary<int, int> FloorList;
        private static int SortRooms(IRoom rm1, IRoom rm2)
        {
            return string.CompareOrdinal(rm1.RoomNum, rm2.RoomNum);
        }
        List<List<IRoom>> RoomsInAllFloorList = new List<List<IRoom>>();
        private void InitializeMainHall(object sender, EventArgs e)
        {
            InitializeMainHall();
        }
        private void InitializeMainHall()
        {
            List<IRoom> rooms = facade.GetRooms();
            FloorList = new Dictionary<int, int>();
            RoomsInAllFloorList = new List<List<IRoom>>();
            foreach (IRoom rm in rooms)
            {
                while (rm.RoomNum[0]-'0' > RoomsInAllFloorList.Count)
                    RoomsInAllFloorList.Add(new List<IRoom>());
                RoomsInAllFloorList[rm.RoomNum[0]-'1'].Add(rm);
            }
            int count = 0;
            foreach (List<IRoom> rmlst in RoomsInAllFloorList)
            {
                rmlst.Sort(SortRooms);
                FloorList.Add(count, ++count);
            }
            SelectFloorCbx.ItemsSource = FloorList;
            SelectFloorCbx.SelectedValuePath = "Key";
            SelectFloorCbx.DisplayMemberPath = "Value";
            SelectFloorCbx.SelectedIndex = 0;
        }
        private void LoadFloor(object sender, RoutedEventArgs e)
        {
            LoadFloor(SelectFloorCbx.SelectedIndex);
        }
        private void LoadFloor(object sender, SelectionChangedEventArgs e)
        {
            LoadFloor(SelectFloorCbx.SelectedIndex);
        }
        private void LoadFloor(int floor)
        {
            FloorLbl.Content = (SelectFloorCbx.SelectedIndex + 1) + " 楼";
            RoomsBox.Children.Clear();
            RoomsBox.RowDefinitions.Clear();
            RoomsBox.ColumnDefinitions.Clear();
            if (floor == -1) return;
            int rows = (int)Math.Sqrt(RoomsInAllFloorList[floor].Count);
            for (int i = 0; i < rows; i++)
            {
                RoomsBox.RowDefinitions.Add(new RowDefinition());
            }
            int row = 0, column = 0;
            foreach (IRoom rm in RoomsInAllFloorList[floor])
            {
                var btn = new Button();
                btn.Content = rm.RoomNum + "\n" + rm.RType.ToString();
                btn.FontSize = 25;
                btn.FontStretch = FontStretches.Condensed;
                btn.Margin = new Thickness(5);
                btn.SetValue(Grid.RowProperty, row);
                btn.SetValue(Grid.ColumnProperty, column);
                //btn.Click += (RoutedEventHandler)MessageBox.Show("");
                btn.Background = null;
                if (rm.RStatus == RoomStatus.Occupied)
                {
                    btn.Background = new SolidColorBrush(Color.FromRgb(200, 200, 200));
                }
                row++;
                if (row >= rows)
                {
                    row = 0;
                    column++;
                    RoomsBox.ColumnDefinitions.Add(new ColumnDefinition());
                }
                RoomsBox.Children.Add(btn);
            }

        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

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

        private void refreshTabsStatus(object sender, SelectionChangedEventArgs e)
        {
            refreshTabsStatus();
        }
        private void refreshTabsStatus()
        {
            if (RecieptTab.IsSelected == true)
                RecieptTab.IsEnabled = true;
            else if (ContractsDetailsTab.IsSelected == true)
            {
                RecieptTab.IsEnabled = false;
                ContractsDetailsTab.IsEnabled = true;
            }
            else if (AvailabilityTab.IsSelected == true)
            {
                ContractsDetailsTab.IsEnabled = false;
                RecieptTab.IsEnabled = false;
            }
        }

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
            //DataGridComboBoxColumn.it
        }
        private void CheckAvailabilityBtn_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(((DateTime)StartDatePicker.SelectedDate).ToLongDateString());
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
                else if (StartDatePicker.SelectedDate < IClock.Time.AddDays(-1))
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

        private void AddRoomBox(object sender, DataTransferEventArgs e)
        {
            if (RoomsDataGrid.SelectedIndex != -1)
            {
                var room = RoomsDataGrid.SelectedItem as IAvaliableRoom;
                if (room.ChosenNum > 0 && room.ChosenNum <= room.Remain)
                {
                    selectedRoomList.Add(room);
                    Label Lbl = new Label();
                    Lbl.Content = room.ChosenNum + " × " + room.RType.ToString();
                    RecieptRooms.Children.Add(Lbl);
                    Label Lbl2 = new Label();
                    Lbl2.Content = room.ChosenNum + " × " + room.RType.ToString();
                    AddAvailableRoom.Children.Add(Lbl2);
                }
                else
                    MessageBox.Show("选中房间数值非法！","无法添加房间");
            }
        }

        private void AvailabilityClear(object sender, RoutedEventArgs e)
        {
            RecieptRooms.Children.Clear();
            AddAvailableRoom.Children.Clear();
            StartDatePicker.SelectedDate = null;
            EndDatePicker.SelectedDate = null;
            RoomTypeCombo.SelectedIndex = -1;
            RoomsDataGrid.ItemsSource = null;
            selectedRoomList = new List<IAvaliableRoom>();
        }

        private void GoToCustomerDetailsBtn_Click(object sender, RoutedEventArgs e)
        {
            if (selectedRoomList.Count == 0)
            {
                MessageBox.Show("请选择一个房间");
                return;
            }
            BookingTab.SelectedIndex = 1;
            refreshTabsStatus();
        }
        #endregion

        #region Customer Details Tab
        // NewReservationTab - CustomerDetailsTab
        // 预订 - 旅客信息

        private void GoToReceiptBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ContractsDetailsNameTbx.Text == "")
            {
                MessageBox.Show("请填写联系人姓名");
            }
            else if (ContractsDetailsPhoneNoTbx.Text == "")
            {
                MessageBox.Show("请填写联系电话");
            }
            else
            {
                BookingTab.SelectedIndex = 2;
                refreshTabsStatus();
                customer = facade.CreateCustomer(
                    ContractsDetailsNameTbx.Text,
                    null, null,
                    ContractsDetailsPhoneNoTbx.Text,
                    null,
                    ContractsDetailsCreditCardNoTbx.Text,
                    null, null, null
                    );
                reservation = facade.CreateReservation();
                RecieptDetailsNameTbx.Text = customer.Name;
                RecieptDetailsCreditCardNoTbx.Text = customer.IDcard;
                RecieptPhoneNoTbx.Text = customer.Phone;
                reservationNumberTbx.Text = reservation.ID;
                DateTime date = (DateTime)StartDatePicker.SelectedDate;
                RecieptCheckInDateTbx.Text = date.ToLongDateString();
                date = (DateTime)EndDatePicker.SelectedDate;
                RecieptCheckOutDateTbx.Text = date.ToLongDateString();
                RecieptPhoneCountryCodeTbx.Text = ContractsDetailsPhoneCountryCodeTbx.Text;
            }
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
                    ContractsDetailsConpanyTbx.Text = customer.Company;
                    ContractsDetailsAddrTbx.Text = customer.Address;
                }
                catch (Exception)
                {
                    MessageBox.Show("找不到旅客信息！请添加新旅客！");
                    CheckBoxRegister_Checked(sender, e);
                }
        }

        private void BackToAvailibiltyBtn_Click(object sender, RoutedEventArgs e)
        {
            BookingTab.SelectedIndex = 0;
            refreshTabsStatus();
            ClearContractsDetails();
        }

        private void CheckBoxRegister_Checked(object sender, RoutedEventArgs e)
        {
            ContractsDetailsSearchBtn.IsEnabled = false;
            ClearContractsDetails();
            EnableDisabletextBoxes(true);
        }

        private void ClearContractsDetails()
        {
            ContractsDetailsNameTbx.Text = "";
            ContractsDetailsCreditCardNoTbx.Text = "";
            ContractsDetailsPhoneCountryCodeTbx.Text = "";
            ContractsDetailsPhoneNoTbx.Text = "";
            ContractsDetailsConpanyTbx.Text = "";
            ContractsDetailsAddrTbx.Text = "";
        }

        private void CheckBoxRegister_Unchecked(object sender, RoutedEventArgs e)
        {
            ContractsDetailsSearchBtn.IsEnabled = true;
            EnableDisabletextBoxes(false);
        }

        private void EnableDisabletextBoxes(bool enabled)
        {
            ContractsDetailsNameTbx.IsEnabled = enabled;
            ContractsDetailsCreditCardNoTbx.IsEnabled = enabled;
            ContractsDetailsPhoneCountryCodeTbx.IsEnabled = enabled;
            ContractsDetailsConpanyTbx.IsEnabled = enabled;
            ContractsDetailsAddrTbx.IsEnabled = enabled;
        }
        #endregion

        #region Reciept Tab
        // New Reservation Tab - Reciept Tab
        // 预订 - 生成订单

        private void RecieptConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ArrivalTimeHourCombo.Value == null)
            {
                MessageBox.Show("请填写最晚到店时间");
                return;
            }
            facade.CreateBookings(selectedRoomList,
                (DateTime)StartDatePicker.SelectedDate,
                (DateTime)EndDatePicker.SelectedDate,
                string.Format("{0:HHmm}", ArrivalTimeHourCombo.Value),
                customer.ID, reservation.ID
                );
            facade.ComfirmReservation(reservation, double.Parse(RecieptDownPaymentTbx.Text));
            MessageBox.Show("预订成功！");
            ClearReservationTab(sender, e);
        }

        private void RecieptClear(object sender, RoutedEventArgs e)
        {
            RecieptDetailsNameTbx.Text = "";
            RecieptDetailsCreditCardNoTbx.Text = "";
            RecieptPhoneNoTbx.Text = "";
            reservationNumberTbx.Text = "";
            RecieptCheckInDateTbx.Text = "";
            RecieptCheckOutDateTbx.Text = "";
            RecieptDownPaymentTbx.Text = "";
            RecieptPhoneCountryCodeTbx.Text = "";
            ArrivalTimeHourCombo.Value = null;
        }

        private void ClearReservationTab(object sender, RoutedEventArgs e)
        {
            RecieptClear(sender, e);
            ClearContractsDetails();
            AvailabilityClear(sender, e);
            BookingTab.SelectedIndex = 0;
            refreshTabsStatus();
        }
        #endregion

        #endregion

        #region CheckIn/CheckOut Tab
        private void SearchByNameBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CheckInCheckOutSearchTbx.Text))
            {
                MessageBox.Show("请填写姓名！");
            }
            else
            {
                try
                {
                    searchbyRooms = false;
                    CheckInCheckOutDataGrid.ItemsSource = facade.GetActiveBookingsViaName(CheckInCheckOutSearchTbx.Text);
                    if (CheckInCheckOutDataGrid.Items.Count == 0)
                    {
                        MessageBox.Show("找不到联系人或订单");
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("格式不正确！");
                }
            }
        }
        private void SearchByResNoBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CheckInCheckOutSearchTbx.Text))
            {
                MessageBox.Show("请填写一个订单号！");
            }
            else
            {
                try
                {
                    searchbyRooms = false;
                    CheckInCheckOutDataGrid.ItemsSource = facade.GetActiveBookings(CheckInCheckOutSearchTbx.Text);
                    if (CheckInCheckOutDataGrid.Items.Count == 0)
                    {
                        MessageBox.Show("找不到订单");
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("订单号只能为数字！");
                }
            }
        }
        private void SearchByRoomBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CheckInCheckOutSearchTbx.Text))
            {
                MessageBox.Show("请填写房间号！");
            }
            else
            {
                try
                {
                    searchbyRooms = true;
                    CheckInCheckOutDataGrid.ItemsSource = facade.GetRoomViaNum(CheckInCheckOutSearchTbx.Text);
                    if (CheckInCheckOutDataGrid.Items.Count == 0)
                    {
                        MessageBox.Show("房间不存在");
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("房间格式不正确！");
                }
            }
        }
        private void AllBtn_Click(object sender, RoutedEventArgs e)
        {
            searchbyRooms = false;
            CheckInCheckOutDataGrid.ItemsSource = facade.GetActiveBookings(null);
            if (CheckInCheckOutDataGrid.Items.Count == 0)
            {
                MessageBox.Show("没有找到订单");
            }
        }
        private void CheckInBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CheckInCheckOutDataGrid.SelectedItem == null)
            {
                MessageBox.Show("请选择一个订单！");
                return;
            }
            IBooking bk = CheckInCheckOutDataGrid.SelectedItem as IBooking;
            if (bk.RoomID != "")
            {
                MessageBox.Show("订单已入住！");
                return;
            }
            CheckInWindow checkinWin = new CheckInWindow((CheckInCheckOutDataGrid.SelectedItem as IBooking).ID);
            checkinWin.ShowDialog();
        }
        private void CheckOutBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CheckInCheckOutDataGrid.SelectedItem == null)
            {
                MessageBox.Show("请选择一个订单！");
                return;
            }
            else if (searchbyRooms)
            {
                IRoom rm = CheckInCheckOutDataGrid.SelectedItem as IRoom;
                if (rm.RStatus != RoomStatus.Occupied)
                {
                    MessageBox.Show("房间没有人入住！");
                    return;
                }
                CheckOutWindow checkoutWin = new CheckOutWindow(CheckInCheckOutDataGrid.SelectedItem as IRoom);
                checkoutWin.ShowDialog();
            }
            else if (!searchbyRooms)
            {
                IBooking bk = CheckInCheckOutDataGrid.SelectedItem as IBooking;
                if (bk.RoomID == "")
                {
                    MessageBox.Show("未入住，不能离店！");
                    return;
                }
                CheckOutWindow checkoutWin = new CheckOutWindow((CheckInCheckOutDataGrid.SelectedItem as IBooking).ID);
                checkoutWin.ShowDialog();
            }
        }
        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CheckInCheckOutDataGrid.SelectedItem == null)
            {
                MessageBox.Show("请选择一个订单！");
                return;
            }
            else if (searchbyRooms)
            {
                IRoom rm = CheckInCheckOutDataGrid.SelectedItem as IRoom;
                IBooking bk = null;
                var book = facade.GetActiveBookings(null);
                foreach (IBooking bktemp in book)
                {
                    if (bktemp.RoomID == rm.ID)
                    {
                        bk = bktemp;
                        break;
                    }
                }
                try
                {
                    if(bk != null)
                    {
                        facade.CancelBooking(bk.ID);
                        MessageBox.Show("订单已取消");
                        facade.Log_Cancel(bk);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("取消 Room 订单错误\n" + ex);
                }
            }
            else if (!searchbyRooms)
            {
                IBooking bk = CheckInCheckOutDataGrid.SelectedItem as IBooking;
                try
                {
                    facade.CancelBooking(bk.ID);
                    MessageBox.Show("订单已取消");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("取消 Booking 订单错误\n" + ex);
                }
            }
        }
        #endregion

        #region Manage Tab

        private void UpdateManageRoomDataGrid(object sender, RoutedEventArgs e)
        {
            UpdateManageRoomDataGrid();
        }
        private void UpdateManageRoomDataGrid()
        {
            ManageRoomDataGrid.ItemsSource = facade.GetRooms();
        }
        private void UpdateManageRoomGroup(string RoomID)
        {
            IRoom room = facade.GetRoom(RoomID);
            ManageRoomID.Content = room.ID;
            ManageRoomNum.Text = room.RoomNum;
            ManageRoomTypeCombo.SelectedIndex = (int)room.RType;
        }
        private void ResetManageRoomGroup()
        {
            ManageRoomID.Content = "0";
            ManageRoomNum.Text = "";
            ManageRoomTypeCombo.SelectedIndex = -1;
        }
        private void ManageRoomDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((selectedRoom = (ManageRoomDataGrid.SelectedValue as IRoom)) != null)
            {
                ManageRoomChangeRadio.IsChecked = true;
                ManageRoomDelete.IsEnabled = true;
                UpdateManageRoomGroup(selectedRoom.ID);
                UpdateManageRoomPrice(selectedRoom.RType);
            }
            else
            {
                ResetManageRoomGroup();
            }
        }
        private void ManageRoomAddRadio_Click(object sender, RoutedEventArgs e)
        {
            ManageRoomDelete.IsEnabled = false;
            ResetManageRoomGroup();
        }
        private void ManageRoomChangeRadio_Click(object sender, RoutedEventArgs e)
        {
            ManageRoomDelete.IsEnabled = true;
        }

        private void ManageRoomDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ManageRoomNum.Text == "" || ManageRoomTypeCombo.SelectedIndex == -1)
                    return;
                IRoom room = facade.GetRoom(ManageRoomID.Content.ToString());
                if (room == null) return;
                room.RStatus = RoomStatus.NA;
                facade.UpdateRoom(room);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Manage Room Delete Error!\n" + ex.Message);
            }
            finally
            {
                UpdateManageRoomDataGrid();
            }
        }
        private void ManageRoomSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ManageRoomNum.Text == "" || ManageRoomTypeCombo.SelectedIndex == -1)
                {
                    MessageBox.Show("请选择一个房间或检查信息是否完整");
                    return;
                }
                if (ManageRoomAddRadio.IsChecked == true)
                {
                    facade.CreateRoom(ManageRoomNum.Text, (RoomType)ManageRoomTypeCombo.SelectedItem);
                }
                else if (ManageRoomChangeRadio.IsChecked == true && ManageRoomDataGrid.SelectedIndex != -1)
                {
                    IRoom room = facade.GetRoom(ManageRoomID.Content.ToString());
                    room.RoomNum = ManageRoomNum.Text;
                    room.RType = (RoomType)ManageRoomTypeCombo.SelectedIndex;
                    facade.UpdateRoom(room);
                }
                else MessageBox.Show("请选择一个房间或检查信息是否完整");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Manage Room Save Error!\n" + ex.Message);
            }
            finally
            {
                UpdateManageRoomDataGrid();
            }
        }

        private void UpdateManageRoomPrice(RoomType rtype)
        {
            ManageRoomPriceCombo.SelectedItem = rtype;
        }
        private void ManageRoomPriceSave_Click(object sender, RoutedEventArgs e)
        {
            if (ManageRoomPriceCombo.SelectedIndex == -1 || ManageRoomPriceTbx.Text == "")
                return;
            facade.UpdateRoomPrice((RoomType)ManageRoomPriceCombo.SelectedIndex, double.Parse(ManageRoomPriceTbx.Text.ToString()));
        }
        //private void ManageRoomPriceCombo_Changed(object sender, RoutedEventArgs e)
        private void ManageRoomPriceCombo_Changed(object sender, SelectionChangedEventArgs e)
        {
            IRoomPrice roomprice = facade.GetRoomPrice((RoomType)ManageRoomPriceCombo.SelectedItem);
            if (roomprice != null)
                ManageRoomPriceTbx.Text = roomprice.Price.ToString();
        }

        private void LogLoaded(object sender, RoutedEventArgs e)
        {
            LogDataGrid.ItemsSource = facade.GetLogs();
        }
        private void LogSearch_Click(object sender, RoutedEventArgs e)
        {
            LogDataGrid.ItemsSource = facade.GetLogs(LogStartDay.SelectedDate, LogEndDay.SelectedDate);
        }
        #endregion

        private void LogBackup_Click(object sender, RoutedEventArgs e)
        {
            string sourcePath = Environment.CurrentDirectory + "\\HotelDataBase.db";
            string bkFileName = "\\" + string.Format("{0:yyyyMMddHHmm}_", IClock.Time) + "HotelDataBase.db.bak";
            string destPath = Environment.CurrentDirectory + bkFileName;
            System.IO.File.Copy(sourcePath, destPath, true);
            MessageBox.Show("文件：" + destPath, "备份成功");
        }

        private void LogLoadBackup_Click(object sender, RoutedEventArgs e)
        {
            string sourcePath = selectFile();
            if (sourcePath != null)
            {
                string destPath = Environment.CurrentDirectory + "\\HotelDataBase.db";
                System.IO.File.Copy(sourcePath, destPath, true);
                MessageBox.Show("文件：" + sourcePath, "加载备份成功");
            }
        }

        private string selectFile()
        {
            string file = null;
            System.Windows.Forms.OpenFileDialog fileDialog = new System.Windows.Forms.OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Title = "请选择备份文件";
            fileDialog.InitialDirectory = Environment.CurrentDirectory;
            fileDialog.Filter = "备份文件(*.bak)|*.bak";
            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                file = fileDialog.FileName;
            }
            return file;
        }
        
    }
}
