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
            CreateTableResult result1 = await Database.CreateTableAsync<RolesItem>();
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
            item.Lives = 1;

            if (item.ID != 0)
                return Database.UpdateAsync(item);
            else
                return Database.InsertAsync(item);
        }

        public Task<List<PlayerItem>> GetPlayersAsync()
        {
            return Database.Table<PlayerItem>().ToListAsync();
        }
        public Task<List<PlayerItem>> GetPlayersByRoleAsync(roles role)
        {
            return Database.Table<PlayerItem>().Where(r => r.Role == role).ToListAsync();
        }

        public Task<int> DeletePlayerAsync(PlayerItem item)
        {
            return Database.DeleteAsync(item);      
        }

        public Task<int> ChangePlayerPresentAsync(PlayerItem item)          // dabei am besten noch alle Spieldaten Rolle usw löschen
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
            List<PlayerItem> names = await Database.QueryAsync<PlayerItem>("SELECT Name FROM [PlayerItem] WHERE Present = true AND Role = ?", roles.None);
            //Task<int> n = Database.Table<PlayerItem>().Where(r => r.Present == true && r.Role == roles.None).CountAsync();
            int n = names.Count; 
            string[] nameList = new string[n]; 
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

            // Leben für Spieler setzen
            PlayerItem player = await Database.Table<PlayerItem>().Where(p => p.Name == name).FirstOrDefaultAsync();
            if (player != null)
            {
                player.Role = role;
            }
            return await Database.UpdateAsync(player);           
        }

        public async Task<string[]> GetPlayersMarriedAsync(string spouse1, bool married)
        {
            if (married == false)
            {
                List<PlayerItem> names = await Database.QueryAsync<PlayerItem>("SELECT Name FROM [PlayerItem] WHERE Present = true");
                int n = names.Count;
                string[] nameList = new string[n];
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
            else
            {
                string s = "None";
                List<PlayerItem> names = await Database.QueryAsync<PlayerItem>("SELECT Name FROM [PlayerItem] WHERE Present = true AND Spouse != ?", s);
                int n = names.Count;
                string[] nameList = new string[n];
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
        }

        public async Task<int> SetPlayerSpouseAsync(string s1, string s2)
        {
            PlayerItem spouse1 = await Database.Table<PlayerItem>().Where(p => p.Name == s1).FirstOrDefaultAsync();
            PlayerItem spouse2 = await Database.Table<PlayerItem>().Where(p => p.Name == s2).FirstOrDefaultAsync();
            spouse1.Spouse = s2;
            spouse2.Spouse = s1;
            return await Database.UpdateAsync(spouse1);     //nicht wirklich aussagekräftig
        }
        public async Task<int> SetPlayerNotSpouseAsync(string s1, string s2)
        {
            PlayerItem spouse1 = await Database.Table<PlayerItem>().Where(p => p.Name == s1).FirstOrDefaultAsync();
            PlayerItem spouse2 = await Database.Table<PlayerItem>().Where(p => p.Name == s2).FirstOrDefaultAsync();
            if (spouse1 != null && spouse2 != null)
            {
                spouse1.Spouse = "None";
                spouse2.Spouse = "None";
            }
            return await Database.UpdateAsync(spouse1);     //nicht wirklich aussagekräftig
        }

        public async Task<string[]> GetPlayersByRoleAndNumberAsync(roles role, int anzahl)
        {
            List<PlayerItem> names = await Database.QueryAsync<PlayerItem>("SELECT Name FROM [PlayerItem] WHERE Present = true AND Role = ?", role);
            int n = names.Count;
            if (n > anzahl)
            {
                string[] err = new string[] { "Error", "Error" };
                return err;
            }
            string[] nameList = new string[anzahl];
            int i = 0;
            foreach (PlayerItem aPlayerItem in names)  
            {
                nameList[i] = aPlayerItem.Name;
                i++;
            }
            while (i < anzahl)
            {
                nameList[i] = "Keiner";
                i++;
            }
            return nameList;
        }

        public async Task<string[]> GetPlayersPresentAndAliveAsync()
        {
            List<PlayerItem> names = await Database.QueryAsync<PlayerItem>("SELECT Name FROM [PlayerItem] WHERE Present = true AND Lives > 0");
            int n = names.Count;
            string[] nameList = new string[n];
            int i = 0;
            foreach (PlayerItem aPlayerItem in names)
            {
                nameList[i] = aPlayerItem.Name;
                i++;
            }
            return nameList;
        }

        public async Task<int> SetPlayerLivesAsync(string playerName, double number)
        {
            PlayerItem player = await Database.Table<PlayerItem>().Where(p => p.Name == playerName).FirstOrDefaultAsync();
            if (player != null)
            {
                player.Lives = player.Lives + number;
            }
            return await Database.UpdateAsync(player);
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





        // Rollen Tabelle

        public async Task<int> SetUp()
        {
            RolesItem test = await Database.Table<RolesItem>().Where(p => p.Role == roles.Amor).FirstOrDefaultAsync();
            if (test == null)
            {
                RolesItem r = new RolesItem();
                r.Role = roles.Amor;
                r.Number = 1;
                await Database.InsertAsync(r);
            }
            test = await Database.Table<RolesItem>().Where(p => p.Role == roles.Mafia).FirstOrDefaultAsync();
            if (test == null)
            {
                RolesItem r = new RolesItem();
                r.Role = roles.Mafia;
                r.Number = 2;
                await Database.InsertAsync(r);
            }
            test = await Database.Table<RolesItem>().Where(p => p.Role == roles.Hexe).FirstOrDefaultAsync();
            if (test == null)
            {
                RolesItem r = new RolesItem();
                r.Role = roles.Hexe;
                r.Number = 1;
                await Database.InsertAsync(r);
            }
            test = await Database.Table<RolesItem>().Where(p => p.Role == roles.Detektiv).FirstOrDefaultAsync();
            if (test == null)
            {
                RolesItem r = new RolesItem();
                r.Role = roles.Detektiv;
                r.Number = 1;
                await Database.InsertAsync(r);
            }
            test = await Database.Table<RolesItem>().Where(p => p.Role == roles.Bürger).FirstOrDefaultAsync();
            if (test == null)
            {
                RolesItem r = new RolesItem();
                r.Role = roles.Bürger;
                r.Number = 2;
                await Database.InsertAsync(r);
            }
            return 1;
        }

        public async Task<int> SetRoleNumbersAuto(int player)
        {
            int m;
            switch (player)
            {
                case 5:
                    m = 1;
                    break;
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                    m = 2;
                    break;
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                    m = 3;
                    break;
                case 16:
                case 17:
                case 18:
                case 19:
                case 20:
                    m = 4;
                    break;
                default:
                    return 0; // ERROR sollte nicht auftreten, weil vorher schon presentplayerauf >5 untersucht wird
            }

            RolesItem buerger = await Database.Table<RolesItem>().Where(p => p.Role == roles.Bürger).FirstOrDefaultAsync();
            buerger.Number = player - m - 3;   // 3 wegen Hexe, Detektiv, Amor  andere Rollen im Auomatikmodus nicht berücksichtigt
            await Database.UpdateAsync(buerger);
            RolesItem mafia = await Database.Table<RolesItem>().Where(p => p.Role == roles.Mafia).FirstOrDefaultAsync();
            mafia.Number = m;
            await Database.UpdateAsync(mafia);
            return 1;
        }


        public Task<int> DeleteRolesTableAsync(RolesItem r)
        {
            return Database.DeleteAsync(r);
        }

        public Task<List<RolesItem>> GetRolesAsync()
        {
            return Database.Table<RolesItem>().ToListAsync();
        }

        public async Task<int> SetRoleNumbersManual(roles r, bool increase)
        {
            RolesItem role = await Database.Table<RolesItem>().Where(p => p.Role == r).FirstOrDefaultAsync();
            RolesItem buerger = await Database.Table<RolesItem>().Where(p => p.Role == roles.Bürger).FirstOrDefaultAsync();

            if (increase == true)
            {
                role.Number++;
                buerger.Number--;
            }       
            else
            {
                role.Number--;
                buerger.Number++;
            }
            await Database.UpdateAsync(role);
            await Database.UpdateAsync(buerger);
            return 1;
        }

        public async Task<int> GetRoleNumber(roles r)
        {
            RolesItem role = await Database.Table<RolesItem>().Where(p => p.Role == r).FirstOrDefaultAsync();
            return role.Number;   //besser Array von der Datenbank Spalte zurückgeben
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
