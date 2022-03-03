using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MafiaApp
{
    public static class RolesInitializer
    {
        public static async Task<int> Initialize()
        {
            //foreach (Roles aRole in Enum.GetValues(typeof(Roles)))
            //{
            //    await App.RolesDatabase.SaveRoleAsync(new RolesItem
            //    {
            //        Role = aRole,
            //        Number = 1,
            //        Ability = "kann nichts",
            //        Lives = 1,
            //        Active = true,
            //    });
            //}
            await App.RolesDatabase.SaveRoleAsync(new RolesItem
            {
                Role = Roles.Bürger,
                Number = 0,
                Ability = "kann nichts",
                Lives = 1,
                Active = true,
            });

            await App.RolesDatabase.SaveRoleAsync(new RolesItem
            {
                Role = Roles.Mafia,
                Number = 0,
                Ability = "kann was",
                Lives = 1,
                Active = true,
            });

            await App.RolesDatabase.SaveRoleAsync(new RolesItem
            {
                Role = Roles.Hexe,
                Number = 0,
                Ability = "kann nichts",
                Lives = 1,
                Active = true,
            });

            await App.RolesDatabase.SaveRoleAsync(new RolesItem
            {
                Role = Roles.Amor,
                Number = 0,
                Ability = "kann nichts",
                Lives = 1,
                Active = true,
            });
            await App.RolesDatabase.SaveRoleAsync(new RolesItem
            {
                Role = Roles.Detektiv,
                Number = 0,
                Ability = "kann nichts",
                Lives = 1,
                Active = true,
            });
            await App.RolesDatabase.SaveRoleAsync(new RolesItem
            {
                Role = Roles.Penner,
                Number = 0,
                Ability = "kann nichts",
                Lives = 1,
                Active = true,
            });

            return 1;
        }

        public static async Task<int> SetNumbersAuto(int playerNumber)
        {
            int rolesNumber = 0;
            List<RolesItem> roles = await App.RolesDatabase.GetRolesAsync();
            foreach (RolesItem aRolesItem in roles)
            {
                rolesNumber += aRolesItem.Number;
            }
            if(playerNumber != rolesNumber)
            {
                RolesItem buerger = await App.RolesDatabase.GetRoleAsync(Roles.Bürger);
                buerger.Number = playerNumber;
                await App.RolesDatabase.UpdateRolesAsync(buerger);
            }
            return 1;
        }

        public static async Task<int> SetNumbersManual(Roles role, bool increase)
        {
            if(increase == true)
            {
                RolesItem rolesItem = await App.RolesDatabase.GetRoleAsync(role);
                RolesItem buergerItem = await App.RolesDatabase.GetRoleAsync(Roles.Bürger);
                rolesItem.Number += 1;
                buergerItem.Number -= 1;
                await App.RolesDatabase.UpdateRolesAsync(rolesItem);
                await App.RolesDatabase.UpdateRolesAsync(buergerItem);
            }
            else
            {
                RolesItem rolesItem = await App.RolesDatabase.GetRoleAsync(role);
                RolesItem buergerItem = await App.RolesDatabase.GetRoleAsync(Roles.Bürger);
                rolesItem.Number -= 1;
                buergerItem.Number += 1;
                await App.RolesDatabase.UpdateRolesAsync(rolesItem);
                await App.RolesDatabase.UpdateRolesAsync(buergerItem);
            }

            return 1;
        }
    }
}
