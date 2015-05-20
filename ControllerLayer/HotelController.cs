using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interface;
using Model;


namespace ControllerLayer
{
    class DatabaseController
    {
         internal IRoom CreateRoom(int hotelID, int roomNum, double price, RoomType rType, RoomStatus rStatus)
        {
            //打开文档connect();
            //生成新空间new,存储room内容
             IRoom new_room;
             //new_room.HotelID=hotelID;//酒店ID只能看 不能改=。=
             new_room.RoomNum=roomNum;
             new_room.Price=price;
             new_room.RType=rType;
             new_room.RStatus=rStatus;
             ///写入文件

             return new_room; 
            /*int id;
            if (int.TryParse(cmd.Parameters["@ID"].Value.ToString(), out id))
            {
                disconnect();
                return GetRoom(id);
            }*/
            
        }

        internal IRoom GetRoom(int id)
        {
            //打开文档connect();
            //访问第id个数据
            //创建一个局部变量用于存储读取的数据
            IRoom room;
            room.RoomNum=id;
            room.Price=;
            room.RType=;
            room.RStatus=;

            return room;
        }

        // Gets all the rooms from the database.
        internal List<IRoom> GetRooms()
        {
            //打开文件connect();

            List<IRoom> temp = new List<IRoom>();
            /*SqlCommand cmd = new SqlCommand("GetRooms", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader r = cmd.ExecuteReader();*/

            RoomType rType;
            RoomStatus rStatus;
            int id, hotelID, roomNum;
            double price;

            try
            {
                while (1)//r.Read()读到文件尾
                {
                    rStatus = (RoomStatus)Enum.Parse(typeof(RoomStatus), r["RSTATUS"].ToString());
                    rType = (RoomType)Enum.Parse(typeof(RoomType), r["RTYPE"].ToString());

                    /*id = int.Parse(r["ID"].ToString());
                    hotelID = int.Parse(r["HOTELID"].ToString());
                    roomNum = int.Parse(r["ROOMNUM"].ToString());
                    price = double.Parse(r["PRICE"].ToString());*/
                    //从文件读数据

                    /*temp.Add(new Room(id, hotelID, roomNum, price, rType, rStatus));*/

                }
                return temp;
            }
            catch (Exception ex)
            {
                throw new Exception("An error ocoured trying to parse data from database!\n" + ex.Message);
            }
            finally
            {
                disconnect();
            }
        }

        /// <summary>
        /// Made by: Andreana
        /// </summary>
        /// <returns></returns>
        internal List<IHotel> GetHotels()
        {
            connect();
            List<IHotel> hotels = new List<IHotel>();
            SqlCommand cmd = new SqlCommand("GetHotels", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    hotels.Add(new Hotel
                        (
                            int.Parse(rdr["ID"].ToString()),
                            rdr["NAME"].ToString(),
                            rdr["STREETNAME"].ToString(),
                            rdr["CITY"].ToString(),
                            rdr["EMAIL"].ToString(),
                            rdr["PHONE"].ToString(),
                            rdr["FAX"].ToString(),
                            (QualityStars)Enum.Parse(typeof(QualityStars), rdr["QUALITY"].ToString())
                        ));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create hotels!\n" + ex.Message);
            }
            finally
            {
                disconnect();
            }
            return hotels;
        }

        internal IRoom UpdateRoom(IRoom room)
        {
            //打开文件connect();
            //利用room.RoomNum找到相应位置开始重新写入

            SqlCommand cmd = new SqlCommand("UpdateRoom", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ID", room.ID);
            cmd.Parameters.AddWithValue("@HOTELID", room.HotelID);
            cmd.Parameters.AddWithValue("@ROOMNUM", room.RoomNum);
            cmd.Parameters.AddWithValue("@RTYPE", room.RType.ToString());
            cmd.Parameters.AddWithValue("@PRICE", room.Price);
            cmd.Parameters.AddWithValue("@RSTATUS", room.RStatus.ToString());

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Could not update room\n" + ex.Message);
            }
            finally
            {
                disconnect();
            }
            return GetRoom(room.ID);
        }

        internal List<IBooking> GetBookings()
        {
            connect();
            SqlCommand cmd = new SqlCommand("GetAllBookings", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;

            List<IBooking> bookings = new List<IBooking>();
            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    bookings.Add(new Booking
                    (
                        int.Parse(rdr["ID"].ToString()),
                        DateTime.Parse(rdr["STARTDATE"].ToString()),
                        DateTime.Parse(rdr["ENDDATE"].ToString()),
                        double.Parse(rdr["PRICE"].ToString()),
                        rdr["COMMENT"].ToString(),
                        (BookingType)Enum.Parse(typeof(BookingType), rdr["BTYPE"].ToString()),
                        int.Parse(rdr["ROOMID"].ToString())
                    ));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Could not parse bookings!\n" + ex.Message);
            }
            finally
            {
                disconnect();
            }
            return bookings;
        }
    }
    class HotelController
    {
        //初始化HotelController

       private DatabaseController dbCon;
        // 添加一个房间到database
        
        internal IRoom CreateRoom(int hotelID, int roomNum, double price, RoomType rType, RoomStatus rStatus)
        {
            return dbCon.CreateRoom(hotelID, roomNum, price, rType, rStatus);
        }



        internal HotelController(DatabaseController db)
        {
            dbCon = db;
        }
        
        
        /// Adds a room to the database.
        internal IRoom CreateRoom(int hotelID, int roomNum, double price, RoomType rType, RoomStatus rStatus)
        {
            return dbCon.CreateRoom(hotelID, roomNum, price, rType, rStatus);
        }
       
        /// Returns a IRoom from id.
        internal IRoom GetRoom (int id)
        {
            return dbCon.GetRoom(id);
        }

        /// Returns a list of all the rooms.
        internal List<IRoom> GetRooms(int hotelID = -1)
        {
            if (hotelID == -1)
            {
                return dbCon.GetRooms();
            }
            else
            {
                List<IRoom> temp = new List<IRoom>();
                foreach (IRoom room in dbCon.GetRooms())
                {
                    if (room.HotelID == hotelID)
                        temp.Add(room);
                }
                return temp;
            }
        }

       
        /// Updates a room
       
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

       
        internal List<IHotel> GetHotels()
        {
            return dbCon.GetHotels();
        }

        
        internal IHotel GetHotel(int hotelID)
        {
            foreach (IHotel hotel in dbCon.GetHotels())
                if (hotel.ID == hotelID)
                    return hotel;
            return null;
        }
        
        internal List<IRoom> GetAvailableRooms(RoomType? roomtype, DateTime? startdate, DateTime? enddate, int hotelID =-1)
        {
            List<IBooking> bookings = dbCon.GetBookings();
            List<IRoom> rooms = GetRooms(hotelID), temp;

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
        
        // Gets a single room based on id
 
