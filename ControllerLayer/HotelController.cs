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

        //Initializes a HotelController.r
        internal HotelController(SQLiteController db)
        {//初始化
            dbCon = db;
        }

        // Adds a room to the database.
        internal IRoom CreateRoom(int hotelID, int roomNum, double price, RoomType rType, RoomStatus rStatus)
        {
            return dbCon.CreateRoom(hotelID, roomNum, price, rType, rStatus);
        }

        // Returns a IRoom from id.
        internal IRoom GetRoom(int id)
        {
            return dbCon.GetRoom(id);
        }

        /// Returns a list of all the rooms.
        internal List<IRoom> GetRooms()
        {
            return dbCon.GetRooms();
        }

        /// Updates a room
        /// Room to update based on ID
        internal IRoom UpdateRoom(IRoom room)
        {
            return dbCon.UpdateRoom(room);
        }


        // Checks in the customer. By changing CStatus and RStatus on the rooms booked.
        internal IRoom CheckInRoom(IRoom room)
        {
            room.RStatus = RoomStatus.Occupied;
            return dbCon.UpdateRoom(room);
        }


        //Return a list of available rooms
        internal List<IRoom> GetAvailableRooms(RoomType? roomtype, DateTime? startdate, DateTime? enddate, int hotelID = -1)
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
                            // If the selected startdate is before or the same day as the booking enddate
                            // AND the selected enddate is after the booking startdate.
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
 
