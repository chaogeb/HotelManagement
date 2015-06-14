using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Interface;
using Model;

namespace ControllerLayer
{
    /// <summary>
    /// Made by strongman1995
    /// </summary>
    internal class HotelController
    {
        private SQLiteController dbCon;

        //Initializes a HotelController
        internal HotelController(SQLiteController db)
        {
            dbCon = db;
        }

        // Adds a room to the database.
        internal IRoom CreateRoom(string roomNum, RoomType rType)
        {
            var room = new Room();
            dbCon.UpdateClock();
            return dbCon.CreateRoom(room.ID, roomNum, rType, RoomStatus.Idle);
        }

        // Returns a IRoom from id.
        internal IRoom GetRoom(string id)
        {
            return dbCon.GetRoom(id);
        }

        internal IRoom GetRoomViaNum(string roomNum)
        {
            var list = dbCon.GetRooms();
            foreach (IRoom room in list)
            {
                if (room.RoomNum == roomNum && room.RStatus != RoomStatus.NA)
                    return room;
            }
            return null;
        }

        /// Returns a list of all the rooms (except NA).
        internal List<IRoom> GetRooms()
        {
            var allrooms = dbCon.GetRooms();
            List<IRoom> temp = new List<IRoom>();
            foreach (IRoom rooms in allrooms)
            {
                if (rooms.RStatus != RoomStatus.NA)
                {
                    temp.Add(rooms);
                }
            }
            return temp;
        }

        /// Updates a room
        /// Room to update based on ID
        internal IRoom UpdateRoom(IRoom room)
        {
            return dbCon.UpdateRoom(room);
        }
        
        //Return a list of available room types
        internal List<IAvaliableRoom> GetAvailableRooms(RoomType? roomtype, DateTime? startdate, DateTime? enddate)
        {
            List<IBooking> bookings = dbCon.GetBookings();
            List<IRoom> rooms = GetRooms();

            List<IAvaliableRoom> avaliablerooms = new List<IAvaliableRoom>(), temp;
            foreach (IRoom room in rooms)
            {
                bool foundroom = false;
                foreach (AvaliableRoom avaliroom in avaliablerooms)
                {
                    if (avaliroom.RType == room.RType)
                    {
                        foundroom = true;
                        avaliroom.Add();
                    }
                }
                if (foundroom == false)
                {
                    IRoomPrice rmp = dbCon.GetRoomPrice(room.RType);
                    if (rmp == null)
                    {
                        MessageBox.Show("房间价格未初始化！", "查询价格错误");
                        return avaliablerooms;
                    }
                    avaliablerooms.Add(new AvaliableRoom(room.RType, rmp.Price));
                }
            }

            if (roomtype != null)
            {
                temp = new List<IAvaliableRoom>();
                foreach (IAvaliableRoom avaliroom in avaliablerooms)
                {
                    if (avaliroom.RType == roomtype)
                    {
                        temp.Add(avaliroom);
                    }
                }
                avaliablerooms = temp;
            }

            if (startdate != null && enddate != null)
            {
                foreach (AvaliableRoom avaliroom in avaliablerooms)
                {
                    foreach (IBooking booking in bookings)
                    {
                        if (booking.Roomtype == avaliroom.RType)
                        {
                            bool overlap = startdate < booking.EndDate && enddate > booking.StartDate;
                            if (overlap)
                                avaliroom.Reduce();
                        }
                    }
                }
            }
            
            return avaliablerooms;
        }

        internal IRoomPrice UpdateRoomPrice(RoomType rType, double price)
        {
            if (dbCon.GetRoomPrice(rType) != null)
                return dbCon.UpdateRoomPrice(new RoomPrice(rType, price));
            else
                return dbCon.CreateRoomPrice(rType, price);
        }

        internal IRoomPrice GetRoomPrice(RoomType rType)
        {
            return dbCon.GetRoomPrice(rType);
        }

        internal IRoomPrice GetRoomPrice(string roomNum)
        {
            var roomlist = dbCon.GetRooms();
            foreach (IRoom rm in roomlist)
            {
                if (rm.RStatus != RoomStatus.NA && rm.RoomNum == roomNum)
                {
                    return dbCon.GetRoomPrice(rm.RType);
                }
            }
            return null;
        }

        internal List<IRoom> RefreshRooms(List<IRoom> roomlist)
        {
            var temp = new List<IRoom>();
            foreach (IRoom rm in roomlist)
            {
                //IRoom newroom = CreateRoom(rm.RoomNum, rm.RType);
                //UpdateRoom(newroom);
                //rm.RStatus = RoomStatus.NA;
                //UpdateRoom(rm);
                //temp.Add(newroom);
                temp.Add(RefreshRoom(rm));
            }
            return temp;
        }
        internal IRoom RefreshRoom(IRoom room)
        {
            IRoom newroom = CreateRoom(room.RoomNum, room.RType);
            room.RStatus = RoomStatus.NA;
            UpdateRoom(room);
            return UpdateRoom(newroom);
        }
    }
}
 
