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
            List<PlayerItem> player = await App.PlayerDatabase.GetPlayersPresentByRoleAsync(role);
            int n = player.Count;
            if (n > number)
            {
                //TODO:  to much players with this Role --> reset all 
                return new List<PlayerItem>();
            }
            // Fill up with "Keiner"
            for (int i = 0; i < (number - n); i++)
            {
                player.Add(new PlayerItem { Name = "Keiner" });
            }
            return player;
        }
        //// returns List of Playernames with number Elements, Fills up with Keiner
        //public static async Task<List<string>> GetPlayerNamesAsync(Roles role, int number)
        //{
        //    List<PlayerItem> player = await App.PlayerDatabase.GetPlayersPresentAliveByRoleAsync(role);
        //    int n = player.Count;
        //    if (n > number)
        //    {
        //        //TODO:  to much players with this Role --> reset all 
        //       return new List<string>{ "Error", "Error"};
        //    }
        //    List<string> result = new List<string>();
        //    foreach (PlayerItem aPlayerItem in player)
        //    {
        //        result.Add(aPlayerItem.Name);
        //    }
        //    // Fill up with "Keiner"
        //    for(int i=0; i<(number-n); i++)
        //    {
        //        result.Add("Keiner");
        //    }
        //    return result;
        //}
        public static async Task<List<string>> GetPlayerNamesAsync(Roles role)
        {
            List<PlayerItem> player = await App.PlayerDatabase.GetPlayersPresentByRoleAsync(role);

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
            if (player != null)
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
        public static async Task<int> SetPlayersRoleAsync(List<string> names, Roles role)
        {
            foreach (string aName in names)
            {
                await SetPlayersRoleAsync(aName, role);
            }
            return 1;
        }
        public static async Task<int> SetPlayersSpouseAsync(string name1, string name2)
        {
            // Reset all Spouse
            List<PlayerItem> player = await App.PlayerDatabase.GetPlayersPresentAliveAsync();

            foreach (PlayerItem aPlayerItem in player)
            {
                aPlayerItem.Spouse = null;
                await App.PlayerDatabase.UpdatePlayerAsync(aPlayerItem);
            }

            // Set new Spouse
            PlayerItem spouse1 = await App.PlayerDatabase.GetPlayerAsync(name1);
            PlayerItem spouse2 = await App.PlayerDatabase.GetPlayerAsync(name2);
            if (spouse1 != null)
            {
                spouse1.Spouse = name2;
                await App.PlayerDatabase.UpdatePlayerAsync(spouse1);
            }
            if (spouse2 != null)
            {
                spouse2.Spouse = name1;
                await App.PlayerDatabase.UpdatePlayerAsync(spouse2);
            }
            return 1;
        }

        public static async Task<int> SetAccomodationAsync(string accomodationName)
        {
            // reset all accomodates
            List<PlayerItem> players = await App.PlayerDatabase.GetPlayersPresentAliveAsync();
            foreach (PlayerItem aPlayerItem in players)
            {
                aPlayerItem.Accomodates = false;
                await App.PlayerDatabase.UpdatePlayerAsync(aPlayerItem);
            }

            // set new Accomodation
            if (accomodationName != null) {
                if (!accomodationName.Equals("Brücke"))
                {
                    PlayerItem player = await App.PlayerDatabase.GetPlayerAsync(accomodationName);
                    if (player.Role != Roles.Penner)
                    {
                        player.Accomodates = true;
                    }
                    await App.PlayerDatabase.UpdatePlayerAsync(player);
                }
            }
            return 1;
        }

        private static async Task<bool> GetAccomodationAsync()
        {
            bool result = false;
            List<PlayerItem> player = await App.PlayerDatabase.GetPlayersPresentAliveAsync();
            foreach (PlayerItem aPlayerItem in player)
            {
                if (aPlayerItem.Accomodates)
                {
                    result = true;
                }
            }
            return result;
        }

        private static async Task<HashSet<string>> KillPennerAsync()
        {
            HashSet<string> result = new HashSet<string>();

            List<PlayerItem> player = await App.PlayerDatabase.GetPlayersPresentAliveAsync();
            foreach (PlayerItem aPlayerItem in player)
            {
                if (aPlayerItem.Role.Equals(Roles.Penner))
                {
                    aPlayerItem.Lives = 0;
                    result.Add(aPlayerItem.Name);
                    if (aPlayerItem.Spouse != null)
                    {
                        result.Add(aPlayerItem.Spouse);
                        PlayerItem spouse = await App.PlayerDatabase.GetPlayerAsync(aPlayerItem.Spouse);
                        spouse.Lives = 0;
                        await App.PlayerDatabase.UpdatePlayerAsync(spouse);
                    }
                    await App.PlayerDatabase.UpdatePlayerAsync(aPlayerItem);
                }
            }
            return result;
        }

        // Decrease Playes lives, return name only if player dies
        private static async Task<string> DecreasePlayersLiveAsync(string name)
        {
            PlayerItem player = await App.PlayerDatabase.GetPlayerAsync(name);
            if (!player.Role.Equals(Roles.Penner) || !await GetAccomodationAsync())
            {
                player.Lives -= 1;
                await App.PlayerDatabase.UpdatePlayerAsync(player);
            }
            return player.Lives <= 0 ? name : null;
        }


        //public static async Task<HashSet<string>> GetSetPlayersDeathAsync(HashSet<string> playerNames)
        //{
        //    HashSet<string> result = new HashSet<string>();
        //    foreach (string aName in playerNames)
        //    {
        //        PlayerItem player = await App.PlayerDatabase.GetPlayerAsync(aName);
        //        if (!player.Role.Equals(Roles.Penner))
        //        {
        //            player.Lives -= 1;
        //            if (player.Spouse != null)
        //            {
        //                result.Add(player.Spouse);
        //                PlayerItem spouse = await App.PlayerDatabase.GetPlayerAsync(player.Spouse);
        //                spouse.Lives -= 1;
        //                if (spouse.Accomodates == true)
        //                {
        //                    result.UnionWith(await KillPennerAsync());
        //                }
        //                await App.PlayerDatabase.UpdatePlayerAsync(spouse);
        //            }
        //            if (player.Accomodates == true)
        //            {
        //                result.UnionWith(await KillPennerAsync());
        //            }
        //            await App.PlayerDatabase.UpdatePlayerAsync(player);
        //        }
        //        if (player.Role.Equals(Roles.Penner) && (await GetAccomodationAsync()) == false)
        //        {
        //            result.UnionWith(await KillPennerAsync());
        //        }
        //    }
        //    result.UnionWith(playerNames);
        //    return result;
        //}
        public static async Task<HashSet<string>> GetSetPlayersDeathNightAsync(HashSet<string> playerNames)
        {
            HashSet<string> deadPlayers = new HashSet<string>();
            foreach (string aName in playerNames) 
            {
                string name = await DecreasePlayersLiveAsync(aName);
                if (name != null) 
                {
                    deadPlayers.Add(name);
                }
            }
            HashSet<string> result = new HashSet<string>();

            foreach (string aName in deadPlayers)
            {
                result.Add(aName);
                PlayerItem player = await App.PlayerDatabase.GetPlayerAsync(aName);
                // Spouse
                if (player.Spouse != null)
                {
                    result.Add(player.Spouse);
                    PlayerItem spouse = await App.PlayerDatabase.GetPlayerAsync(player.Spouse);
                    spouse.Lives = 0;
                    // Spouse Accomodates
                    if (spouse.Accomodates)
                    {
                        result.UnionWith(await KillPennerAsync());
                    }
                    await App.PlayerDatabase.UpdatePlayerAsync(spouse);
                }
                // Accomodates
                if (player.Accomodates)
                {
                    result.UnionWith(await KillPennerAsync());
                }
                await App.PlayerDatabase.UpdatePlayerAsync(player);
            }
            return result;
        }

        public static async Task<int> GetSetPlayersDeathDayAsync(HashSet<string> playerNames)
        {
            foreach (string aName in playerNames)
            {
                PlayerItem player = await App.PlayerDatabase.GetPlayerAsync(aName);
                player.Lives = 0;
                await App.PlayerDatabase.UpdatePlayerAsync(player);
                // Spouse
                if (player.Spouse != null)
                {
                    PlayerItem spouse = await App.PlayerDatabase.GetPlayerAsync(player.Spouse);
                    spouse.Lives = 0;
                    await App.PlayerDatabase.UpdatePlayerAsync(spouse);
                }             
            }
            return 1;
        }

        public static async Task<string> CheckWin()
        {
            List<PlayerItem> alivePlayers = await App.PlayerDatabase.GetPlayersPresentAliveAsync();
            int mafias = (await App.PlayerDatabase.GetPlayersPresentAliveByRoleAsync(Roles.Mafia)).Count;
            int buergers = alivePlayers.Count - mafias;

            if (mafias == 0)
            {
                return "Bürger";
            } 
            if(buergers == 0)
            {
                return "Mafia";
            }
            if(alivePlayers[0].Spouse == alivePlayers[1].Name && alivePlayers.Count == 2)
            {
                return "Liebespaar";
            }
            return null;
        }
        public static async Task<int> ResetGameAsync()
        {
            List<PlayerItem> players = await App.PlayerDatabase.GetPlayersAsync();
            foreach(PlayerItem aPlayer in players)
            {
                aPlayer.Role = Roles.None;
                aPlayer.Spouse = null;
                aPlayer.Lives = 1;
                await App.PlayerDatabase.UpdatePlayerAsync(aPlayer);
            }
            //Liebespaar, Rollen, Leben, Fähigkeiten
            return 1;
        }
    }
}
