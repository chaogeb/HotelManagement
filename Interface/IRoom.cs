using System;

namespace Interface 
{
    // Made by chaogeb
    public interface IRoom
    {
        string ID           { get; }
        string RoomNum      { get; set; }
        RoomType RType      { get; set; }
        RoomStatus RStatus  { get; set; }
    }

    // Made by chaogeb
    public interface IRoomPrice
    {
        RoomType RType      { get; set; }
        double Price        { get; set; }
    }
}
