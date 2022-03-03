using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MafiaApp
{
    public class RolesDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public RolesDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<RolesItem>().Wait();
        }

        public Task<List<RolesItem>> GetRolesAsync()
        {
            return _database.Table<RolesItem>().ToListAsync();
        }

        public Task<RolesItem> GetRoleAsync(Roles role)
        {
           return _database.Table<RolesItem>().Where(p => p.Role == role).FirstOrDefaultAsync();
        }

        public Task<int> SaveRoleAsync(RolesItem role)
        {
            return _database.InsertAsync(role);
        }

        public Task<int> UpdateRolesAsync(RolesItem role)
        {
            return _database.UpdateAsync(role);
        }
        public Task<int> DeleteRoleAsync(RolesItem role)
        {
            return _database.DeleteAsync(role);
        }
    }
}
