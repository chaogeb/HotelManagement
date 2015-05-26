using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interface;

namespace Model
{
    //made by 廖开翔
    public class Booking
    {
        public int ID { get; private set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Price { get; set; }
        public string Comment { get; set; }
        public int RoomID { get; set; }
        public BookingType BType { get; set; }
        public BookStatus BStatus { get; set; }
        public Booking(int id, DateTime start, DateTime end, double price,
                       string comment, BookingType type, int roomId)
        {
            ID = id;
            StartDate = start;
            EndDate = end;
            Price = price;
            Comment = comment;
            BType = type;
            RoomID = roomId;
            BStatus = BookStatus.Confirmed;
        }
    }
}
