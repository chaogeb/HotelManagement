using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interface;
using Model;

//made by 廖开翔
namespace ControllerLayer
{
    internal class CustomerController
    {
        private SQLiteController dbCon;
        internal CustomerController(SQLiteController db)
        {
            dbCon = db;
        }

        internal bool DeleteBooking(ICustomer cus, int bookingID)
        {
            return dbCon.DeleteBooking(bookingID);
        }

        internal void CheckInCustomer(ICustomer cus)
        {
            dbCon.UpdateCustomer(cus);

            foreach (IBooking book in GetActiveBookings(cus.ID))
            {
                IRoom room = dbCon.GetRoom(book.RoomID);
                room.RStatus = RoomStatus.Occupied;
                dbCon.UpdateRoom(room);
            }
        }

        internal List<IBooking> GetActiveBookings(int customerID)
        {
            List<IBooking> bookings = new List<IBooking>();

            foreach (IBooking book in dbCon.GetBookings(customerID))
            {
                if (book.BStatus == BookStatus.Confirmed || book.BStatus == BookStatus.Paid)
                    bookings.Add(book);
            }
            return bookings;
        }

        internal ICustomer CreateCustomer(string name, string gender, string phone, int id, string company, string city)
        {
            return dbCon.CreateCustomer(name, gender, phone, id, company, city, status);
        }

        internal ICustomer GetCustomer(int customerID)
        {
            return dbCon.GetCustomer(customerID);
        }

        internal bool DeleteCustomer(int customerID)
        {
            return dbCon.DeleteCustomer(customerID);
        }

        internal ICustomer UpdateCustomer(ICustomer cus)
        {
            return dbCon.UpdateCustomer(cus);
        }

        internal IBooking CreateBooking(ICustomer customer, DateTime start, DateTime end, double price, string comment, BookingType type, long roomId)
        {
            IBooking booking = dbCon.CreateBooking(customer, start, end, price, comment, type, roomId);
            return booking;
        }

        internal List<ICustomer> GetCustomers()
        {
            return dbCon.GetCustomers();
        }
    }
}
