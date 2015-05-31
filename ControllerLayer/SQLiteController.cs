using Interface;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        internal void DatabaseController(string user, string pass)
        {
            string dbPath = "Data Source =" + Environment.CurrentDirectory + "/HotelDataBase.db";
            sqlCon = new SQLiteConnection(dbPath);
        }

        #endregion


    }
}
