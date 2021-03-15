using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace MafiaApp //vorher .Daten
{
    public class MafiaItemDatabase
    {
        static SQLiteAsyncConnection Database;

        public static readonly AsyncLazyusw<MafiaItemDatabase> Instance = new AsyncLazyusw<MafiaItemDatabase>(async()=>
        {
           var inst = new MafiaItemDatabase();
           CreateTableResult result = await Database.CreateTableAsync<PlayerItem>();
           return inst;
        
        });

        public MafiaItemDatabase()
        {
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        }

        // Daten Methoden
        // Für PlayerPage
        public Task<int> SavePlayerAsync(string player)
        {
            PlayerItem item = new PlayerItem();
            item.Name = player;
            item.Role = roles.None;             
            item.Present = true;

            if (item.ID != 0)
                return Database.UpdateAsync(item);
            else
                return Database.InsertAsync(item);
        }

        public Task<List<PlayerItem>> GetPlayersAsync()
        {
            return Database.Table<PlayerItem>().ToListAsync();
        }

        public Task<int> DeletePlayerAsync(PlayerItem item)
        {
            return Database.DeleteAsync(item);      
        }

        public Task<int> ChangePlayerPresentAsync(PlayerItem item)
        {
            item.Present = !(item.Present);
            if (item.ID != 0)
                return Database.UpdateAsync(item);
            else
                return Database.InsertAsync(item);
        }

        public Task<List<PlayerItem>> GetSortedPlayersAsync(bool ascending)      //ascending = ausfsteigend
        {
            if (ascending == true)
            {
                return Database.QueryAsync<PlayerItem>("SELECT * FROM [PlayerItem] ORDER BY Name ASC");
            }
            else
            {
                return Database.QueryAsync<PlayerItem>("SELECT * FROM [PlayerItem] ORDER BY Name DESC");
            }
        }

        public Task<List<PlayerItem>> GetPlayersPresentAsync()
        {
            return Database.QueryAsync<PlayerItem>("SELECT * FROM [PlayerItem] WHERE Present = true ");
        }

        // Für StartGamePage

        public async Task<string[]> GetPlayersNoRoleAndPresentAsync()
        {
            List<PlayerItem> names = await Database.QueryAsync<PlayerItem>("SELECT Name FROM [PlayerItem] WHERE Present = true");  // keine Rolle noch hinzufügen
            string[] nameList = new string[8];
            int i = 0;
            foreach (PlayerItem aPlayerItem in names)  // vielleicht nich bessere Typkonvertierung finden
            {
                nameList[i] = aPlayerItem.Name;
                i++;
            }
            return nameList;
        }

        public async Task<int> SetPlayersRoleAsync(string name, roles role)  // besser Namens Array Übergeben
        {
            // nur mit await funtkioniert der ganze Hase
            // Error hinzufügen, falls kein Name oder Rolle ankommt
            PlayerItem player = await Database.Table<PlayerItem>().Where(p => p.Name == name).FirstOrDefaultAsync();
            player.Role = role;
            return await Database.UpdateAsync(player);           
        }





        // altes Zeug
        public Task<List<PlayerItem>> GetItemsAsync()
        {
            return Database.Table<PlayerItem>().ToListAsync();
        }

        public Task<List<PlayerItem>> GetItemsNotDoneAsync()
        {
            // SQL queries are also possible
            return Database.QueryAsync<PlayerItem>("SELECT * FROM [PlayerItem] WHERE Done = 0");
        }

        public Task<PlayerItem> GetItemAsync(int id)
        {
            return Database.Table<PlayerItem>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(PlayerItem item)
        {
            if (item.ID != 0)
                return Database.UpdateAsync(item);
            else
                return Database.InsertAsync(item);
        }
        public Task<int> DeleteItemAsync(PlayerItem item)
        {
            return Database.DeleteAsync(item);
        }
    }
}
