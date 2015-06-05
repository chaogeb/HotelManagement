using System;
using Interface;

namespace Model
{
    // Made by chaogeb
    class Clock : IClock
    {
        public DateTime Time        { get; set; }   // present date & time
        private int CountRoom       { get; set; }   // counter of rooms
        private int CountCustomer   { get; set; }   // counter of customers
        private int CountBooking    { get; set; }   // counter of bookings
        private int CountReserv     { get; set; }   // counter of reservations
        
        public Clock(DateTime time, int room = 0, int cust = 0, int booking = 0, int reserv = 0)
        {
            Time = time;
            CountRoom = room;
            CountCustomer = cust;
            CountBooking = booking;
            CountReserv = reserv;
        }

        public string GetReservCount
        {
            get
            {
                string id = Time.ToLongDateString() + CountReserv;
                CountReserv++;
                return id;
            }
        }
        public string GetBookingCount
        {
            get
            {
                string id = Time.ToLongDateString() + CountBooking;
                CountBooking++;
                return id;
            }
        }
        public string GetCustomerCount
        {
            get
            {
                string id = Time.ToLongDateString() + CountCustomer;
                CountCustomer++;
                return id;
            }
        }
        public string GetRoomCount
        {
            get
            {
                string id = Time.ToLongDateString() + CountRoom;
                CountRoom++;
                return id;
            }
        }
    }
}
