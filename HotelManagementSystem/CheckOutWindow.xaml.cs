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

namespace HotelManagementSystem
{
    /// <summary>
    /// CheckOutWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CheckOutWindow : Window
    {
        FacadeController facade = FacadeController.GetInstance();
        IReservation reservation;
        IBooking booking;
        ICustomer contract;
        IRoom room;
        List<IBooking> bookinglist = new List<IBooking>();
        List<IRoom> roomlist = new List<IRoom>();

        public CheckOutWindow(IRoom rm)
        {
            room = rm;
            var book = facade.GetActiveBookings(null);
            foreach (IBooking bktemp in book)
            {
                if (bktemp.RoomID == room.ID)
                {
                    booking = bktemp;
                    break;
                }
            }
            InitializeComponent();
            InitializeWindowContent();
        }
        public CheckOutWindow(string bookingid)
        {
            booking = facade.GetBooking(bookingid);
            room = facade.GetRoom(booking.RoomID);
            InitializeComponent();
            InitializeWindowContent();
        }

        private void InitializeWindowContent()
        {
            reservation = facade.GetReservation(booking.ReservationID);
            contract = facade.GetCustomer(booking.ContractID);
            
            reservationNumber.Content = reservation.ID;
            checkInDate.Content = booking.StartDate.ToLongDateString().ToString();
            checkOutDate.Content = booking.EndDate.ToLongDateString().ToString();
            contracts.Content = contract.Name.ToString();
            roomType.Content = booking.Roomtype.ToString();
            contractsNum.Content = contract.Phone;
            company.Content = contract.Company;
            address.Text = contract.Address;

            roomPrice.Content = booking.ThisPrice;
            checkOutTime.Content = IClock.Time.GetDateTimeFormats('f')[0].ToString();
            downPayment.Content = reservation.DownPayment;
        }

        private void LoadRoomList()
        {
            bookinglist = facade.GetActiveBookings(reservation.ID);
            foreach (IBooking bk in bookinglist)
            {
                roomlist.Add(facade.GetRoom(bk.RoomID));
            }
            //checkOutRoomsList.Children.Add()
        }
        private static UIElement NewCheckBox(string name, string content)
        {
            var cbx = new CheckBox();
            cbx.Name = name;
            cbx.Content = content;
            cbx.Margin = new Thickness(5, 0, 5, 0);
            return cbx;
        }

        private void CheckOutConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
