using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interface;
using Model;

//made by 廖开翔
namespace ControllerLayer
{
    internal class BookingController
    {
        private SQLiteController dbCon;
        private HotelController hCon;
        internal BookingController(SQLiteController db,HotelController h)
        {
            dbCon = db;
            hCon = h;
        }
        
        #region Customer 
        internal ICustomer CreateCustomer(string id, string name, CustomerGender gender, int age, string phone, string fax, string idcard,string roomid, string company, string address)
        {
            return dbCon.CreateCustomer(id,name, gender, age,phone, fax, idcard, roomid, company, address);
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
        /// 在checkin时给顾客分配房间RoomID
        /// </summary>
        /// <param name="cus"></param>
        internal void CheckInCustomer(ICustomer cus,string RoomID)
        {
            cus.RoomID = RoomID;
            dbCon.UpdateCustomer(cus);
            /*
            foreach (IBooking book in GetActiveBookings(cus.ID))
            {
                List<IRoom> rooms = hCon.GetAvailableRooms(book.Roomtype, book.StartDate, book.EndDate);
                IRoom room = rooms.Fine();
                room.RStatus = RoomStatus.Occupied;
                dbCon.UpdateRoom(room);
            }*/
        }
        internal ICustomer UpdateCustomer(ICustomer cus)
        {
            return dbCon.UpdateCustomer(cus);
        }
        #endregion 

        #region Booking
        internal IBooking CreateBooking(DateTime start, DateTime end, string reservetime, string contractid, RoomType roomtype)
        {
            IBooking book = new Booking();
            IRoomPrice roomprice = dbCon.GetRprice(roomtype);
            return dbCon.CreateBooking(book.ID, start, end, reservetime, contractid, roomtype,roomprice.Price , "000",null);
        }
        
        internal List<IBooking> GetActiveBookings(string reservationID)
        {
            List<IBooking> bookings = new List<IBooking>();

            foreach (IBooking book in dbCon.GetBookings(reservationID))//与数据库的这个函数含义不同
            {
                if (book.BStatus == BookStatus.Confirmed)
                    bookings.Add(book);
            }
            return bookings;
        }
        /// <summary>
        /// 入住，分配房间给每个booking,并且把该房间状态改变
        /// </summary>
        /// <param name="BookingID"></param>
        /// <param name="RoomID"></param>
        internal void CheckInBooking(string BookingID, string RoomID)
        {
            IBooking book = dbCon.GetBooking(BookingID);
            book.RoomID = RoomID;
            dbCon.UpdateBooking(book);
            IRoom room = dbCon.GetRoom(RoomID);
            hCon.CheckInRoom(room);
        }
        /// <summary>
        /// 在reservationsID订单号中的booking单中的房间全部设为N/A,然后创建一个新ID的房间
        /// </summary>
        /// <param name="reservationID"></param>
        internal void CheckOutBooking(string reservationID)
        {
            List<IBooking> bookings = GetActiveBookings(reservationID);
            foreach (IBooking book in bookings)
            {
                IRoom room = dbCon.GetRoom(book.RoomID);
                room.RStatus = RoomStatus.NA;
                dbCon.UpdateRoom(room);
                hCon.CreateRoom(room.RoomNum, room.RType);
            }
        }
        /// <summary>
        /// 取消单个booking
        /// </summary>
        /// <param name="BookingID"></param>
        internal void CancelBooking(string BookingID)
        {
            IBooking book = dbCon.GetBooking(BookingID);
            book.BStatus = BookStatus.Canceled;
            dbCon.UpdateBooking(book);
            //如果reservation中只有一个booking，当booking取消时，reservation也取消
            string reservationID = book.ReservationID;
            List<IBooking> books = dbCon.GetBooking(reservationID);
            int count=books.Count();
            if (count == 1)
                CancelReservation(reservationID);
        }

        internal void Timeout(string BookingID)
        {
            IBooking book = dbCon.GetBooking(BookingID);
            book.BStatus = BookStatus.Timeout;
            dbCon .UpdateBooking(book);
        }

        internal bool is_Timeout(string BookingID)
        {
            IBooking book=dbCon.GetBooking(BookingID);
            if(book.BStatus == BookStatus.Confirmed && IClock.Time > book.StartDate)
                return true;
            return false;
        }

        #endregion

        #region Reservation
        /// <summary>
        /// 一个reservation订单里包含多个booking
        /// </summary>
        /// <param name="books"></param>
        /// <returns>新建的reservation单</returns>
        internal IReservation CreateReservation(List<IBooking> books)
        {
            IReservation reservation = new Reservation();
            string reservationID=reservation.ID;
            double payment = 0;
            //将生成的reservationID赋给属于它的每一个booking
            foreach (IBooking book in books)
            {
                book.ReservationID = reservationID;
                IRoomPrice roomprice=dbCon.GetRprice(book.Roomtype);
                payment += roomprice.Price;
                dbCon.UpdateBooking(book);
            }
            return dbCon.CreateReservation(reservationID, payment, downpayment, ReservationStatus.Booked);//定金一个固定值
        }

        internal IReservation GetReservation(string reservationID)
        {
            return dbCon.GetReservation(reservationID);
        }

        internal List<IReservation> GetReservations()
        {
            return dbCon.GetReservations();
        }

        internal void CheckOutReservation(string reservationID)
        {
            IReservation reservation = dbCon.GetReservation(reservationID);
            reservation.RStatus = ReservationStatus.Paid;
            dbCon.UpdateReservation(reservation);
        }
        /// <summary>
        /// 取消reservationID的单，并且取消属于它的所有booking单
        /// </summary>
        /// <param name="reservationID"></param>
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
        internal void DefaultReservation(string reservationID)
        {
            IReservation reservation = dbCon.GetReservation(reservationID);
            reservation.RStatus = ReservationStatus.Default;
            dbCon.UpdateReservation(reservation);
        }
        #endregion
    }
}
