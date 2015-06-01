using Interface;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        internal void DatabaseController()
        {
            string dbPath = "Data Source =" + Environment.CurrentDirectory + "/HotelDataBase.db";
            sqlCon = new SQLiteConnection(dbPath);
        }

        #endregion


    }
}
