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
        public static int CountRoom         { get; private set; }   // counter of rooms
        public static int CountCustomer     { get; private set; }   // counter of customers
        public static int CountBooking      { get; private set; }   // counter of bookings
        public static int CountReserv       { get; private set; }   // counter of reservations

        static IClock()
        {
            Time = DateTime.Now;
            CountRoom = 0;
            CountCustomer = 0;
            CountBooking = 0;
            CountReserv = 0;
        }

        public static void SetClock(DateTime time, int croom, int ccustomer, int cbooking, int creserv)
        {
            Time = time;
            CountRoom = croom;
            CountCustomer = ccustomer;
            CountBooking = cbooking;
            CountReserv = creserv;
        }
        public static void RunClock()
        {
            int day = Time.Day;
            Time = Time.AddMinutes(20);
            if (Time.Day != day)
            {
                CountRoom = 0;
                CountCustomer = 0;
                CountBooking = 0;
                CountReserv = 0;
            }
        }

        public static string GetTime
        {
            get
            {
                return Time.GetDateTimeFormats('f')[0].ToString();
            }
        }
        public static string GetReservID
        {
            get
            {
                string id = string.Format("{0:yyyyMMdd}", Time) + CountReserv;
                CountReserv++;
                return id;
            }
        }
        public static string GetBookingID
        {
            get
            {
                string id = string.Format("{0:yyyyMMdd}", Time) + CountBooking;
                CountBooking++;
                return id;
            }
        }
        public static string GetCustomerID
        {
            get
            {
                string id = string.Format("{0:yyyyMMdd}", Time) + CountCustomer;
                CountCustomer++;
                return id;
            }
        }
        public static string GetRoomID
        {
            get
            {
                string id = string.Format("{0:yyyyMMdd}", Time) + CountRoom;
                CountRoom++;
                return id;
            }
        }
    }
}
