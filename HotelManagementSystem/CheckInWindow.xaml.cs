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
using ControllerLayer;
using Interface;
using System.Collections.ObjectModel;

namespace HotelManagementSystem
{
    internal class RoomItem
    {
        public string RoomID { get; set; }
        public string RoomNum { get; set; }
    }
    /// <summary>
    /// CheckInWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CheckInWindow : Window
    {
        FacadeController facade;
        IReservation reservation;
        IBooking booking;
        ICustomer contract;
        public CheckInWindow(string bookingid)
        {
            InitializeComponent();
            facade = FacadeController.GetInstance();
            LoadData(bookingid);
        }

        private void LoadData(string bookingid)
        {
            booking = facade.GetBooking(bookingid);
            reservation = facade.GetReservation(booking.ReservationID);
            contract = facade.GetCustomer(booking.ContractID);
            InitializeWindowContent();
        }

        private void InitializeWindowContent()
        {
            reservationNumber.Content = reservation.ID;
            checkInDate.Content = booking.StartDate.ToLongDateString().ToString();
            checkOutDate.Content = booking.EndDate.ToLongDateString().ToString();
            contracts.Content = contract.Name.ToString();
            roomType.Content = booking.Roomtype.ToString();
            roomPriceTbx.Content = booking.ThisPrice;
            downPayment.Content = reservation.DownPayment;

            CustomerGenderCbx.ItemsSource = Enum.GetValues(typeof(CustomerGender));
            lstTypes.ItemsSource = customerlist;
            List<IRoom> list = facade.GetRooms();
            foreach (IRoom room in list)
            {
                if (room.RType == booking.Roomtype && room.RStatus == RoomStatus.Idle)
                {
                    roomDict.Add(room.ID, room.RoomNum);
                }
            }
            roomNum.ItemsSource = roomDict;
            roomNum.SelectedValuePath = "Key";
            roomNum.DisplayMemberPath = "Value";
            roomNum.SelectedIndex = 0;
        }

        private void CheckInConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveCustomerDetails();
            foreach (ICustomer cus in customerlist)
            {
                cus.RoomID = roomNum.SelectedValue.ToString();
                facade.UpdateCustomer(cus);
            }
            IRoom room = facade.GetRoom(roomNum.SelectedValue.ToString());
            room.RStatus = RoomStatus.Occupied;
            facade.UpdateRoom(room);
            booking.RoomID = room.ID;
            this.Close();
        }

        private void CheckBoxContracts_Checked(object sender, RoutedEventArgs e)
        {
            customerlist.Add(contract);
            lstTypes.SelectedItem = contract;
        }
        private void CheckBoxContracts_Unchecked(object sender, RoutedEventArgs e)
        {
            customerlist.Remove(contract);
            lstTypes.SelectedItem = null;
        }

        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (lstTypes.SelectedItem != null)
                SaveCustomerDetails();
            var cus = facade.CreateCustomer("新旅客", null, 0, null, null, null, null, null, null);
            customerlist.Add(cus);
            lstTypes.SelectedItem = cus;
        }
        private void lstTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Pro WPF 4.5 in C#\Chapter17\ControlTemplateBrowser
            if (lstTypes.SelectedItem != null)
                LoadCustomerDetails();
        }

        private void SaveCustomerDetails()
        {
            ICustomer cus = lstTypes.SelectedItem as ICustomer;
            cus.Name = CustomerNameTbx.Text;
            cus.Gender = (CustomerGender)Enum.Parse(typeof(CustomerGender), CustomerGenderCbx.SelectedValue.ToString());
            cus.Age = int.Parse(CustomerAgeTbx.Text);
            cus.IDcard = CreditCardNoTbx.Text;
            lstTypes.SelectedItem = cus;
        }
        private void LoadCustomerDetails()
        {
            ICustomer cus = lstTypes.SelectedItem as ICustomer;
            CustomerNameTbx.Text = cus.Name;
            CustomerGenderCbx.SelectedValue = cus.Gender;
            CustomerAgeTbx.Text = cus.Age.ToString();
            CreditCardNoTbx.Text = cus.IDcard;
        }

        Dictionary<string, string> roomDict = new Dictionary<string, string>();
        ObservableCollection<ICustomer> customerlist = new ObservableCollection<ICustomer>();
    }
}
