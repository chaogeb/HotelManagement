using Interface;
using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Data.SQLite;

/// <summary>
/// Require: System.Data.SQLite.dll
/// Version: SQLite for .NET Framework 4
/// Download from http://system.data.sqlite.org/
/// or copy: ..\ControllerLayer\System.Data.SQLite.dll
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

        internal void SQLiteController()
        {
            string dbPath = "Data Source =" + Environment.CurrentDirectory + "/HotelDataBase.db";
            sqlCon = new SQLiteConnection(dbPath);
            sqlCon.Open();//打开数据库，若文件不存在会自动创建   
            string sql1 = "CREATE TABLE IF NOT EXISTS Room(ROOMID varchar, ROOMNUM varchar,RTYPE varchar,RSTATUS varchar );";//建表语句   
            SQLiteCommand cmdCreateTable1 = new SQLiteCommand(sql1, sqlCon);
            cmdCreateTable1.ExecuteNonQuery();//如果表不存在，创建数据表  
 
            string sql2 = "CREATE TABLE IF NOT EXISTS Customer(CUSTOMERID varchar, NAME varchar, GENDER varchar,AGE varchar,PHONE varchar,FAX varchar,IDCARD varchar,ROOMTD varchar,COMPANY varchar,ADDRESS avrchar,CITY varchar );";//建表语句   
            SQLiteCommand cmdCreateTable2 = new SQLiteCommand(sql2, sqlCon);
            cmdCreateTable2.ExecuteNonQuery();//如果表不存在，创建数据表  

            string sql3 = "CREATE TABLE IF NOT EXISTS Booking(BOOKINGID varchar, STARTDATE varchar, ENDDATE varchar,RESERVETIME varchar,CONTRACTID varchar,ROOMTYPE varchar,THISPRICE double,ROOMID varchar,RESERVATION varchar,BSTATUS varchar );";//建表语句   
            SQLiteCommand cmdCreateTable3 = new SQLiteCommand(sql3, sqlCon);
            cmdCreateTable3.ExecuteNonQuery();//如果表不存在，创建数据表  

            string sql4 = "CREATE TABLE IF NOT EXISTS Reservation(RESERVATIONID varchar, PAYMENT varchar, DOWNPAYMENT varchar,RSTATUS varchar );";//建表语句   
            SQLiteCommand cmdCreateTable4 = new SQLiteCommand(sql4, sqlCon);
            cmdCreateTable4.ExecuteNonQuery();//如果表不存在，创建数据表  

            string sql5 = "CREATE TABLE IF NOT EXISTS Clock(TIME varchar, COUNTROOM varchar, COUNTCUSTOMER varchar,COUNTBOOKING varchar,COUNTRESERVE varchar );";//建表语句   
            SQLiteCommand cmdCreateTable5 = new SQLiteCommand(sql5, sqlCon);
            cmdCreateTable5.ExecuteNonQuery();//如果表不存在，创建数据表  

            string sql6 = "CREATE TABLE IF NOT EXISTS RoomPrice(RTYPE varchar, RPRICE varchar );";//建表语句   
            SQLiteCommand cmdCreateTable6 = new SQLiteCommand(sql5, sqlCon);
            cmdCreateTable5.ExecuteNonQuery();//如果表不存在，创建数据表  

            sqlCon.Close();
        }

        internal bool Authenticated()
        {
            try
            {
                sqlCon.Open();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                sqlCon.Close();
            }
        }
        #endregion

        #region Customer Procedures

        internal ICustomer GetCustomer(string id)
        {
            connect();
            SQLiteCommand cmd = new SQLiteCommand("GetCustomer", sqlCon);

            cmd.Parameters.AddWithValue("@CUSTOMERID", id);

            try
            {
                SQLiteDataReader rdr = cmd.ExecuteReader();
                rdr.Read();
                return new Customer()
                {
                    Name = rdr["NAME"].ToString(),
                    Gender = (CustomerGender)Enum.Parse(typeof(CustomerGender), rdr["GENDER"].ToString()),
                    Age = int.Parse(rdr["AGE"].ToString()),
                    Phone = rdr["PHONE"].ToString(),
                    Fax = rdr["FAX"].ToString(),
                    IDcard = rdr["IDCARD"].ToString(),
                    RoomID = rdr["ROOMID"].ToString(),
                    Company = rdr["COMPANY"].ToString(),
                    Address = rdr["ADDRESS"].ToString()
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
                          rdr["CUSTOMERID"].ToString(),
                          rdr["NAME"].ToString(),
                          (CustomerGender)Enum.Parse(typeof(CustomerGender), rdr["GENDER"].ToString()),
                          int.Parse(rdr["AGE"].ToString()),
                          rdr["PHONE"].ToString(),
                          rdr["FAX"].ToString(),
                          rdr["IDCARD"].ToString(),
                          rdr["ROOMID"].ToString(),
                          rdr["COMPANY"].ToString(),
                          rdr["ADDRESS"].ToString()
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

            cmd.Parameters.AddWithValue("@CUSTOMERID", customer.ID);
            cmd.Parameters.AddWithValue("@NAME", customer.Name);
            cmd.Parameters.AddWithValue("@GENDER", customer.Gender);
            cmd.Parameters.AddWithValue("@AGE", customer.Age);
            cmd.Parameters.AddWithValue("@PHONE", customer.Phone);
            cmd.Parameters.AddWithValue("@FAX", customer.Fax);
            cmd.Parameters.AddWithValue("@IDCARD", customer.IDcard);
            cmd.Parameters.AddWithValue("@ROOMID", customer.RoomID);
            cmd.Parameters.AddWithValue("@COMPANY", customer.Company);
            cmd.Parameters.AddWithValue("@ADDRESS", customer.Address);

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

        internal ICustomer CreateCustomer(string id,string name, CustomerGender gender,int age, string phone,string fax,string idcard,string roomid, string company,string address)
        {
            connect();
            SQLiteCommand cmd = new SQLiteCommand("CreateCustomer", sqlCon);

            cmd.Parameters["@CUSTOMERID"].Direction = ParameterDirection.Output;

            cmd.Parameters.AddWithValue("@CUSTOMERID", id);
            cmd.Parameters.AddWithValue("@NAME", name);
            cmd.Parameters.AddWithValue("@GENDER", gender);
            cmd.Parameters.AddWithValue("@AGE", age);
            cmd.Parameters.AddWithValue("@PHONE", phone);
            cmd.Parameters.AddWithValue("@FAX", fax);
            cmd.Parameters.AddWithValue("@IDCARD", idcard);
            cmd.Parameters.AddWithValue("@ROOMID", roomid);
            cmd.Parameters.AddWithValue("@COMPANY", company);
            cmd.Parameters.AddWithValue("@ADDRESS", address);

            try
            {
                cmd.ExecuteNonQuery();
                id = cmd.Parameters["@CUSTOMERID"].Value.ToString();
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
            string id , roomNum;

            try
            {
                while (r.Read())
                {
                    rStatus = (RoomStatus)Enum.Parse(typeof(RoomStatus), r["RSTATUS"].ToString());
                    rType = (RoomType)Enum.Parse(typeof(RoomType), r["RTYPE"].ToString());
                    id = r["ROOMID"].ToString();
                    roomNum = r["ROOMNUM"].ToString();
                    temp.Add(new Room(id, roomNum, rType, rStatus));

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

        internal IRoom GetRoom(string id)
        {
            connect();

            SQLiteCommand cmd = new SQLiteCommand("GetRoom", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ROOMID", id);

            try
            {
                SQLiteDataReader rdr = cmd.ExecuteReader();
                rdr.Read();

                return new Room()
                {
                    RoomNum = rdr["ROOMNUM"].ToString(),
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

        internal IRoom CreateRoom(string id, string roomNum,  RoomType rType, RoomStatus rStatus)
        {
            connect();

            SQLiteCommand cmd = new SQLiteCommand("CreateRoom", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters["@ROOMID"].Direction = ParameterDirection.Output;

            cmd.Parameters.AddWithValue("@ROOMID", id);
            cmd.Parameters.AddWithValue("@ROOMNUM", roomNum);
            cmd.Parameters.AddWithValue("@RTYPE", rType.ToString());
            cmd.Parameters.AddWithValue("@RSTATUS", rStatus.ToString());

            cmd.ExecuteNonQuery();
            return GetRoom(id);
           
        }

        internal IRoom UpdateRoom(IRoom room)
        {
            connect();

            SQLiteCommand cmd = new SQLiteCommand("UpdateRoom", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ROOMID", room.ID);
            cmd.Parameters.AddWithValue("@ROOMNUM", room.RoomNum);
            cmd.Parameters.AddWithValue("@RTYPE", room.RType.ToString());
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

        internal IBooking GetBooking(string bookingID)
        {
            connect();
            Booking book;
            SQLiteCommand cmd = new SQLiteCommand("GetBooking", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BOOKINGID", bookingID);

            try
            {
                SQLiteDataReader rdr = cmd.ExecuteReader();
                rdr.Read();
                book = new Booking
                    (
                        rdr["BOOKINGID"].ToString(),
                        DateTime.Parse(rdr["STARTDATE"].ToString()),
                        DateTime.Parse(rdr["ENDDATE"].ToString()),
                        rdr["RESERVETIME"].ToString(),
                        rdr["CONTRACTID"].ToString(),
                        (RoomType)Enum.Parse(typeof(RoomType), (rdr["RTYPE"].ToString())),
                        double.Parse(rdr["THISPRICE"].ToString()),
                        rdr["ROOMID"].ToString(),
                        rdr["RESERVATIONID"].ToString()
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

        internal IBooking CreateBooking(string id, DateTime start, DateTime end, string reservetime, string contractid,  RoomType roomtype,double thisprice,string roomid,string reservationid)
        {
            connect();
            SQLiteCommand cmd = new SQLiteCommand("CreateBooking", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters["@BOOKINGID"].Direction = ParameterDirection.Output;

            cmd.Parameters.AddWithValue("@BOOKINGID", id);
            cmd.Parameters.AddWithValue("@STARTDATE", start);
            cmd.Parameters.AddWithValue("@ENDDATE", end);
            cmd.Parameters.AddWithValue("@RESERVETIME", reservetime);
            cmd.Parameters.AddWithValue("@CONTRACTID", contractid);
            cmd.Parameters.AddWithValue("@ROOMTYPE", roomtype);
            cmd.Parameters.AddWithValue("@THISPRICE", thisprice);
            cmd.Parameters.AddWithValue("@ROOMID", roomid);
            cmd.Parameters.AddWithValue("@RESERVATIONID", reservationid);
            cmd.Parameters.AddWithValue("@BSTATUS", "Confirmed");

            try
            {
                cmd.ExecuteNonQuery();
                id = cmd.Parameters["@BOOKINGID"].Value.ToString();
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

        internal List<IBooking> GetBookings(string customerID)
        {
            connect();
            SQLiteCommand cmd = new SQLiteCommand("GetBookings", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CUSTOMERID", customerID);

            List<IBooking> bookings = new List<IBooking>();
            try
            {
                SQLiteDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    bookings.Add(new Booking
                    (
                        rdr["BOOKINGID"].ToString(),
                        DateTime.Parse(rdr["STARTDATE"].ToString()),
                        DateTime.Parse(rdr["ENDDATE"].ToString()),
                        rdr["RESERVETIME"].ToString(),
                        rdr["CONTRACTID"].ToString(),
                        (RoomType)Enum.Parse(typeof(RoomType), (rdr["ROOMTYPE"].ToString())),
                        double.Parse(rdr["THISPRICE"].ToString()),
                        rdr["ROOMID"].ToString(),
                        rdr["RESERVATIONID"].ToString()
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
                        rdr["BOOKINGID"].ToString(),
                        DateTime.Parse(rdr["STARTDATE"].ToString()),
                        DateTime.Parse(rdr["ENDDATE"].ToString()),
                        rdr["RESERVETIME"].ToString(),
                        rdr["CONTRACTID"].ToString(),
                        (RoomType)Enum.Parse(typeof(RoomType), (rdr["ROOMTYPE"].ToString())),
                        double.Parse(rdr["THISPRICE"].ToString()),
                        rdr["ROOMID"].ToString(),
                        rdr["RESERVATIONID"].ToString()
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

        #region Reservation Procedures
        internal IReservation GetReservation(string ReservationID)
        {
            connect();
            Reservation reservation;
            SQLiteCommand cmd = new SQLiteCommand("GetReservation", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BOOKINGID", ReservationID);

            try
            {
                SQLiteDataReader rdr = cmd.ExecuteReader();
                rdr.Read();
                reservation = new Reservation
                    (
                        rdr["RESERVATIONID"].ToString(),
                        double.Parse(rdr["PAYMENT"].ToString()),
                        double.Parse(rdr["DOWNPAYMENT"].ToString()),
                        (ReservationStatus)Enum.Parse(typeof(ReservationStatus), rdr["RSTATUS"].ToString())
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
            return reservation;
        }

        internal IReservation CreateReservation(string id, double payment, double downpayment, ReservationStatus rstatus)
        {
            connect();
            SQLiteCommand cmd = new SQLiteCommand("CreateReservation", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters["@RESERVATIONID"].Direction = ParameterDirection.Output;

            cmd.Parameters.AddWithValue("@RESERVATIONID", id);
            cmd.Parameters.AddWithValue("@PAYMENT", payment);
            cmd.Parameters.AddWithValue("@DOWNPAYMENT", downpayment);
            cmd.Parameters.AddWithValue("@RSTATUS", rstatus);

            try
            {
                cmd.ExecuteNonQuery();
                id = cmd.Parameters["@RESERVATIONID"].Value.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("Could not add booking to database!\n" + ex.Message);
            }
            finally
            {
                disconnect();
            }
            return GetReservation(id);
        }

        internal List<IReservation> GetReservations(string reservationID)
        {
            connect();
            SQLiteCommand cmd = new SQLiteCommand("GetReservations", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@RESERVATIONID", reservationID);

            List<IReservation> reservations = new List<IReservation>();
            try
            {
                SQLiteDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    reservations.Add(new Reservation
                    (
                        rdr["RESERVATIONID"].ToString(),
                        double.Parse(rdr["PAYMENT"].ToString()),
                        double.Parse(rdr["DOWNPAYMENT"].ToString()),
                        (ReservationStatus)Enum.Parse(typeof(ReservationStatus), rdr["RSTATUS"].ToString())
                    ));
                }
                return reservations;
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

        internal List<IReservation> GetReservation()
        {
            connect();
            SQLiteCommand cmd = new SQLiteCommand("GetAllReservations", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;

            List<IReservation> reservations = new List<IReservation>();
            try
            {
                SQLiteDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    reservations.Add(new Reservation
                    (
                        rdr["RESERVATIONID"].ToString(),
                        double.Parse(rdr["PAYMENT"].ToString()),
                        double.Parse(rdr["DOWNPAYMENT"].ToString()),
                        (ReservationStatus)Enum.Parse(typeof(ReservationStatus), rdr["RSTATUS"].ToString())
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
            return reservations;
        }

        #endregion
    }
}
