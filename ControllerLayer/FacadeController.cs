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

        public bool DeleteBooking(ICustomer cus, string bookingID)
        {
            return customerCon.DeleteBooking(cus, bookingID);
        }

        public IBooking CreateBooking(string id, DateTime start, DateTime end, string reservetime, string contractid, RoomType roomtype,double thisprice, string roomid, string reservationid)
        {
            return customerCon.CreateBooking(id, start, end, reservetime, contractid, roomtype,thisprice, roomid,reservationid);
        }

        public List<IBooking> GetActiveBookings(string customerID)
        {
            return customerCon.GetActiveBookings(customerID);
        }
        #endregion

        #region Customer Methods

        public void CheckInCustomer(ICustomer customer)
        {
            customerCon.CheckInCustomer(customer);
        }

        public ICustomer CreateCustomer(string id, string name, CustomerGender gender, int age, string phone, string fax, string idcard, string roomid, string company, string address)
        {
            return customerCon.CreateCustomer(id ,name, gender,age, phone,fax, idcard, roomid, company,address);
        }

        /// <summary>
        /// Made by: Andreana
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public ICustomer GetCustomer(string customerID)
        {
            return customerCon.GetCustomer(customerID);
        }

        public List<ICustomer> GetCustomers()
        {
            return customerCon.GetCustomers();
        }

        public bool DeleteCustomer(string customerID)
        {
            return customerCon.DeleteCustomer(customerID);
        }

        public ICustomer UpdateCustomer(ICustomer cus)
        {
            return customerCon.UpdateCustomer(cus);
        }

        #endregion

        #region Room Methods

        public IRoom CreateRoom(string id, string roomNum, double price, RoomType rType, RoomStatus rStatus)
        {
            return hotelCon.CreateRoom(id, roomNum, price, rType, rStatus);
        }

        public IRoom GetRoom(string roomID)
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
