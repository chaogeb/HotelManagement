using System;

namespace Interface
{
    // Made by chaogeb
    public interface IBooking
    {
        int ID { get; }

        DateTime StartDate { get; set; }

        DateTime EndDate { get; set; }

        double Price { get; set; }

        string Comment { get; set; }

        BookingType BType { get; set; }

        BookStatus BStatus { get; set; }

        int RoomID { get; set; }
    }
}
