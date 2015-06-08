using System;
using System.Collections.Generic;
using Interface;

namespace ControllerLayer
{
    public class FacadeController
    {
        private SQLiteController dbCon;
        private HotelController hotelCon;
        private CustomerController customerCon;

        private static FacadeController instance;

        //private FacadeController(string user, string pass)
        private FacadeController()
        {
            dbCon = new SQLiteController();
            hotelCon = new HotelController(dbCon);
            customerCon = new CustomerController(dbCon);
        }

        //public static FacadeController GetInstance(string user, string pass)
        public static FacadeController GetInstance()
        {
            if (instance == null)
            {
                //instance = new FacadeController(user, pass);
                instance = new FacadeController();
            }
            return instance;
        }
        
        public bool Authenticated()
        {
            return dbCon.Authenticated();
        }

        #region Booking Methods

        //public bool DeleteBooking(ICustomer cus, string bookingID)
        //{
        //    return customerCon.DeleteBooking(cus, bookingID);
        //}

        public IBooking CreateBooking(DateTime start, DateTime end, string reservetime, string contractid, RoomType roomtype, double thisprice, string reservationid)
        {
            return customerCon.CreateBooking(IClock.GetBookingID, start, end, reservetime, contractid, roomtype, thisprice, reservationid);
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
        
        public ICustomer GetCustomer(string customerID)
        {
            return customerCon.GetCustomer(customerID);
        }

        public ICustomer GetCustomerViaPhone(string customerPhone)
        {
            return customerCon.GetCustomerViaPhone(customerPhone);
        }

        public List<ICustomer> GetCustomers()
        {
            return customerCon.GetCustomers();
        }
        
        public ICustomer UpdateCustomer(ICustomer cus)
        {
            return customerCon.UpdateCustomer(cus);
        }

        #endregion

        #region Room Methods

        public IRoom CreateRoom(string id, string roomNum, RoomType rType, RoomStatus rStatus)
        {
            return hotelCon.CreateRoom(id, roomNum, rType, rStatus);
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

        #region Clock Method
        public void GetClock()
        {
            if (!dbCon.GetClock())
                dbCon.CreateClock();
        }
        public void SetClock()
        {
            dbCon.UpdateClock();
        }
        #endregion
    }
}
