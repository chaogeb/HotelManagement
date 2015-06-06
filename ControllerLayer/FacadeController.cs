using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interface;

namespace ControllerLayer
{
    class FacadeController
    {
        private SQLiteController dbCon;
        private HotelController hotelCon;
        private CustomerController customerCon;

        private static FacadeController instance;
        private FacadeController(string user, string pass)
        {
            //dbCon = new SQLiteController(user, pass);
            hotelCon = new HotelController(dbCon);
            customerCon = new CustomerController(dbCon);
        }

        public static FacadeController GetInstance(string user, string pass)
        {
            if (instance == null)
            {
                instance = new FacadeController(user, pass);
            }
            return instance;
        }

        public static FacadeController GetInstance()
        {
            if (instance == null)
                throw new Exception("Not authenticated!");
            return instance;
        }

        public bool Authenticated()
        {
            return dbCon.Authenticated();
        }

        #region Booking Methods

        public bool DeleteBooking(ICustomer cus, int bookingID)
        {
            return customerCon.DeleteBooking(cus, bookingID);
        }

        public IBooking CreateBooking(ICustomer customer, DateTime start, DateTime end, double price, string comment, BookingType type, long roomId)
        {
            return customerCon.CreateBooking(customer, start, end, price, comment, type, roomId);
        }

        public List<IBooking> GetActiveBookings(int customerID)
        {
            return customerCon.GetActiveBookings(customerID);
        }
        #endregion

        #region Customer Methods

        public void CheckInCustomer(ICustomer customer)
        {
            customerCon.CheckInCustomer(customer);
        }

        public ICustomer CreateCustomer(string name, string gender, string phone, int id,
                        string company, string city, CustomerStatus status)
        {
            return customerCon.CreateCustomer(name, gender, phone, id, company, city, status);
        }

        /// <summary>
        /// Made by: Andreana
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public ICustomer GetCustomer(int customerID)
        {
            return customerCon.GetCustomer(customerID);
        }

        public List<ICustomer> GetCustomers()
        {
            return customerCon.GetCustomers();
        }

        public bool DeleteCustomer(int customerID)
        {
            return customerCon.DeleteCustomer(customerID);
        }

        public ICustomer UpdateCustomer(ICustomer cus)
        {
            return customerCon.UpdateCustomer(cus);
        }

        #endregion

        #region Room Methods

        public IRoom CreateRoom(int hotelID, int roomNum, double price, RoomType rType, RoomStatus rStatus)
        {
            return hotelCon.CreateRoom(hotelID, roomNum, price, rType, rStatus);
        }

        public IRoom GetRoom(int roomID)
        {
            return hotelCon.GetRoom(roomID);
        }

        public List<IRoom> GetRooms()
        {
            return hotelCon.GetRooms();
        }

        public IRoom UpdateRoom(IRoom room)
        {
            return hotelCon.UpdateRoom(room);
        }

        public List<IRoom> GetAvailableRooms(RoomType? roomtype, DateTime? startdate, DateTime? enddate)
        {
            return hotelCon.GetAvailableRooms(roomtype, startdate, enddate);
        }
        #endregion
    }
}
