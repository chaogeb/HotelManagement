using System;

namespace Interface 
{
    // Made by chaogeb
    public interface IRoom
    {
        int ID { get; }

        int HotelID { get; }

        int RoomNum { get; set; }

        double Price { get; set; }

        RoomType RType { get; set; }

        RoomStatus RStatus { get; set; }
    }
}
