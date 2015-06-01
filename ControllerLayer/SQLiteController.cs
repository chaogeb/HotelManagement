using Interface;
using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;

/// <summary>
/// Require C:\Program Files (x86)\System.Data.SQLite\2013\bin\System.Data.SQLite.dll
/// Version: SQLite for .NET Framework 4.5.1
/// Download from http://system.data.sqlite.org/
/// or copy: Project_Dir\..\ControllerLayer\System.Data.SQLite.dll
/// </summary>
namespace ControllerLayer
{
    internal class SQLiteController
    {
        #region Misc

        SQLiteConnection sqlCon = null;

        /// <summary>
        /// Opens the connection to the database.
        /// </summary>
        /// <returns>If success.</returns>
        private void connect()
        {
            try
            {
                sqlCon.Open();
            }
            catch
            {
                throw new Exception("Connection to database failed!");
            }
        }

        /// <summary>
        /// Closes the connection to the database.
        /// </summary>
        /// <returns>Id success.</returns>
        private void disconnect()
        {
            try
            {
                sqlCon.Close();
            }
            catch
            {
                throw new Exception("Disconnection from database failed!");
            }
        }

        internal void DatabaseController()
        {
            string dbPath = "Data Source =" + Environment.CurrentDirectory + "/HotelDataBase.db";
            sqlCon = new SQLiteConnection(dbPath);
        }
        #endregion

        #region Customer Procedures

        internal ICustomer GetCustomer(int id)
        {
            connect();
            SQLiteCommand cmd = new SQLiteCommand("GetCustomer", sqlCon);

            cmd.Parameters.AddWithValue("@ID", id);

            try
            {
                SQLiteDataReader rdr = cmd.ExecuteReader();
                rdr.Read();
                return new Customer(int.Parse(rdr["ID"].ToString()))
                {
                    Name = rdr["NAME"].ToString(),
                    Age = int.Parse(rdr["AGE"].ToString()),
                    Phone = rdr["PHONE"].ToString(),
                    Company = rdr["COMPANY"].ToString(),
                    City = rdr["CITY"].ToString(),
                    CStatus = (CustomerStatus)Enum.Parse(typeof(CustomerStatus), rdr["CSTATUS"].ToString())
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Could not parse information. Failed to create new customer.\n" + ex.Message);
            }
            finally
            {
                disconnect();
            }
        }

        internal List<ICustomer> GetCustomers()
        {
            connect();

            List<ICustomer> customers = new List<ICustomer>();
            SQLiteCommand cmd = new SQLiteCommand("GetCustomers", sqlCon);
            SQLiteDataReader rdr = cmd.ExecuteReader();


            try
            {
                while (rdr.Read())
                {
                    customers.Add(new Customer
                        (
                        rdr["NAME"].ToString(),
                        rdr["GENDER"].ToString(),
                        int.Parse(rdr["AGE"].ToString()),
                        rdr["PHONE"].ToString(),
                        int.Parse(rdr["ID"].ToString()),
                        rdr["COMPANY"].ToString(),
                        rdr["CITY"].ToString(),
                        (CustomerStatus)Enum.Parse(typeof(CustomerStatus), rdr["CSTATUS"].ToString())
                        ));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to parse customers!\n" + ex.Message);
            }
            finally
            {
                disconnect();
            }
            return customers;
        }

        internal ICustomer UpdateCustomer(ICustomer customer)
        {
            connect();
            SQLiteCommand cmd = new SQLiteCommand("UpdateCustomer", sqlCon);

            cmd.Parameters.AddWithValue("@ID", customer.ID);
            cmd.Parameters.AddWithValue("@NAME", customer.Name);
            cmd.Parameters.AddWithValue("@GENDER", customer.Gender);
            cmd.Parameters.AddWithValue("@AGE", customer.Age);
            cmd.Parameters.AddWithValue("@PHONE", customer.Phone);
            cmd.Parameters.AddWithValue("@COMPANY", customer.Company);
            cmd.Parameters.AddWithValue("@CITY", customer.City);
            cmd.Parameters.AddWithValue("@CSTATUS", customer.CStatus.ToString());

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Customer with ID: " + customer.ID + " could not be updated!\n" + ex.Message);
            }
            finally
            {
                disconnect();
            }
            return GetCustomer(customer.ID);
        }

        internal ICustomer CreateCustomer(string name, string gender, string phone, int id, string company, string city, CustomerStatus status)
        {
            connect();
            SQLiteCommand cmd = new SQLiteCommand("CreateCustomer", sqlCon);

            cmd.Parameters["@ID"].Direction = ParameterDirection.Output;

            cmd.Parameters.AddWithValue("@NAME", name);
            cmd.Parameters.AddWithValue("@GENDER", gender);
            cmd.Parameters.AddWithValue("@PHONE", phone);
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@COMPANY", company);
            cmd.Parameters.AddWithValue("@CITY", city);
            cmd.Parameters.AddWithValue("@CSTATUS", status.ToString());

            try
            {
                cmd.ExecuteNonQuery();
                id = int.Parse(cmd.Parameters["@ID"].Value.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("Could not add customer to database!\n" + ex.Message);
            }
            finally
            {
                disconnect();
            }
            return GetCustomer(id);
        }

        internal bool DeleteCustomer(int customerID)
        {
            connect();
            SQLiteCommand cmd = new SQLiteCommand("DeleteCustomer", sqlCon);

            cmd.Parameters.AddWithValue("@ID", customerID);

            try
            {
                int customersDeleted = cmd.ExecuteNonQuery();

                int id = int.Parse(cmd.Parameters["@ID"].Value.ToString());
                return customersDeleted > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed deleting the customer!\n" + ex.Message);
            }
        }

        #endregion

        #region Room Procedures

        internal List<IRoom> GetRooms()
        {
            connect();

            List<IRoom> temp = new List<IRoom>();
            SQLiteCommand cmd = new SQLiteCommand("GetRooms", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            SQLiteDataReader r = cmd.ExecuteReader();

            RoomType rType;
            RoomStatus rStatus;
            int id, hotelID, roomNum;
            double price;

            try
            {
                while (r.Read())
                {
                    rStatus = (RoomStatus)Enum.Parse(typeof(RoomStatus), r["RSTATUS"].ToString());
                    rType = (RoomType)Enum.Parse(typeof(RoomType), r["RTYPE"].ToString());

                    id = int.Parse(r["ID"].ToString());
                    roomNum = int.Parse(r["ROOMNUM"].ToString());
                    price = double.Parse(r["PRICE"].ToString());

                    temp.Add(new Room(id, roomNum, price, rType, rStatus));

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

        internal IRoom GetRoom(int id)
        {
            connect();

            SQLiteCommand cmd = new SQLiteCommand("GetRoom", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);

            try
            {
                SQLiteDataReader rdr = cmd.ExecuteReader();
                rdr.Read();

                return new Room(int.Parse(rdr["ID"].ToString()))
                {
                    RoomNum = int.Parse((rdr["ROOMNUM"].ToString())),
                    Price = double.Parse((rdr["PRICE"].ToString())),
                    RType = (RoomType)Enum.Parse(typeof(RoomType), (rdr["RTYPE"].ToString())),
                    RStatus = (RoomStatus)Enum.Parse(typeof(RoomStatus), (rdr["RSTATUS"].ToString()))
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Could not parse information from database. Failed to create new Room\n" + ex.Message);
            }
            finally
            {
                disconnect();
            }
        }

        internal IRoom CreateRoom(int id, int roomNum, double price, RoomType rType, RoomStatus rStatus)
        {
            connect();

            SQLiteCommand cmd = new SQLiteCommand("CreateRoom", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters["@ID"].Direction = ParameterDirection.Output;

            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@ROOMNUM", roomNum);
            cmd.Parameters.AddWithValue("@PRICE", price);
            cmd.Parameters.AddWithValue("@RTYPE", rType.ToString());
            cmd.Parameters.AddWithValue("@RSTATUS", rStatus.ToString());

            cmd.ExecuteNonQuery();
            if (int.TryParse(cmd.Parameters["@ID"].Value.ToString(), out id))
            {
                disconnect();
                return GetRoom(id);
            }
            else
            {
                disconnect();
                throw new Exception("Returning IRoom Failed!");
            }
        }

        internal IRoom UpdateRoom(IRoom room)
        {
            connect();

            SQLiteCommand cmd = new SQLiteCommand("UpdateRoom", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ID", room.ID);
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

        #endregion

        #region Booking Procedures

        internal bool DeleteBooking(int bookingID)
        {
            connect();
            SQLiteCommand cmd = new SQLiteCommand("DeleteBooking", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", bookingID);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to delete Booking from databse!\n" + ex.Message);
            }
            finally
            {
                disconnect();
            }
            return true;
        }

        internal IBooking GetBooking(int bookingID)
        {
            connect();
            Booking book;
            SQLiteCommand cmd = new SQLiteCommand("GetBooking", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", bookingID);

            try
            {
                SQLiteDataReader rdr = cmd.ExecuteReader();
                rdr.Read();
                book = new Booking
                    (
                        int.Parse(rdr["ID"].ToString()),
                        DateTime.Parse(rdr["STARTDATE"].ToString()),
                        DateTime.Parse(rdr["ENDDATE"].ToString()),
                        double.Parse(rdr["PRICE"].ToString()),
                        rdr["COMMENT"].ToString(),
                        (BookingType)Enum.Parse(typeof(BookingType), rdr["BTYPE"].ToString()),
                        int.Parse(rdr["ROOMID"].ToString())
                    );
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to parse/create new booking!\n" + ex.Message);
            }
            finally
            {
                disconnect();
            }
            return book;
        }

        internal IBooking CreateBooking(ICustomer customer, DateTime start, DateTime end, double price, string comment, BookingType type, long roomId)
        {
            connect();
            SQLiteCommand cmd = new SQLiteCommand("CreateBooking", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters["@ID"].Direction = ParameterDirection.Output;

            cmd.Parameters.AddWithValue("@CUSTOMERID", customer.ID);
            cmd.Parameters.AddWithValue("@STARTDATE", start);
            cmd.Parameters.AddWithValue("@ENDDATE", end);
            cmd.Parameters.AddWithValue("@PRICE", price);
            cmd.Parameters.AddWithValue("@COMMENT", comment);
            cmd.Parameters.AddWithValue("@BTYPE", type.ToString());
            cmd.Parameters.AddWithValue("@BSTATUS", "Confirmed");
            cmd.Parameters.AddWithValue("@ROOMID", roomId);

            int id;
            try
            {
                cmd.ExecuteNonQuery();
                id = int.Parse(cmd.Parameters["@ID"].Value.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("Could not add booking to database!\n" + ex.Message);
            }
            finally
            {
                disconnect();
            }
            return GetBooking(id);
        }

        internal List<IBooking> GetBookings(int customerID)
        {
            connect();
            SQLiteCommand cmd = new SQLiteCommand("GetBookings", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CustomerID", customerID);

            List<IBooking> bookings = new List<IBooking>();
            try
            {
                SQLiteDataReader rdr = cmd.ExecuteReader();

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
                return bookings;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not parse bookings!\n" + ex.Message);
            }
            finally
            {
                disconnect();
            }
        }

        internal List<IBooking> GetBookings()
        {
            connect();
            SQLiteCommand cmd = new SQLiteCommand("GetAllBookings", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;

            List<IBooking> bookings = new List<IBooking>();
            try
            {
                SQLiteDataReader rdr = cmd.ExecuteReader();

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
        #endregion

    }
}
