using System;

namespace Interface
{
    // Made by chaogeb
    public interface IClock
    {
        DateTime Time           { get; set; }   // present date & time
        string GetReservCount   { get; }        // get reservations id
        string GetBookingCount  { get; }        // get bookings id
        string GetCustomerCount { get; }        // get customers id
        string GetRoomCount     { get; }        // get rooms id
    }
}
