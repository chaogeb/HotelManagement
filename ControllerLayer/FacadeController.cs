using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Interface;

namespace ControllerLayer
{
    public class FacadeController
    {
        private SQLiteController dbCon;
        private HotelController hotelCon;
        private BookingController customerCon;
        private LogController logCon;

        private static FacadeController instance;
        
        private FacadeController()
        {
            dbCon = new SQLiteController();
            hotelCon = new HotelController(dbCon);
            logCon = new LogController(dbCon);
            customerCon = new BookingController(dbCon, hotelCon, logCon);
        }
        
        public static FacadeController GetInstance()
        {
            if (instance == null)
            {
                instance = new FacadeController();
            }
            return instance;
        }
        
        public void TimeLine()
        {
            List<IBooking> booking = customerCon.GetActiveBookings();
            foreach (IBooking bk in booking)
            {
                if (bk.BStatus == BookStatus.Confirmed)
                {
                    if (bk.RoomID != "")
                    {   // room id exists
                        IRoom rm = hotelCon.GetRoom(bk.RoomID);
                        if (rm.RStatus != RoomStatus.NA)
                        if (bk.EndDate.Date == IClock.Time.Date && IClock.Time.Hour >= 12)
                        {
                            MessageBox.Show("房间" + hotelCon.GetRoom(bk.RoomID).RoomNum
                                + "旅客已超出离店时间", "离店提醒");
                        }
                    }
                    else if (bk.StartDate.Date == IClock.Time.Date 
                        && String.CompareOrdinal(string.Format("{0:HHmm}", IClock.Time), bk.ReserveTime) >= 0)
                    {   // room id not exists && check in is today
                        bk.BStatus = BookStatus.Timeout;
                        var cus = customerCon.GetCustomer(bk.ContractID);
                        MessageBox.Show("旅客" + cus.Name + "超过预订时间未入住\n电话：" + cus.Phone, "订单超时");
                    }
                }
                else if (bk.BStatus == BookStatus.Timeout)
                {
                    if (bk.StartDate.Date < IClock.Time.Date)
                    {
                        var cus = customerCon.GetCustomer(bk.ContractID);
                        MessageBox.Show("旅客" + cus.Name + "超过预订日期未入住\n电话：" + cus.Phone, "订单超时");
                    }
                }
            }
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
        
        public IBooking UpdateBooking(IBooking book)
        {
            return dbCon.UpdateBooking(book);
        }

        public IBooking GetBooking(string bookingID)
        {
            return dbCon.GetBooking(bookingID);
        }
        /// <summary>
        /// 返回非取消状态的订单列表
        /// 标记 reservationID 为 null 时不标记
        /// </summary>
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

        public void CancelBooking(string BookingID)
        {
            customerCon.CancelBooking(BookingID);
        }
        #endregion

        #region Customer Methods
        
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
            IRoom rm = hotelCon.GetRoomViaNum(roomNum);
            if (rm != null)
                list.Add(rm);
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

        /// <summary>
        /// 获取指定房间类型的价格
        /// </summary>
        public IRoomPrice GetRoomPrice(RoomType roomtype)
        {
            return hotelCon.GetRoomPrice(roomtype);
        }

        /// <summary>
        /// 获取指定房间号的价格
        /// </summary>
        public IRoomPrice GetRoomPrice(string roomNum)
        {
            return hotelCon.GetRoomPrice(roomNum);
        }
        public List<IRoom> RefreshRooms(List<IRoom> roomList)
        {
            return hotelCon.RefreshRooms(roomList);
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

        #region Log Method

        public void Log_CheckIn(List<ICustomer> customers, IBooking booking)
        {
            logCon.Log_CheckIn(customers, booking);
        }

        public void Log_CheckOut(IBooking booking)
        {
            logCon.Log_CheckOut(booking);
        }

        public void Log_Cancel(IBooking booking)
        {
            logCon.Log_Cancel(booking);
        }

        public List<Log> GetLogs()
        {
            return logCon.GetLogs();
        }

        public List<Log> GetLogs(DateTime? start, DateTime? end)
        {
            var chosenlogs = new List<Log>();
            var logs = logCon.GetLogs();
            foreach (Log lg in logs)
            {
                if (start != null)
                {
                    if (end != null)
                    { // start && end
                        if (lg.Time >= start && lg.Time < ((DateTime)end).AddDays(1))
                            chosenlogs.Add(lg);
                    }
                    else // start && !end
                    if (lg.Time >= start)
                        chosenlogs.Add(lg);
                }
                else if (end != null)
                {   // !start && end
                    if (lg.Time < ((DateTime)end).AddDays(1))
                        chosenlogs.Add(lg);
                }
                else // !start && !end
                    chosenlogs.Add(lg);
            }
            return chosenlogs;
        }
        #endregion
    }
}
