using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace MafiaApp.Daten
{
    public class RolesItemDatabase
    {
        static SQLiteAsyncConnection Database;

        public static readonly AsyncLazyusw<RolesItemDatabase> Instance = new AsyncLazyusw<RolesItemDatabase>(async () =>
        {
            var inst = new RolesItemDatabase();
            CreateTableResult result = await Database.CreateTableAsync<PlayerItem>();
            return inst;

        });


        public RolesItemDatabase()
        {
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        }



    }
}
