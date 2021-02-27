using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MafiaApp
{
    public static class Constants
    {
        public const string DatabaseFilename = "MafiaSQLite.db3";
        public const SQLite.SQLiteOpenFlags Flags = SQLite.SQLiteOpenFlags.Create;
        
        public static string DatabasePath
        {
            get
            {
                var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(basePath, DatabaseFilename);
            }
        }
    }
}
