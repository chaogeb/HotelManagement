using System;

namespace Interface
{
    /// <summary>
    /// Public static class of environment status.
    /// Made by chaogeb
    /// </summary>
    public static class IClock
    {
        public static DateTime Time         { get; set; }   // present date & time
        private static int CountRoom        { get; set; }   // counter of rooms
        private static int CountCustomer    { get; set; }   // counter of customers
        private static int CountBooking     { get; set; }   // counter of bookings
        private static int CountReserv      { get; set; }   // counter of reservations

        static IClock()
        {
            Time = DateTime.Now;
            CountRoom = 0;
            CountCustomer = 0;
            CountBooking = 0;
            CountReserv = 0;
        }
        
        public static string GetReservCount
        {
            get
            {
                string id = Time.ToLongDateString() + CountReserv;
                CountReserv++;
                return id;
            }
        }
        public static string GetBookingCount
        {
            get
            {
                string id = Time.ToLongDateString() + CountBooking;
                CountBooking++;
                return id;
            }
        }
        public static string GetCustomerCount
        {
            get
            {
                string id = Time.ToLongDateString() + CountCustomer;
                CountCustomer++;
                return id;
            }
        }
        public static string GetRoomCount
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
