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
            await App.RolesDatabase.SaveRoleAsync(new RolesItem
            {
                Name = "Bürger",
                Number = 0,
                Ability = "kann nichts",
                Lives = 1,
                Active = true,
            });

            await App.RolesDatabase.SaveRoleAsync(new RolesItem
            {
                Name = "Mafia",
                Number = 0,
                Ability = "kann was",
                Lives = 1,
                Active = true,
            });

            await App.RolesDatabase.SaveRoleAsync(new RolesItem
            {
                Name = "Hexe",
                Number = 0,
                Ability = "kann nichts",
                Lives = 1,
                Active = true,
            });

            await App.RolesDatabase.SaveRoleAsync(new RolesItem
            {
                Name = "Amor",
                Number = 0,
                Ability = "kann nichts",
                Lives = 1,
                Active = true,
            });
            await App.RolesDatabase.SaveRoleAsync(new RolesItem
            {
                Name = "Detektiv",
                Number = 0,
                Ability = "kann nichts",
                Lives = 1,
                Active = true,
            });
            await App.RolesDatabase.SaveRoleAsync(new RolesItem
            {
                Name = "Penner",
                Number = 0,
                Ability = "kann nichts",
                Lives = 1,
                Active = true,
            });

            return 1;
        }
    }
}
