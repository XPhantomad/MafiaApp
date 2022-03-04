using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MafiaApp
{
    public static class GameManagement
    {
        // returns List of Playernames with number Elements, Fills up with Keiner
        public static async Task<List<string>> GetPlayerNamesAsync(Roles role, int number)
        {
            List<PlayerItem> player = await App.PlayerDatabase.GetPlayersByRoleAsync(role);
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
            List<PlayerItem> player = await App.PlayerDatabase.GetPlayersByRoleAsync(role);

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
                await App.PlayerDatabase.UpdatePlayerAsync(player);
                return 1;
            }
            return 0;
                        
        }

        public static async Task<int> ResetGameAsync()
        {
            //Liebespaar, Rollen, Leben, Fähigkeiten
            return 1;
        }
    }
}
