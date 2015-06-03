using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interface;

namespace Model
{
     //made by 廖开翔
     public class Room : IRoom
    {
        public int ID { get; private set; }
        public int RoomNum { get; set; }
        public double Price { get; set; }
        public RoomType RType { get; set; }
        public RoomStatus RStatus { get; set; }
        public Room(int id, int roomNum, double price, RoomType type, RoomStatus status)
        {
            ID = id;
            RoomNum = roomNum;
            Price = price;
            RType = type;
            RStatus = status;
        }
        public Room(int id)
        {
            ID = id;
        }
    }
}
