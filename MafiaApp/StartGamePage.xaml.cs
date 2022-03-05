using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SQLite;
using Xamarin.Forms.Shapes;

namespace MafiaApp
{
    public partial class StartGamePage : ContentPage
    {
        int round = 1;
        bool witchCanSave = true;
        int numberAmor, numberMafia, numberHexe, numberBuerger, numberDetektiv;
        
        public StartGamePage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await SetUp();
        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.PopModalAsync();
            //TODO: Reset all
            return true;
        }

        private async Task<int> SetUp()
        {
            numberMafia = (await App.RolesDatabase.GetRoleAsync(Roles.Mafia)).Number;
            numberAmor = (await App.RolesDatabase.GetRoleAsync(Roles.Amor)).Number;
            numberHexe = (await App.RolesDatabase.GetRoleAsync(Roles.Hexe)).Number;
            numberDetektiv = (await App.RolesDatabase.GetRoleAsync(Roles.Detektiv)).Number;
            numberBuerger = (await App.RolesDatabase.GetRoleAsync(Roles.Bürger)).Number;
            if (numberAmor == 0)
            {
                amorFrame.IsVisible = false;
            }
            else
            {
                amorFrame.IsVisible = true;
                amorNames.ItemsSource = await GameManagement.GetPlayersAsync(Roles.Amor, numberAmor);
            }
            if (numberMafia == 0)
            {
                mafiaFrame.IsVisible = false;
            }
            else
            {
                mafiaFrame.IsVisible = true;
                mafiaNames.ItemsSource = await GameManagement.GetPlayersAsync(Roles.Mafia, numberMafia);
            }
            if (numberHexe == 0)
            {
                hexeFrame.IsVisible = false;
            }
            else
            {
                hexeFrame.IsVisible = true;
                hexeNames.ItemsSource = await GameManagement.GetPlayersAsync(Roles.Hexe, numberHexe);
            }
            if (numberDetektiv == 0)
            {
                detektivFrame.IsVisible = false;
            }
            else
            {
                detektivFrame.IsVisible = true;
                detektivNames.ItemsSource = await GameManagement.GetPlayersAsync(Roles.Detektiv, numberDetektiv);
            }
            return 1;
        }



        async void OnResetGame(object sender, EventArgs e)
        {
            await GameManagement.ResetGameAsync();
            await SetUp();
        }

        async Task<int> CloseAllFrames()
        {
            int c = 1;
            object o = new object();
            EventArgs e = new EventArgs();
            if (amorFrame.HeightRequest == 200)
            {
                OnPopoutAmor(o, e);
            }
            if (mafiaFrame.HeightRequest == 200)
            {
                OnPopoutMafia(o, e);
            }
            if (hexeFrame.HeightRequest == 200)
            {
                OnPopoutHexe(o, e);
            }
            if (detektivFrame.HeightRequest == 200)
            {
                OnPopoutDetektiv(o, e);
            }
            if (buergerFrame.HeightRequest == 200)
            {
                // zurückgeben ob abstimmung fertig ist 
                OnPopoutBuerger(o, e);
            }
            return c;
        }

        async Task<int> ChooseNameAsync(CollectionView collectionNames, Roles role)
        {
            if (collectionNames.SelectedItem != null)
            {
                string previous = ((PlayerItem)collectionNames.SelectedItem).Name;
                List<string> playerNames = await GameManagement.GetPlayerNamesAsync(Roles.None);
                string selection = await DisplayActionSheet("Name Auswählen", "Abbrechen", "Keiner", playerNames.ToArray());
                if (selection != null)
                {
                    if (selection.Equals("Keiner"))
                    {
                        await GameManagement.SetPlayersRoleAsync(previous, Roles.None);
                    }
                    else if (!selection.Equals("Abbrechen"))
                    {
                        await GameManagement.SetPlayersRoleAsync(previous, Roles.None);
                        await GameManagement.SetPlayersRoleAsync(selection, role);
                    }
                }
                collectionNames.ItemsSource = await GameManagement.GetPlayersAsync(role, (await App.RolesDatabase.GetRoleAsync(role)).Number);
                collectionNames.SelectedItem = null;
            }
            return 1;
        }
        async void OnAmorSelectionChanged(object sender, EventArgs e)
        {
            await ChooseNameAsync(amorNames, Roles.Amor);
        }
        async void OnMafiaSelectionChanged(object sender, EventArgs e)
        {
            await ChooseNameAsync(mafiaNames, Roles.Mafia);
        }
        async void OnHexeSelectionChanged(object sender, EventArgs e)
        {
            await ChooseNameAsync(hexeNames, Roles.Hexe);
        }
        async void OnDetektivSelectionChanged(object sender, EventArgs e)
        {
            await ChooseNameAsync(detektivNames, Roles.Detektiv);
        }


        async void OnPopoutAmor(object sender, EventArgs e)
        {
            Frame item = amorFrame;
            // open
            if (item.HeightRequest == 50)
            {
                //List<PlayerItem> names = ((List<PlayerItem>)amorNames.ItemsSource);
                //bool containsKeiner = names.Last<PlayerItem>().Name.Equals("Keiner");
                await CloseAllFrames();
                if (((List<PlayerItem>)amorNames.ItemsSource).Last().Name.Equals("Keiner"))
                {
                    await DisplayAlert("Warnung", "Du hast noch nicht alle Spieler für diese Rolle eingetragen.", "Okay");
                }
                else
                {
                    item.HeightRequest = 200;
                }
            }
            // close
            else
            {
                if (spouse1.Text != null)
                {
                    item.BackgroundColor = Color.Gold;
                }
                else
                {
                    bool input = await DisplayAlert("Warnung", "Du hast noch kein Liebespaar ausgewählt", "Fortfahren", "Abbrechen");
                    if (input == false)
                    {
                        return;
                    }
                }
                item.HeightRequest = 50;

            }
        }

        async void OnSpouseChange(object sender, EventArgs e)
        {
            List<string> playerNames = await GameManagement.GetPlayerNamesAsync();
            string s1 = await DisplayActionSheet("Ehepartner 1 auswählen", "Abbrechen", null, playerNames.ToArray());
            if (s1.Equals("Abbrechen"))
            {
                return;
            }
            // Remove s1 from choosable players
            playerNames.Remove(s1);
            string s2 = await DisplayActionSheet("Ehepartner 2 auswählen", "Abbrechen", null, playerNames.ToArray());
            if (!s1.Equals("Abbrechen") && !s2.Equals("Abbrechen"))
            {
                await GameManagement.SetPlayersSpouseAsync(s1, s2);
                spouse1.Text = s1;
                spouse2.Text = s2;
                
            }
        }

        async void OnPopoutMafia(object sender, EventArgs e)
        {
            Frame item = mafiaFrame;
            //open
            if (item.HeightRequest == 50)
            {
                await CloseAllFrames();
                if (((List<PlayerItem>)mafiaNames.ItemsSource).Last().Name.Equals("Keiner"))
                {
                    await DisplayAlert("Warnung", "Du hast noch nicht alle Spieler für diese Rolle eingetragen.", "Okay");
                }
                else
                {
                    item.HeightRequest = 200;
                }
            }
            //close
            else
            {
                if (victim.Text != "")
                {
                    item.BackgroundColor = Color.Gold;
                }
                else 
                { 
                    bool input = await DisplayAlert("Warnung", "Du hast noch kein Opfer ausgewählt", "Fortfahren", "Abbrechen");
                    if (input == false)
                    {
                        return;
                    }
                }
                item.HeightRequest = 50;
            }
        }


        async void OnVictimSelect(object sender, EventArgs e)
        {
            string prev = victim.Text;

            List<string> playerNames = await GameManagement.GetPlayerNamesAsync();
            string selection = await DisplayActionSheet("Opfer Auswählen", "Abbrechen", null, playerNames.ToArray());
            if (selection != null)
            {
                if (selection.Equals("Abbrechen"))
                {
                    return;
                }
                else
                {
                    victim.Text = selection;
                    //showVictim.Text = selection;
                }
            }
        }

        async void OnPopoutHexe(object sender, EventArgs e)
        {
            Frame item = hexeFrame;
            if (item.HeightRequest == 50)
            {
                await CloseAllFrames();
                if (((List<PlayerItem>)hexeNames.ItemsSource).Last().Name.Equals("Keiner"))
                {
                    await DisplayAlert("Warnung", "Du hast noch nicht alle Spieler für diese Rolle eingetragen.", "Okay");
                }
                else
                {
                    item.HeightRequest = 200;
                }
            }
            else
            {
                item.HeightRequest = 50;
                //if (witchSaveSwitch.IsToggled == true && witchSaveSwitch.IsEnabled == true)
                //{
                //    await database.SetPlayerLivesAsync(showVictim.Text, 0.5);
                //    witchSaveSwitch.IsEnabled = false;
                //}
                //else
                //{
                //    // sterben des Spielers in log
                //    await database.SetPlayerLivesAsync(showVictim.Text, -0.5);
                //}

                //if (witchKill.Text != null)
                //{
                //    await database.SetPlayerLivesAsync(witchKill.Text, -1);
                //    witchKill.IsEnabled = false;
                //}
            }
        }
        //async void OnWitchKill(object sender, EventArgs e)
        //{

        //    MafiaItemDatabase database = await MafiaItemDatabase.Instance;
        //    string[] playerNames = await database.GetPlayersNoRoleAndPresentAsync();
        //    string selection = await DisplayActionSheet("Name Auswählen", "Abbrechen", "Keiner", playerNames);
        //    if (selection != null)
        //    {
        //        if (selection.Equals("Keiner"))
        //        {
        //            witchKill.Text = null;
        //        }
        //        else if (!selection.Equals("Abbrechen"))
        //        {
        //            witchKill.Text = selection;
        //        }
        //    }
        //}

        
        async void OnPopoutDetektiv(object sender, EventArgs e)
        {
            Frame item = detektivFrame;
            if (item.HeightRequest == 50)
            {
                await CloseAllFrames();
                if (((List<PlayerItem>)detektivNames.ItemsSource).Last().Name.Equals("Keiner"))
                {
                    await DisplayAlert("Warnung", "Du hast noch nicht alle Spieler für diese Rolle eingetragen.", "Okay");
                }
                else
                {
                    item.HeightRequest = 200;
                }
            }
            else
            {
                item.HeightRequest = 50;
                //MafiaItemDatabase database = await MafiaItemDatabase.Instance;
                //await database.SetRoleActive(roles.Detektiv, false);
            }
        }

        //async void OnChoosePerson(object sender, EventArgs e)
        //{
        //    MafiaItemDatabase database = await MafiaItemDatabase.Instance;
        //    string[] playerNames = await database.GetPlayersPresentAndAliveAsync();
        //    string selection = await DisplayActionSheet("Name Auswählen", "Abbrechen", null, playerNames);
        //    if (selection != null && !selection.Equals("Abbrechen"))
        //    {
        //        name.Text = selection;
        //        role.Text = (await database.GetPlayersRole(selection)).ToString();
        //    }
        //}

        async void OnPopoutBuerger(object sender, EventArgs e)
        {
            Frame item = buergerFrame;
            if (item.HeightRequest == 50)
            {
                
                int ok = await CloseAllFrames();
                if (ok == 1)
                {
                    //if ((await database.GetPlayersNoRoleAndPresentAsync()).Length == await database.GetRoleNumber(roles.Bürger))  // Wenn alle anderen Rollen festgelgt sind
                    //{
                    //    if (await database.GetBuergerInitialized())
                    //    {
                    //        //await database.SetBuerger();
                    //    }
                    //    buergerNames.ItemsSource = await database.GetPlayersByRoleAndNumberAsync(roles.Bürger, (await database.GetRoleNumber(roles.Bürger)));
                        item.HeightRequest = 200;
                    //}
                    //else
                    //{
                    //    await DisplayAlert("Warnung", "Du hast noch nicht alle Namen zugewiesen", null, "OK");
                    //}
                }

            }
            else
            {
                item.HeightRequest = 50;
                //MafiaItemDatabase database = await MafiaItemDatabase.Instance;
                //await database.SetRoleActive(roles.Bürger, false);
            }
        }
    }
}
