using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Wel.SharedLibrary
{
    public class Const
    {
        public const string REQUEST_HEADER_ACCEPT = "application/json";
        public const string MSG_ERROR_PREFIX = "Error:";
        public const string server = "http://api-test.welverdiendforestry.co.za";

       

        public const string DatabaseFilename = "Welverdiend.db3";

       

        public const SQLite.SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath =>
            Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);

        //public SQLiteAsyncConnection Database = new SQLiteAsyncConnection(Const.DatabasePath, Const.Flags);
    }
}
