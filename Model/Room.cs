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
        public string ID { get; private set; }
        public string RoomNum { get; set; }
        public double Price { get; set; }
        public RoomType RType { get; set; }
        public RoomStatus RStatus { get; set; }
        /// <summary>
        /// 根据数据库提供的数据，初始化旅馆的状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="roomNum"></param>
        /// <param name="price"></param>
        /// <param name="type"></param>
        /// <param name="status"></param>
        public Room(string id, string roomNum, double price, RoomType type, RoomStatus status)
        {
            ID = id;
            RoomNum = roomNum;
            Price = price;
            RType = type;
            RStatus = status;
        }
        /// <summary>
        /// 根据处在N/A状态的失效房间，创建一个新房间ID，房间状态为空闲，房间其他信息和原来一样
        /// </summary>
        /// <param name="id"></param>
        public Room(Room id)
        {
            ID = IClock.GetRoomCount;
            RoomNum = id.RoomNum;
            RType = id.RType;
            RStatus =  RoomStatus.Idle;
        }
    }
}
