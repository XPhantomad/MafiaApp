﻿using SQLite;
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
            item.Spouse = "None";
            item.Alive = true;
            item.Victim = false;

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
            List<PlayerItem> names = await Database.QueryAsync<PlayerItem>("SELECT Name FROM [PlayerItem] WHERE Present = true AND Role = roles.None");  // keine Rolle noch hinzufügen
            string[] nameList = new string[8];          // achtung nur 8
            int i = 0;
            foreach (PlayerItem aPlayerItem in names)  // vielleicht nich bessere Typkonvertierung finden
            {
                nameList[i] = aPlayerItem.Name;
                i++;
            }
            return nameList;
        }

        public async Task<int> SetPlayersRoleAsync(string name, roles role)  
        {
            // nur mit await funtkioniert der ganze Hase
            // Error hinzufügen, falls kein Name oder Rolle ankommt
            PlayerItem player = await Database.Table<PlayerItem>().Where(p => p.Name == name).FirstOrDefaultAsync();
            player.Role = role;
            return await Database.UpdateAsync(player);           
        }

        public async Task<string[]> GetPlayersUnmarriedAsync(string spouse1)
        {
            List<PlayerItem> names = await Database.QueryAsync<PlayerItem>("SELECT Name FROM [PlayerItem] WHERE Present = true AND Spouse = None");  
            string[] nameList = new string[8];          // auf die 8 aufpassen das geht nicht gut  
            // besser vielleicht alle Spieler nach IDs einzeln auslesen und dann in Array einfügen 
            // oder maxID irgendwie rausbekommen
            int i = 0;
            foreach (PlayerItem aPlayerItem in names)  // vielleicht nich bessere Typkonvertierung finden
            {
                if (!aPlayerItem.Name.Equals(spouse1))  // bereits gesetzten Ehepartner aussortieren
                {
                    nameList[i] = aPlayerItem.Name;
                    i++;
                }
            }
            return nameList;
        }

        public async Task<int> SetPlayerSpouseAsync(string s1, string s2)
        {
            PlayerItem spouse1 = await Database.Table<PlayerItem>().Where(p => p.Name == s1).FirstOrDefaultAsync();
            PlayerItem spouse2 = await Database.Table<PlayerItem>().Where(p => p.Name == s2).FirstOrDefaultAsync();
            spouse1.Spouse = s2;
            spouse2.Spouse = s1;
            return await Database.UpdateAsync(spouse1);     //nicht wirklich aussagekräftig
        }
        


        /*
         * falls man das mal braucht
        public async void XXXXXSetPlayersRoleAsync(string[] name, roles role)
        {
            // nur mit await funtkioniert der ganze Hase
            // Error hinzufügen, falls kein Name oder Rolle ankommt

            foreach (string aName in name)  // das geht schöner!!!!
            {
                PlayerItem player = await Database.Table<PlayerItem>().Where(p => p.Name == aName).FirstOrDefaultAsync();
                player.Role = role;
                await Database.UpdateAsync(player);
            }
            return;
        }
        */





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
