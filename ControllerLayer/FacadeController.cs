using System;
using System.Collections.Generic;
using Interface;

namespace ControllerLayer
{
    public class FacadeController
    {
        private SQLiteController dbCon;
        private HotelController hotelCon;
        private BookingController customerCon;

        private static FacadeController instance;

        //private FacadeController(string user, string pass)
        private FacadeController()
        {
            dbCon = new SQLiteController();
            hotelCon = new HotelController(dbCon);
            customerCon = new BookingController(dbCon, hotelCon);
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

        #region Reservation Methods

        public IReservation CreateReservation()
        {
            return customerCon.CreateReservation();
        }
        public IReservation GetReservation(string reservationID)
        {
            return customerCon.GetReservation(reservationID);
        }
        public IReservation ComfirmReservation(IReservation reserv, double downpayment)
        {
            reserv.RStatus = ReservationStatus.Booked;
            reserv.DownPayment = downpayment;
            return customerCon.UpdateReservation(reserv);
        }
        public IReservation UpdateReservation(IReservation reserv)
        {
            return customerCon.UpdateReservation(reserv);
        }

        #endregion

        #region Booking Methods

        public List<IBooking> CreateBookings(List<IAvaliableRoom> selectedRoomList, DateTime startday, DateTime endday, string reservetime, string contractid, string reservationid)
        {
            return customerCon.CreateBookings(selectedRoomList, startday, endday, reservetime, contractid, reservationid);
        }

        public IBooking CreateBooking(DateTime start, DateTime end, string reservetime, string contractid, RoomType roomtype, string reservationid)
        {
            var booking = customerCon.CreateBooking(start, end, reservetime, contractid, roomtype, reservationid);
            SetClock();
            return booking;
        }
        public IBooking GetBooking(string bookingID)
        {
            return dbCon.GetBooking(bookingID);
        }
        /// <summary>
        /// 返回非取消状态的订单列表
        /// 标记 reservationID 为 null 时不标记
        /// </summary>
        /// <param name="reservationID"></param>
        /// <returns></returns>
        public List<IBooking> GetActiveBookings(string reservationID)
        {
            if (reservationID != null)
                return customerCon.GetActiveBookings(reservationID);
            else
                return customerCon.GetActiveBookings();
        }
        public List<IBooking> GetActiveBookingsViaName(string name)
        {
            var list = customerCon.GetActiveBookings();
            var temp = new List<IBooking>();
            foreach (IBooking booking in list)
            {
                var customer = customerCon.GetCustomer(booking.ContractID);
                if (customer.Name == name)
                    temp.Add(booking);
            }
            return temp;
        }
        #endregion

        #region Customer Methods

        public void CheckInCustomer(ICustomer customer)
        {
            //customerCon.CheckInCustomer(customer);
        }

        public ICustomer CreateCustomer(string name, CustomerGender? gender, int? age, string phone, string fax, string idcard, string roomid, string company, string address)
        {
            if (gender == null) gender = CustomerGender.Male;
            if (age == null) age = 0;
            var customer = customerCon.CreateCustomer(name, (CustomerGender)gender, (int)age, phone,fax, idcard, roomid, company,address);
            SetClock();
            return customer;
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

        public IRoom CreateRoom(string roomNum, RoomType rType)
        {
            //var room = hotelCon.CreateRoom(roomNum, rType);
            //SetClock();
            //return room;
            return hotelCon.CreateRoom(roomNum, rType);
        }

        public IRoom GetRoom(string roomID)
        {
            return hotelCon.GetRoom(roomID);
        }

        public List<IRoom> GetRoomViaNum(string roomNum)
        {
            var list = new List<IRoom>();
            list.Add(hotelCon.GetRoomViaNum(roomNum));
            return list;
        }

        public List<IRoom> GetRooms()
        {
            return hotelCon.GetRooms();
        }

        public IRoom UpdateRoom(IRoom room)
        {
            return hotelCon.UpdateRoom(room);
        }

        public List<IAvaliableRoom> GetAvailableRooms(RoomType? roomtype, DateTime? startdate, DateTime? enddate)
        {
            return hotelCon.GetAvailableRooms(roomtype, startdate, enddate);
        }

        public IRoomPrice UpdateRoomPrice(RoomType roomtype, double price)
        {
            return hotelCon.UpdateRoomPrice(roomtype, price);
        }
        public IRoomPrice GetRoomPrice(RoomType roomtype)
        {
            return hotelCon.GetRoomPrice(roomtype);
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
