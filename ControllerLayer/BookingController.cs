using System;
using System.Collections.Generic;
using System.Linq;
using Interface;
using Model;

namespace ControllerLayer
{
    /// <summary>
    /// Made by strongman1995
    /// </summary>
    internal class BookingController
    {
        private SQLiteController dbCon;
        private HotelController hCon;
        private LogController LCon;

        internal BookingController(SQLiteController db,HotelController h, LogController l)
        {
            dbCon = db;
            hCon = h;
            LCon = l;
        }
        
        #region Customer 

        internal ICustomer CreateCustomer(string name, CustomerGender gender, int age, string phone, string fax, string idcard,string roomid, string company, string address)
        {
            var customer = new Customer();
            return dbCon.CreateCustomer(customer.ID, name, gender, age,phone, fax, idcard, roomid, company, address);
        }

        internal ICustomer GetCustomer(string customerID)
        {
            return dbCon.GetCustomer(customerID);
        }

        internal List<ICustomer> GetCustomers()
        {
            return dbCon.GetCustomers();
        }

        /// <summary>
        /// 通过电话擦找联系人
        /// </summary>
        internal ICustomer GetCustomerViaPhone(string customerPhone)
        { 
            var list = dbCon.GetCustomers(); 
            foreach (ICustomer customer in list) 
            { 
                if (customer.Phone == customerPhone) 
                    return customer; 
            } 
            return null; 
        }
        
        internal ICustomer UpdateCustomer(ICustomer cus)
        {
            return dbCon.UpdateCustomer(cus);
        }
        #endregion 

        #region Booking

        internal List<IBooking> CreateBookings(List<IAvaliableRoom> selectedRoomList, DateTime start, DateTime end, string reservetime, string contractid, string reservationid)
        {
            List<IBooking> bookinglist = new List<IBooking>();
            foreach (IAvaliableRoom room in selectedRoomList)
            {
                IRoomPrice roomprice = dbCon.GetRoomPrice(room.RType);
                for (int i = 1; i <= room.ChosenNum; i++)
                {
                    var booking = new Booking();
                    dbCon.UpdateClock();
                    bookinglist.Add(dbCon.CreateBooking(booking.ID, start, end, reservetime, contractid, room.RType, roomprice.Price, reservationid));
                }
            }
            LCon.Log_Booked(dbCon.GetCustomer(contractid), bookinglist);
            return bookinglist;
        }

        internal IBooking CreateBooking(DateTime start, DateTime end, string reservetime, string contractid, RoomType roomtype, string reservationid)
        {
            IRoomPrice roomprice = dbCon.GetRoomPrice(roomtype);
            var booking = new Booking();
            dbCon.UpdateClock();
            return dbCon.CreateBooking(booking.ID, start, end, reservetime, contractid, roomtype, roomprice.Price , reservationid);
        }
        
        internal List<IBooking> GetActiveBookings(string reservationID)
        {
            List<IBooking> bookings = new List<IBooking>();

            foreach (IBooking book in dbCon.GetBookings(reservationID))
            {
                if (book.BStatus != BookStatus.Canceled)
                    bookings.Add(book);
            }
            return bookings;
        }

        internal List<IBooking> GetActiveBookings()
        {
            List<IBooking> bookings = new List<IBooking>();

            foreach (IBooking book in dbCon.GetBookings())
            {
                if (book.BStatus != BookStatus.Canceled)
                    bookings.Add(book);
            }
            return bookings;
        }

        /// <summary>
        /// 取消单个booking
        /// </summary>
        internal void CancelBooking(string BookingID)
        {
            IBooking book = dbCon.GetBooking(BookingID);
            book.BStatus = BookStatus.Canceled;
            dbCon.UpdateBooking(book);
            //如果reservation中只有一个booking，当booking取消时，reservation也取消
            string reservationID = book.ReservationID;
            List<IBooking> books = dbCon.GetBookings(reservationID);
            if (books.Count() == 1)
                CancelReservation(reservationID);
        }
        
        #endregion

        #region Reservation
        /// <summary>
        /// 一个reservation订单里包含多个booking
        /// </summary>
        /// <param name="books"></param>
        /// <returns>新建的reservation单</returns>
        internal IReservation CreateReservation()
        {
            IReservation reservation = new Reservation();
            dbCon.UpdateClock();
            return dbCon.CreateReservation(reservation.ID, 0, 0, ReservationStatus.Canceled);
        }

        internal IReservation UpdateReservation(IReservation reserv)
        {
            return dbCon.UpdateReservation(reserv);
        }

        internal IReservation GetReservation(string reservationID)
        {
            return dbCon.GetReservation(reservationID);
        }
        
        /// <summary>
        /// 取消reservationID的单，并且取消属于它的所有booking单
        /// </summary>
        internal void CancelReservation(string reservationID)
        {
            IReservation reservation = dbCon.GetReservation(reservationID);
            reservation.RStatus = ReservationStatus.Canceled;
            dbCon.UpdateReservation(reservation);
            List<IBooking> books = dbCon.GetBookings(reservationID);//与数据库的函数不同含义
            foreach (IBooking book in books)
            {
                book.BStatus = BookStatus.Canceled;
                dbCon.UpdateBooking(book);
            }

        }
        #endregion
    }
}
