﻿using System;
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

        internal bool DeleteBooking(ICustomer cus, string bookingID)
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

        internal List<IBooking> GetActiveBookings(string customerID)
        {
            List<IBooking> bookings = new List<IBooking>();

            foreach (IBooking book in dbCon.GetBookings(customerID))
            {
                if (book.BStatus == BookStatus.Confirmed )
                    bookings.Add(book);
            }
            return bookings;
        }

        internal ICustomer CreateCustomer(string id, string name, CustomerGender gender, int age, string phone, string fax, string idcard, string roomid, string company, string address, string city)
        {
            return dbCon.CreateCustomer(id,name, gender,age, phone,fax, idcard,roomid, company,address, city);
        }

        internal ICustomer GetCustomer(string customerID)
        {
            return dbCon.GetCustomer(customerID);
        }

        internal bool DeleteCustomer(string customerID)
        {
            return dbCon.DeleteCustomer(customerID);
        }

        internal ICustomer UpdateCustomer(ICustomer cus)
        {
            return dbCon.UpdateCustomer(cus);
        }

        internal IBooking CreateBooking(string id, DateTime start, DateTime end, string reservetime, string contractid, RoomType roomtype, string roomid, string reservationid)
        {
            IBooking booking = dbCon.CreateBooking(id, start, end, reservetime, contractid, roomtype, roomid,reservationid);
            return booking;
        }

        internal List<ICustomer> GetCustomers()
        {
            return dbCon.GetCustomers();
        }
    }
}
