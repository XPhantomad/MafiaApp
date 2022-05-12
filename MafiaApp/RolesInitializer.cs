﻿using System;
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
                Ability = "Lauscht in der Nacht aufmerksam nach auffälligen Geräuschen.",
                Lives = 1, 
            });

            await App.RolesDatabase.SaveRoleAsync(new RolesItem
            {
                Role = Roles.Mafia,
                Number = 0,
                Ability = "Wählt in jeder Nacht ein Opfer aus.",
                Lives = 1,
            });

            await App.RolesDatabase.SaveRoleAsync(new RolesItem
            {
                Role = Roles.Hexe,
                Number = 0,
                Ability = "Darf pro Spiel einmal eine Person retten und einmal eine Person töten.",
                Lives = 1,
            });

            await App.RolesDatabase.SaveRoleAsync(new RolesItem
            {
                Role = Roles.Amor,
                Number = 0,
                Ability = "Bestimmt am Anfang des Spieles ein Liebespaar für das gesamte Spiel.",
                Lives = 1,
            });
            await App.RolesDatabase.SaveRoleAsync(new RolesItem
            {
                Role = Roles.Detektiv,
                Number = 0,
                Ability = "Wählt am Ende jeder Nacht einen Spieler aus, von dem er die Gesinnung erfährt.",
                Lives = 1,
            });
            await App.RolesDatabase.SaveRoleAsync(new RolesItem
            {
                Role = Roles.Penner,
                Number = 0,
                Ability = "Sucht sich in der Nacht ein Quartier zum Übernachten oder schläft unter der Brücke.",
                Lives = 1,
            });
            await App.RolesDatabase.SaveRoleAsync(new RolesItem
            {
                Role = Roles.Opa,
                Number = 0,
                Ability = "Hat in der Nacht Zwei Leben.",
                Lives = 2,
            });
            await App.RolesDatabase.SaveRoleAsync(new RolesItem
            {
                Role = Roles.Jäger,
                Number = 0,
                Ability = "Nimmt bei seinem Tod einen anderen Spieler mit in den Tod.",
                Lives = 1,
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
            if (playerNumber != rolesNumber)
            {
                foreach(RolesItem aRolesItem in roles)
                {
                    if (aRolesItem.Role.Equals(Roles.Bürger))
                    {
                        aRolesItem.Number = playerNumber;
                    }
                    else
                    {
                        aRolesItem.Number = 0;
                    }
                    await App.RolesDatabase.UpdateRolesAsync(aRolesItem);
                }
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
