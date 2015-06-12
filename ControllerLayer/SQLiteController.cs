using Interface;
using Model;
using System;
using System.Collections.Generic;
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

        private SQLiteConnection sqlCon = null;
        
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

        internal SQLiteController()
        {
            string dbPath = "Data Source = " + Environment.CurrentDirectory + "/HotelDataBase.db";
            sqlCon = new SQLiteConnection(dbPath);
            sqlCon.Open();  //打开数据库，若文件不存在会自动创建

            string sql = "CREATE TABLE IF NOT EXISTS Room(ROOMID varchar, ROOMNUM varchar,RTYPE varchar,RSTATUS varchar );";//建表语句   
            SQLiteCommand cmdCreateTable = new SQLiteCommand(sql, sqlCon);
            cmdCreateTable.ExecuteNonQuery();  //如果表不存在，创建数据表

            cmdCreateTable.CommandText = "CREATE TABLE IF NOT EXISTS Customer(CUSTOMERID varchar, NAME varchar, GENDER varchar,AGE varchar,PHONE varchar,FAX varchar,IDCARD varchar,ROOMID varchar,COMPANY varchar,ADDRESS avrchar);";//建表语句   
            cmdCreateTable.ExecuteNonQuery();  //如果表不存在，创建数据表

            cmdCreateTable.CommandText = "CREATE TABLE IF NOT EXISTS Booking(BOOKINGID varchar, STARTDATE varchar, ENDDATE varchar,RESERVETIME varchar,CONTRACTID varchar,ROOMTYPE varchar,THISPRICE double,ROOMID varchar,RESERVATIONID varchar,BSTATUS varchar);";//建表语句   
            cmdCreateTable.ExecuteNonQuery();  //如果表不存在，创建数据表

            cmdCreateTable.CommandText = "CREATE TABLE IF NOT EXISTS Reservation(RESERVATIONID varchar, PAYMENT varchar, DOWNPAYMENT varchar,RSTATUS varchar );";//建表语句   
            cmdCreateTable.ExecuteNonQuery();  //如果表不存在，创建数据表

            cmdCreateTable.CommandText = "CREATE TABLE IF NOT EXISTS Clock(TIME varchar, COUNTROOM varchar, COUNTCUSTOMER varchar,COUNTBOOKING varchar,COUNTRESERVE varchar );";//建表语句   
            cmdCreateTable.ExecuteNonQuery();   //如果表不存在，创建数据表

            cmdCreateTable.CommandText = "CREATE TABLE IF NOT EXISTS RoomPrice(RTYPE varchar, RPRICE varchar );";
            cmdCreateTable.ExecuteNonQuery();   //如果表不存在，创建数据表

            cmdCreateTable.CommandText = "CREATE TABLE IF NOT EXISTS Log(DATETIME varchar, LOGTEXT varchar);";
            cmdCreateTable.ExecuteNonQuery();

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
            ICustomer customer = null;
            SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Customer WHERE CUSTOMERID = '" + id + "'", sqlCon);
            
            try
            {
                SQLiteDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                    customer = new Customer(id,
                        rdr["NAME"].ToString(),
                        (CustomerGender)Enum.Parse(typeof(CustomerGender), rdr["GENDER"].ToString()),
                        int.Parse(rdr["AGE"].ToString()),
                        rdr["PHONE"].ToString(),
                        rdr["FAX"].ToString(),
                        rdr["IDCARD"].ToString(),
                        rdr["ROOMID"].ToString(),
                        rdr["COMPANY"].ToString(),
                        rdr["ADDRESS"].ToString()
                    );
                rdr.Close();
                return customer;
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
            SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Customer", sqlCon);
            
            try
            {
                SQLiteDataReader rdr = cmd.ExecuteReader();
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
                rdr.Close();
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
            SQLiteCommand cmd = new SQLiteCommand("UPDATE Customer SET NAME=:NAME,GENDER=:GENDER,AGE=:AGE,PHONE=:PHONE," +
                "FAX=:FAX,IDCARD=:IDCARD,ROOMID=:ROOMID,COMPANY=:COMPANY,ADDRESS=:ADDRESS WHERE CUSTOMERID=:CUSTOMERID", sqlCon);

            cmd.Parameters.AddWithValue("CUSTOMERID", customer.ID);
            cmd.Parameters.AddWithValue("NAME", customer.Name);
            cmd.Parameters.AddWithValue("GENDER", customer.Gender.ToString());
            cmd.Parameters.AddWithValue("AGE", customer.Age);
            cmd.Parameters.AddWithValue("PHONE", customer.Phone);
            cmd.Parameters.AddWithValue("FAX", customer.Fax);
            cmd.Parameters.AddWithValue("IDCARD", customer.IDcard);
            cmd.Parameters.AddWithValue("ROOMID", customer.RoomID);
            cmd.Parameters.AddWithValue("COMPANY", customer.Company);
            cmd.Parameters.AddWithValue("ADDRESS", customer.Address);

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
            SQLiteCommand cmd = new SQLiteCommand("INSERT INTO Customer VALUES (@CUSTOMERID, @NAME, @GENDER, @AGE, @PHONE, @FAX, @IDCARD, @ROOMID, @COMPANY, @ADDRESS)", sqlCon);

            cmd.Parameters.AddWithValue("@CUSTOMERID", id);
            cmd.Parameters.AddWithValue("@NAME", name);
            cmd.Parameters.AddWithValue("@GENDER", gender.ToString());
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
            SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Room", sqlCon);

            RoomType rType;
            RoomStatus rStatus;
            string id , roomNum;

            try
            {
                SQLiteDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    rStatus = (RoomStatus)Enum.Parse(typeof(RoomStatus), rdr["RSTATUS"].ToString());
                    rType = (RoomType)Enum.Parse(typeof(RoomType), rdr["RTYPE"].ToString());
                    id = rdr["ROOMID"].ToString();
                    roomNum = rdr["ROOMNUM"].ToString();
                    temp.Add(new Room(id, roomNum, rType, rStatus));
                }
                rdr.Close();
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
            IRoom room = null;
            SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Room WHERE ROOMID = '" + id + "'", sqlCon);
            
            try
            {
                SQLiteDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                    room = new Room(id,
                        rdr["ROOMNUM"].ToString(),
                        (RoomType)Enum.Parse(typeof(RoomType), (rdr["RTYPE"].ToString())),
                        (RoomStatus)Enum.Parse(typeof(RoomStatus), (rdr["RSTATUS"].ToString()))
                    );
                rdr.Close();
                return room;
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

            SQLiteCommand cmd = new SQLiteCommand("INSERT INTO Room VALUES (@ROOMID,@ROOMNUM,@RTYPE,@RSTATUS)", sqlCon);
            
            cmd.Parameters.AddWithValue("@ROOMID", id);
            cmd.Parameters.AddWithValue("@ROOMNUM", roomNum);
            cmd.Parameters.AddWithValue("@RTYPE", rType.ToString());
            cmd.Parameters.AddWithValue("@RSTATUS", rStatus.ToString());

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Returning IRoom Failed!\n" + ex.Message);
            }
            finally
            {
                disconnect();
            }
            return GetRoom(id);
        }

        internal IRoom UpdateRoom(IRoom room)
        {
            connect();

            SQLiteCommand cmd = new SQLiteCommand("UPDATE Room SET ROOMNUM=:ROOMNUM,RTYPE=:RTYPE,RSTATUS=:RSTATUS" +
                " WHERE ROOMID=:ROOMID", sqlCon);
            
            cmd.Parameters.AddWithValue("ROOMID", room.ID);
            cmd.Parameters.AddWithValue("ROOMNUM", room.RoomNum);
            cmd.Parameters.AddWithValue("RTYPE", room.RType.ToString());
            cmd.Parameters.AddWithValue("RSTATUS", room.RStatus.ToString());

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
            Booking book = null;
            SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Booking WHERE BOOKINGID = '" + bookingID + "'", sqlCon);
            
            try
            {
                SQLiteDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                    book = new Booking(
                        rdr["BOOKINGID"].ToString(),
                        DateTime.ParseExact(rdr["STARTDATE"].ToString(), "yyyyMMdd", null),
                        DateTime.ParseExact(rdr["ENDDATE"].ToString(), "yyyyMMdd", null),
                        rdr["RESERVETIME"].ToString(),
                        rdr["CONTRACTID"].ToString(),
                        (RoomType)Enum.Parse(typeof(RoomType), (rdr["ROOMTYPE"].ToString())),
                        double.Parse(rdr["THISPRICE"].ToString()),
                        rdr["ROOMID"].ToString(),
                        rdr["RESERVATIONID"].ToString(),
                        (BookStatus)Enum.Parse(typeof(BookStatus), rdr["BSTATUS"].ToString())
                    );
                rdr.Close();
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

        internal IBooking CreateBooking(string id, DateTime start, DateTime end, string reservetime, string contractid,  RoomType roomtype, double thisprice, string reservationid)
        {
            connect();
            SQLiteCommand cmd = new SQLiteCommand("INSERT INTO Booking VALUES (@BOOKINGID,@STARTDATE,@ENDDATE,@RESERVETIME,@CONTRACTID,@ROOMTYPE,@THISPRICE,@ROOMID,@RESERVATIONID,@BSTATUS)", sqlCon);
            
            cmd.Parameters.AddWithValue("@BOOKINGID", id);
            cmd.Parameters.AddWithValue("@STARTDATE", string.Format("{0:yyyyMMdd}", start));
            cmd.Parameters.AddWithValue("@ENDDATE", string.Format("{0:yyyyMMdd}", end));
            cmd.Parameters.AddWithValue("@RESERVETIME", reservetime);
            cmd.Parameters.AddWithValue("@CONTRACTID", contractid);
            cmd.Parameters.AddWithValue("@ROOMTYPE", roomtype.ToString());
            cmd.Parameters.AddWithValue("@THISPRICE", thisprice);
            cmd.Parameters.AddWithValue("@ROOMID", null);
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

        internal List<IBooking> GetBookings(string reservationID)
        {
            connect();
            SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Booking WHERE RESERVATIONID = '" + reservationID + "'", sqlCon);
            
            List<IBooking> bookings = new List<IBooking>();
            try
            {
                SQLiteDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    bookings.Add(new Booking
                    (
                        rdr["BOOKINGID"].ToString(),
                        DateTime.ParseExact(rdr["STARTDATE"].ToString(), "yyyyMMdd", null),
                        DateTime.ParseExact(rdr["ENDDATE"].ToString(), "yyyyMMdd", null),
                        rdr["RESERVETIME"].ToString(),
                        rdr["CONTRACTID"].ToString(),
                        (RoomType)Enum.Parse(typeof(RoomType), (rdr["ROOMTYPE"].ToString())),
                        double.Parse(rdr["THISPRICE"].ToString()),
                        rdr["ROOMID"].ToString(),
                        rdr["RESERVATIONID"].ToString(),
                        (BookStatus)Enum.Parse(typeof(BookStatus), rdr["BSTATUS"].ToString())
                    ));
                }
                rdr.Close();
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
            SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Booking", sqlCon);

            List<IBooking> bookings = new List<IBooking>();
            try
            {
                SQLiteDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    bookings.Add(new Booking
                    (
                        rdr["BOOKINGID"].ToString(),
                        DateTime.ParseExact(rdr["STARTDATE"].ToString(), "yyyyMMdd", null),
                        DateTime.ParseExact(rdr["ENDDATE"].ToString(), "yyyyMMdd", null),
                        rdr["RESERVETIME"].ToString(),
                        rdr["CONTRACTID"].ToString(),
                        (RoomType)Enum.Parse(typeof(RoomType), (rdr["ROOMTYPE"].ToString())),
                        double.Parse(rdr["THISPRICE"].ToString()),
                        rdr["ROOMID"].ToString(),
                        rdr["RESERVATIONID"].ToString(),
                        (BookStatus)Enum.Parse(typeof(BookStatus), rdr["BSTATUS"].ToString())
                    ));
                }
                rdr.Close();
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

        internal IBooking UpdateBooking(IBooking booking)
        {
            connect();
            SQLiteCommand cmd = new SQLiteCommand("UPDATE Booking SET RESERVETIME=:RESERVETIME,CONTRACTID=:CONTRACTID,ROOMID=:ROOMID,BSTATUS=:BSTATUS WHERE BOOKINGID=:BOOKINGID", sqlCon);
            
            cmd.Parameters.AddWithValue("BOOKINGID", booking.ID);
            //cmd.Parameters.AddWithValue("STARTDATE", booking.StartDate);
            //cmd.Parameters.AddWithValue("ENDDATE", booking.EndDate);
            cmd.Parameters.AddWithValue("RESERVETIME", booking.ReserveTime);
            cmd.Parameters.AddWithValue("CONTRACTID", booking.ContractID);
            //cmd.Parameters.AddWithValue("ROOMTYPE", booking.Roomtype);
            //cmd.Parameters.AddWithValue("THISPRICE", booking.ThisPrice);
            cmd.Parameters.AddWithValue("ROOMID", booking.RoomID);
            //cmd.Parameters.AddWithValue("RESERVATIONID", booking.ReservationID);
            cmd.Parameters.AddWithValue("BSTATUS", booking.BStatus.ToString());

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Booking with ID: " + booking.ID + " could not be updated!\n" + ex.Message);
            }
            finally
            {
                disconnect();
            }
            return GetBooking(booking.ID);
        }

        #endregion

        #region Reservation Procedures

        internal IReservation GetReservation(string ReservationID)
        {
            connect();
            Reservation reservation = null;
            SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Reservation WHERE RESERVATIONID = '" + ReservationID + "'", sqlCon);
            
            try
            {
                SQLiteDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                    reservation = new Reservation
                        (
                            rdr["RESERVATIONID"].ToString(),
                            double.Parse(rdr["PAYMENT"].ToString()),
                            double.Parse(rdr["DOWNPAYMENT"].ToString()),
                            (ReservationStatus)Enum.Parse(typeof(ReservationStatus), rdr["RSTATUS"].ToString())
                        );
                rdr.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to parse/create reservation!\n" + ex.Message);
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
            SQLiteCommand cmd = new SQLiteCommand("INSERT INTO Reservation VALUES (@RESERVATIONID,@PAYMENT,@DOWNPAYMENT,@RSTATUS)", sqlCon);
            
            cmd.Parameters.AddWithValue("@RESERVATIONID", id);
            cmd.Parameters.AddWithValue("@PAYMENT", payment);
            cmd.Parameters.AddWithValue("@DOWNPAYMENT", downpayment);
            cmd.Parameters.AddWithValue("@RSTATUS", rstatus.ToString());

            try
            {
                cmd.ExecuteNonQuery();
                id = cmd.Parameters["@RESERVATIONID"].Value.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("Could not add reservation to database!\n" + ex.Message);
            }
            finally
            {
                disconnect();
            }
            return GetReservation(id);
        }

        internal List<IReservation> GetReservations()
        {
            connect();
            SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Reservation", sqlCon);
            
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
                rdr.Close();
                return reservations;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not parse reservations!\n" + ex.Message);
            }
            finally
            {
                disconnect();
            }
        }

        internal IReservation UpdateReservation(IReservation reservation)
        {
            connect();
            SQLiteCommand cmd = new SQLiteCommand("Update Reservation Set PAYMENT=:PAYMENT,DOWNPAYMENT=:DOWNPAYMENT,RSTATUS=:RSTATUS Where RESERVATIONID=:RESERVATIONID", sqlCon);
            
            cmd.Parameters.AddWithValue("RESERVATIONID", reservation.ID);
            cmd.Parameters.AddWithValue("PAYMENT", reservation.Payment);
            cmd.Parameters.AddWithValue("DOWNPAYMENT", reservation.DownPayment);
            cmd.Parameters.AddWithValue("RSTATUS", reservation.RStatus.ToString());
            
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Reservation with ID: " + reservation.ID + " could not be updated!\n" + ex.Message);
            }
            finally
            {
                disconnect();
            }
            return GetReservation(reservation.ID);
        }
        
        #endregion

        #region RoomPrice Procedures
        
        internal IRoomPrice GetRoomPrice(RoomType RType)
        {
            connect();
            RoomPrice roomprice = null;
            SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM RoomPrice WHERE RTYPE = '" + RType.ToString() + "'", sqlCon);
            
            try
            {
                SQLiteDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                    roomprice = new RoomPrice
                        (
                            (RoomType)Enum.Parse(typeof(RoomType), rdr["RTYPE"].ToString()),
                            double.Parse(rdr["RPRICE"].ToString())
                        );
                rdr.Close();
                return roomprice;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to parse/create new roomprice!\n" + ex.Message);
            }
            finally
            {
                disconnect();
            }
        }

        internal IRoomPrice CreateRoomPrice(RoomType rType, double price)
        {
            connect();
            SQLiteCommand cmd = new SQLiteCommand("INSERT INTO RoomPrice VALUES (@RTYPE,@RPRICE)", sqlCon);
            
            cmd.Parameters.AddWithValue("@RTYPE", rType.ToString());
            cmd.Parameters.AddWithValue("@RPRICE", price);
            
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Could not add roomprice to database!\n" + ex.Message);
            }
            finally
            {
                disconnect();
            }
            return GetRoomPrice(rType);
        }

        internal List<IRoomPrice> GetRoomPrices()
        {
            connect();
            SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM RoomPrice", sqlCon);
            
            List<IRoomPrice> roomprice = new List<IRoomPrice>();
            try
            {
                SQLiteDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    roomprice.Add(new RoomPrice
                    (
                        (RoomType)Enum.Parse(typeof(RoomType), rdr["BTYPE"].ToString()),
                        double.Parse(rdr["RPRICE"].ToString())
                    ));
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Could not parse roomprice!\n" + ex.Message);
            }
            finally
            {
                disconnect();
            }
            return roomprice;
        }

        internal IRoomPrice UpdateRoomPrice(IRoomPrice roomprice)
        {
            connect();
            SQLiteCommand cmd = new SQLiteCommand("Update RoomPrice Set RPRICE=:RPRICE Where RTYPE=:RTYPE", sqlCon);

            cmd.Parameters.AddWithValue("RTYPE", roomprice.RType.ToString());
            cmd.Parameters.AddWithValue("RPRICE", roomprice.Price);
            
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Room Price of " + roomprice.RType + " could not be updated!\n" + ex.Message);
            }
            finally
            {
                disconnect();
            }
            return GetRoomPrice(roomprice.RType);
        }

        #endregion

        #region IClock Procedures

        /// <summary>
        /// return true if gets clock from database
        /// </summary>
        /// <returns>true if gets clock</returns>
        internal bool GetClock()
        {
            connect();
            SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Clock", sqlCon);
            SQLiteDataReader rdr = cmd.ExecuteReader();

            try
            {
                if (rdr.Read())
                {
                    IClock.SetClock(
                        DateTime.FromFileTime(long.Parse(rdr["TIME"].ToString())),
                        int.Parse(rdr["COUNTROOM"].ToString()),
                        int.Parse(rdr["COUNTCUSTOMER"].ToString()),
                        int.Parse(rdr["COUNTBOOKING"].ToString()),
                        int.Parse(rdr["COUNTRESERVE"].ToString())
                    );
                }
                else return false;
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to parse/create new clock!\n" + ex.Message);
            }
            finally
            {
                rdr.Close();
                disconnect();
            }
        }

        internal void CreateClock()
        {
            connect();
            SQLiteCommand cmd = new SQLiteCommand("INSERT INTO Clock" +
                " VALUES (@TIME,@COUNTROOM,@COUNTCUSTOMER,@COUNTBOOKING,@COUNTRESERVE)", sqlCon);

            cmd.Parameters.AddWithValue("@TIME", IClock.Time.ToFileTime());
            cmd.Parameters.AddWithValue("@COUNTROOM", IClock.CountRoom);
            cmd.Parameters.AddWithValue("@COUNTCUSTOMER", IClock.CountCustomer);
            cmd.Parameters.AddWithValue("@COUNTBOOKING", IClock.CountBooking);
            cmd.Parameters.AddWithValue("@COUNTRESERVE", IClock.CountReserv);
            
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Could not add Clock to database!\n" + ex.Message);
            }
            finally
            {
                disconnect();
            }
        }

        internal void UpdateClock()
        {
            connect();
            SQLiteCommand cmd = new SQLiteCommand("Update Clock Set TIME=:TIME,COUNTROOM=:COUNTROOM,COUNTCUSTOMER=:COUNTCUSTOMER," +
               "COUNTBOOKING=:COUNTBOOKING,COUNTRESERVE=:COUNTRESERVE", sqlCon);

            cmd.Parameters.AddWithValue("TIME", IClock.Time.ToFileTime());
            cmd.Parameters.AddWithValue("COUNTROOM", IClock.CountRoom);
            cmd.Parameters.AddWithValue("COUNTCUSTOMER", IClock.CountCustomer);
            cmd.Parameters.AddWithValue("COUNTBOOKING", IClock.CountBooking);
            cmd.Parameters.AddWithValue("COUNTRESERVE", IClock.CountReserv);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Clock could not be updated!\n" + ex.Message);
            }
            finally
            {
                disconnect();
            }
        }
        #endregion

        #region Log Procedures

        internal void CreateLog(string logtext)
        {
            connect();
            SQLiteCommand cmd = new SQLiteCommand("INSERT INTO Log VALUES (@DATETIME, @LOGTEXT)", sqlCon);
            cmd.Parameters.AddWithValue("@DATETIME", IClock.Time.ToFileTime());
            cmd.Parameters.AddWithValue("@LOGTEXT", logtext);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Could not add Log to database!\n" + ex.Message);
            }
            finally
            {
                disconnect();
            }
        }

        internal List<Log> GetLogs()
        {
            connect();
            SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Log", sqlCon);
            SQLiteDataReader rdr = cmd.ExecuteReader();

            List<Log> logs = new List<Log>();
            try
            {
                while (rdr.Read())
                {
                    logs.Add(new Log(
                            DateTime.FromFileTime(long.Parse(rdr["DATETIME"].ToString())),
                            rdr["LOGTEXT"].ToString()
                        ));
                }
                return logs;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to parse/create new log!\n" + ex.Message);
            }
            finally
            {
                rdr.Close();
                disconnect();
            }
        }
        
        #endregion
    }
}
