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
        public Task<int> SavePlayerAsync(PlayerItem player)
        {
            return _database.InsertAsync(player);
        }

        public Task<int> UpdatePlayerAsync(PlayerItem player)
        {
            return _database.UpdateAsync(player);
        }

        public async Task<bool> ContainsPlayerAsync(string name)
        {
            List<PlayerItem> players = await _database.Table<PlayerItem>().ToListAsync();
            foreach (PlayerItem aPlayer in players)
            {
                if (aPlayer.Name.Equals(name))
                {
                    return true;
                }
            }
            return false;
        }

        public Task<int> DeletePlayerAsync(PlayerItem player)
        {
            return _database.DeleteAsync(player);
        }
    }
}
