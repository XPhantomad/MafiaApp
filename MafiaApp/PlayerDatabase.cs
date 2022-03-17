using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MafiaApp
{
    public class PlayerDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public PlayerDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<PlayerItem>().Wait();
        }

        public Task<List<PlayerItem>> GetPlayersAsync()
        {
            return _database.Table<PlayerItem>().ToListAsync();
        }

        public Task<List<PlayerItem>> GetPlayersPresentAliveAsync()
        {
            return _database.QueryAsync<PlayerItem>("SELECT * FROM [PlayerItem] WHERE Present = true AND Lives >= 1");
        }

        public Task<List<PlayerItem>> GetPlayersPresentByRoleAsync(Roles role)
        {
            return _database.QueryAsync<PlayerItem>("SELECT * FROM [PlayerItem] WHERE Present = true AND Role = ?", role);
        }
        public Task<List<PlayerItem>> GetPlayersPresentAliveByRoleAsync(Roles role)
        {
            return _database.QueryAsync<PlayerItem>("SELECT * FROM [PlayerItem] WHERE Present = true AND Lives >= 1 AND Role = ?", role);
        }

        public Task<int> SavePlayerAsync(PlayerItem player)
        {
            return _database.InsertAsync(player);
        }

        public Task<int> UpdatePlayerAsync(PlayerItem player)
        {
            return _database.UpdateAsync(player);
        }

        public Task<PlayerItem> GetPlayerAsync(string name)
        {
            //List<PlayerItem> players = await _database.Table<PlayerItem>().ToListAsync();
            //foreach (PlayerItem aPlayer in players)
            //{
            //    if (aPlayer.Name.Equals(name))
            //    {
            //        return true;
            //    }
            //}
            return _database.Table<PlayerItem>().Where(p => p.Name == name).FirstOrDefaultAsync(); ;
        }

        public Task<int> DeletePlayerAsync(PlayerItem player)
        {
            return _database.DeleteAsync(player);
        }
    }
}
