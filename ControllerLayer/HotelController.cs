using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interface;
using Model;


namespace ControllerLayer
{
    internal class HotelController
    {
        private SQLiteController dbCon;

        internal HotelController(SQLiteController db)
        {
            dbCon = db;
        }

        internal IRoom CreateRoom(int id, int roomNum, double price, RoomType rType, RoomStatus rStatus)
        {
            return dbCon.CreateRoom(id, roomNum, price, rType, rStatus);
        }

        internal IRoom GetRoom(int id)
        {
            return dbCon.GetRoom(id);
        }

        internal List<IRoom> GetRooms()
        {
                return dbCon.GetRooms();
        }

        internal IRoom UpdateRoom(IRoom room)
        {
            return dbCon.UpdateRoom(room);
        }

        internal IRoom CheckInRoom(IRoom room)
        {
            room.RStatus = RoomStatus.Occupied;
            return dbCon.UpdateRoom(room);
        }

        internal List<IRoom> GetAvailableRooms(RoomType? roomtype, DateTime? startdate, DateTime? enddate)
        {
            List<IBooking> bookings = dbCon.GetBookings();
            List<IRoom> rooms = GetRooms(), temp;

            if (roomtype != null)
            {
                temp = new List<IRoom>();
                foreach (IRoom room in rooms)
                {
                    if (room.RType == roomtype)
                        temp.Add(room);
                }
                rooms = temp;
            }
            if (startdate != null && enddate != null)
            {
                bool bookingForRoom;
                temp = new List<IRoom>();
                foreach (IRoom room in rooms)
                {
                    bookingForRoom = false;
                    foreach (IBooking booking in bookings)
                    {
                        if (room.ID == booking.RoomID)
                        {
                            bookingForRoom = true;
                            bool overlap = startdate < booking.EndDate && enddate > booking.StartDate;
                            if (!overlap)
                                temp.Add(room);
                        }
                    }
                    if (!bookingForRoom)
                        temp.Add(room);
                }
                rooms = temp;
            }

            return rooms;
        }
    }
}
 
