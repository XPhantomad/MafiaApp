using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MafiaApp
{
    public static class GameManagement
    {
        public static async Task<List<PlayerItem>> GetPlayersAsync(Roles role, int number)
        {
            List<PlayerItem> player = await App.PlayerDatabase.GetPlayersPresentAliveByRoleAsync(role);
            int n = player.Count;
            if (n > number)
            {
                //TODO:  to much players with this Role --> reset all 
                return new List<PlayerItem>();
            }
            // Fill up with "Keiner"
            for (int i = 0; i < (number - n); i++)
            {
                player.Add(new PlayerItem { Name = "Keiner"});
            }
            return player;
        }
        // returns List of Playernames with number Elements, Fills up with Keiner
        public static async Task<List<string>> GetPlayerNamesAsync(Roles role, int number)
        {
            List<PlayerItem> player = await App.PlayerDatabase.GetPlayersPresentAliveByRoleAsync(role);
            int n = player.Count;
            if (n > number)
            {
                //TODO:  to much players with this Role --> reset all 
               return new List<string>{ "Error", "Error"};
            }
            List<string> result = new List<string>();
            foreach (PlayerItem aPlayerItem in player)
            {
                result.Add(aPlayerItem.Name);
            }
            // Fill up with "Keiner"
            for(int i=0; i<(number-n); i++)
            {
                result.Add("Keiner");
            }
            return result;
        }
        public static async Task<List<string>> GetPlayerNamesAsync(Roles role)
        {
            List<PlayerItem> player = await App.PlayerDatabase.GetPlayersPresentAliveByRoleAsync(role);

            List<string> result = new List<string>();
            foreach (PlayerItem aPlayerItem in player)
            {
                result.Add(aPlayerItem.Name);
            }
            return result;
        }
        public static async Task<List<string>> GetPlayerNamesAsync()
        {
            List<PlayerItem> player = await App.PlayerDatabase.GetPlayersPresentAliveAsync();

            List<string> result = new List<string>();
            foreach (PlayerItem aPlayerItem in player)
            {
                result.Add(aPlayerItem.Name);
            }
            return result;
        }

        public static async Task<int> SetPlayersRoleAsync(string name, Roles role)
        {
            PlayerItem player = await App.PlayerDatabase.GetPlayerAsync(name);
            if(player != null)
            {
                player.Role = role;
                if (!role.Equals(Roles.None))
                {
                    player.Lives = (await App.RolesDatabase.GetRoleAsync(role)).Lives;
                }
                await App.PlayerDatabase.UpdatePlayerAsync(player);
                return 1;
            }
            return 0;           
        }
        public static async Task<int> SetPlayersSpouseAsync(string name1, string name2)
        {
            // Reset all Spouse
            List<PlayerItem> player = await App.PlayerDatabase.GetPlayersPresentAliveAsync();

            List<string> result = new List<string>();
            foreach (PlayerItem aPlayerItem in player)
            {
                aPlayerItem.Spouse = null;
                await App.PlayerDatabase.UpdatePlayerAsync(aPlayerItem);
            }

            // Set new Spouse
            PlayerItem spouse1 = await App.PlayerDatabase.GetPlayerAsync(name1);
            PlayerItem spouse2 = await App.PlayerDatabase.GetPlayerAsync(name2);
            if(spouse1 != null)
            {
                spouse1.Spouse = name1;
                await App.PlayerDatabase.UpdatePlayerAsync(spouse1);
            }
            if (spouse2 != null)
            {
                spouse2.Spouse = name2;
                await App.PlayerDatabase.UpdatePlayerAsync(spouse2);
            }
            return 1;
        }

        public static async Task<int> ResetGameAsync()
        {
            //Liebespaar, Rollen, Leben, Fähigkeiten
            return 1;
        }
    }
}
