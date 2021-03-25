using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MafiaApp
{
    public static class Constants
    {
        public const string DatabaseFilename1 = "MafiaSQLite.db3";
        public const string DatabaseFilename2 = "RolesSQLite.db3";
        public const SQLite.SQLiteOpenFlags Flags =
             // open the database in read/write mode
             SQLite.SQLiteOpenFlags.ReadWrite |
             // create the database if it doesn't exist
             SQLite.SQLiteOpenFlags.Create |
             // enable multi-threaded database access
             SQLite.SQLiteOpenFlags.SharedCache;


        public static string DatabasePath1
        {
            get
            {
                var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(basePath, DatabaseFilename1);
            }
        }
        public static string DatabasePath2
        {
            get
            {
                var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(basePath, DatabaseFilename2);
            }
        }
    }
}
