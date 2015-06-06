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

        /// <summary>
        /// Initializes a HotelController.r
        /// </summary>
        /// <param name="db"></param>

        internal HotelController(SQLiteController db)
        {
            dbCon = db;
        }

        /// <summary>
        ///  Adds a room to the database.
        /// </summary>
        /// <param name="hotelID"></param>
        /// <param name="roomNum"></param>
        /// <param name="price"></param>
        /// <param name="rType"></param>
        /// <param name="rStatus"></param>
        /// <returns></returns>

        internal IRoom CreateRoom(int hotelID, int roomNum, double price, RoomType rType, RoomStatus rStatus)
        {
            return dbCon.CreateRoom(hotelID, roomNum, price, rType, rStatus);
        }

        /// <summary>
        ///  Returns a IRoom from id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        internal IRoom GetRoom(int id)
        {
            return dbCon.GetRoom(id);
        }

        /// <summary>
        ///  Returns a list of all the rooms.
        /// </summary>
        /// <returns></returns>

        internal List<IRoom> GetRooms()
        {
            return dbCon.GetRooms();
        }

        /// <summary>
        /// Updates a room,Room to update based on ID
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
 
        internal IRoom UpdateRoom(IRoom room)
        {
            return dbCon.UpdateRoom(room);
        }


        /// <summary>
        ///  Checks in the customer. By changing CStatus and RStatus on the rooms booked.
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>

        internal IRoom CheckInRoom(IRoom room)
        {
            room.RStatus = RoomStatus.Occupied;
            return dbCon.UpdateRoom(room);
        }


        /// <summary>
        /// Return a list of available rooms
        /// </summary>
        /// <param name="roomtype"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <param name="hotelID"></param>
        /// <returns></returns>

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
 
