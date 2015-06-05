using System;

namespace Interface 
{
    // Made by chaogeb
    public interface IRoom
    {
        string ID { get; }
        string RoomNum { get; set; }
        double Price { get; set; }
        RoomType RType { get; set; }
        RoomStatus RStatus { get; set; }
    }
}
