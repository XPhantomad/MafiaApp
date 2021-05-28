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
        public bool witchCanSave = true;
        public StartGamePage()
        {
            InitializeComponent();
            
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await SetUp();


        }

        async Task<int> SetUp()
        {
            MafiaItemDatabase database = await MafiaItemDatabase.Instance;
            int numberMafia = await database.GetRoleNumber(roles.Mafia);
            int numberAmor = await database.GetRoleNumber(roles.Amor);
            int numberHexe = await database.GetRoleNumber(roles.Hexe);
            int numberDetektiv = await database.GetRoleNumber(roles.Detektiv);
            int numberBuerger = await database.GetRoleNumber(roles.Bürger);
            if (numberAmor == 0)
                amorFrame.IsVisible = false;
            else
            {
                amorFrame.IsVisible = true;
                amorNames.ItemsSource = await database.GetPlayersByRoleAndNumberAsync(roles.Amor, numberAmor);
            }
            if (numberMafia == 0)
                mafiaFrame.IsVisible = false;
            else
            {
                mafiaFrame.IsVisible = true;
                mafiaNames.ItemsSource = await database.GetPlayersByRoleAndNumberAsync(roles.Mafia, numberMafia);
            }
            if (numberHexe == 0)
                hexeFrame.IsVisible = false;
            else
            {
                hexeFrame.IsVisible = true;
                hexeNames.ItemsSource = await database.GetPlayersByRoleAndNumberAsync(roles.Hexe, numberHexe);
            }
            if (numberDetektiv == 0)
                detektivFrame.IsVisible = false;
            else
            {
                detektivFrame.IsVisible = true;
                detektivNames.ItemsSource = await database.GetPlayersByRoleAndNumberAsync(roles.Detektiv, numberDetektiv);
            }

            return 1;
        }

        async void OnSettings(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage());
        }

        async void OnResetGame(object sender, EventArgs e)
        {
            MafiaItemDatabase database = await MafiaItemDatabase.Instance;
            // Active zurücksetzen
            await database.SetRoleActive();
            // Rollenzuweisung, Leben, Liebespaar zurücksetzen
            await database.ResetPlayerItems();
            
            //Hexe fähigkeiten zurücksetzen
           
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
                c = await OnPopoutMafia2(o, e);
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


        async void OnAmorSelectionChanged(object sender, EventArgs e)
        {
            if (amorNames.SelectedItem != null)
            {
                string previous = amorNames.SelectedItem.ToString();
                MafiaItemDatabase database = await MafiaItemDatabase.Instance;
                string[] playerNames = await database.GetPlayersNoRoleAndPresentAsync();
                string selection = await DisplayActionSheet("Name Auswählen", "Abbrechen", "Keiner", playerNames);
                if (selection != null)
                {
                    if (selection.Equals("Keiner"))
                    {
                        await database.SetPlayersRoleAsync(previous, roles.None);
                    }
                    else if (!selection.Equals("Abbrechen"))
                    {
                        await database.SetPlayersRoleAsync(previous, roles.None);
                        await database.SetPlayersRoleAsync(selection, roles.Amor);
                    }
                }
                amorNames.ItemsSource = await database.GetPlayersByRoleAndNumberAsync(roles.Amor, await database.GetRoleNumber(roles.Amor));
                amorNames.SelectedItem = null;
            }
        }
        async void OnSpouseChange(object sender, EventArgs e)
        {
            string prevSpouse1 = spouse1.Text;
            string prevSpouse2 = spouse2.Text;
            MafiaItemDatabase database = await MafiaItemDatabase.Instance;
            string[] playerNames = await database.GetPlayersMarriedAsync(null, false);
            string s1 = await DisplayActionSheet("Ehepartner 1 auswählen", "Abbrechen", null, playerNames);
            if (s1.Equals("Abbrechen"))
            {
                return;
            }
            playerNames = await database.GetPlayersMarriedAsync(s1, false);
            string s2 = await DisplayActionSheet("Ehepartner 2 auswählen", "Abbrechen", null, playerNames);
            if (!s1.Equals("Abbrechen") && !s2.Equals("Abbrechen"))
            {
                await database.SetPlayerSpouseAsync(s1, s2);
                await database.SetPlayerNotSpouseAsync(prevSpouse1, prevSpouse2);
                spouse1.Text = s1;
                spouse2.Text = s2;
            }
        }

        async void OnPopoutAmor(object sender, EventArgs e)
        {
            Frame item = amorFrame;
            if (item.HeightRequest == 50)
            {
                int ok = await CloseAllFrames();
                if (ok == 1)
                {
                    item.HeightRequest = 200;
                }
            }
            else
            {
                item.HeightRequest = 50;
                MafiaItemDatabase database = await MafiaItemDatabase.Instance;
                await database.SetRoleActive(roles.Amor, false);
            }
        }
        async void OnPopoutMafia(object sender, EventArgs e)
        {
            await OnPopoutMafia2(sender, e);
        }

        async Task<int> OnPopoutMafia2(object sender, EventArgs e)
        {
            Frame item = mafiaFrame;
            if (item.HeightRequest == 50)
            {
                int ok = await CloseAllFrames();
                if (ok == 1)
                {
                    item.HeightRequest = 200;
                }
            }
            else
            {
                if (victim.Text == "")
                {
                    bool conti = await DisplayAlert("Warnung", "Du hast noch kein Opfer ausgewählt", "Fortfahren", "Abbrechen");
                    if (conti == false)
                    {
                        return 0;
                    }
                }
                item.HeightRequest = 50;
            }
            return 1;
        }

        async void OnMafiaSelectionChanged(object sender, EventArgs e)
        {
            if (mafiaNames.SelectedItem != null)
            {
                string previous = mafiaNames.SelectedItem.ToString();
                MafiaItemDatabase database = await MafiaItemDatabase.Instance;
                string[] playerNames = await database.GetPlayersNoRoleAndPresentAsync();
                string selection = await DisplayActionSheet("Name Auswählen", "Abbrechen", "Keiner", playerNames);
                if (selection != null)
                {
                    if (selection.Equals("Keiner"))
                    {
                        await database.SetPlayersRoleAsync(previous, roles.None);
                    }
                    else if (!selection.Equals("Abbrechen"))
                    {
                        await database.SetPlayersRoleAsync(previous, roles.None);
                        await database.SetPlayersRoleAsync(selection, roles.Mafia);
                    }
                }
                mafiaNames.ItemsSource = await database.GetPlayersByRoleAndNumberAsync(roles.Mafia, await database.GetRoleNumber(roles.Mafia));
                mafiaNames.SelectedItem = null;
            }       
            
        }

        async void OnVictimSelect(object sender, EventArgs e)
        {
            string prev = victim.Text;
            MafiaItemDatabase database = await MafiaItemDatabase.Instance;
            string[] playerNames = await database.GetPlayersPresentAndAliveAsync();
            string selection = await DisplayActionSheet("Opfer Auswählen", "Abbrechen", null, playerNames);
            if (selection != null)
            {

                if (selection.Equals("Abbrechen"))
                {
                    return;
                }
                else
                {
                    await database.SetPlayerLivesAsync(selection, -0.5);
                    await database.SetPlayerLivesAsync(prev, 0.5);
                    victim.Text = selection;
                    showVictim.Text = selection;
                }
            }
        }

        async void OnPopoutHexe(object sender, EventArgs e)
        {
            MafiaItemDatabase database = await MafiaItemDatabase.Instance;
            Frame item = hexeFrame;
            if (item.HeightRequest == 50)
            {
                int ok = await CloseAllFrames();
                if (ok == 1)
                {
                    item.HeightRequest = 200;
                }
            }
            else
            {
                item.HeightRequest = 50;
                if (witchSaveSwitch.IsToggled == true && witchSaveSwitch.IsEnabled == true)
                {
                    await database.SetPlayerLivesAsync(showVictim.Text, 0.5);
                    witchSaveSwitch.IsEnabled = false;
                }
                else
                {
                    // sterben des Spielers in log
                    await database.SetPlayerLivesAsync(showVictim.Text, -0.5);
                }

                if (witchKill.Text != null)
                {
                    await database.SetPlayerLivesAsync(witchKill.Text, -1);
                    witchKill.IsEnabled = false;
                }
            }
        }
        async void OnWitchKill(object sender, EventArgs e)
        {

            MafiaItemDatabase database = await MafiaItemDatabase.Instance;
            string[] playerNames = await database.GetPlayersNoRoleAndPresentAsync();
            string selection = await DisplayActionSheet("Name Auswählen", "Abbrechen", "Keiner", playerNames);
            if (selection != null)
            {
                if (selection.Equals("Keiner"))
                {
                    witchKill.Text = null;
                }
                else if (!selection.Equals("Abbrechen"))
                {
                    witchKill.Text = selection;
                }
            }
        }

        async void OnHexeSelectionChanged(object sender, EventArgs e)
        {
            if (hexeNames.SelectedItem != null)
            {
                string previous = hexeNames.SelectedItem.ToString();
                MafiaItemDatabase database = await MafiaItemDatabase.Instance;
                string[] playerNames = await database.GetPlayersNoRoleAndPresentAsync();
                string selection = await DisplayActionSheet("Name Auswählen", "Abbrechen", "Keiner", playerNames);
                if (selection != null)
                {
                    if (selection.Equals("Keiner"))
                    {
                        await database.SetPlayersRoleAsync(previous, roles.None);
                    }
                    else if (!selection.Equals("Abbrechen"))
                    {
                        await database.SetPlayersRoleAsync(previous, roles.None);
                        await database.SetPlayersRoleAsync(selection, roles.Hexe);
                    }
                }
                hexeNames.ItemsSource = await database.GetPlayersByRoleAndNumberAsync(roles.Hexe, await database.GetRoleNumber(roles.Hexe));
                hexeNames.SelectedItem = null;
            }
        }

        async void OnDetektivSelectionChanged(object sender, EventArgs e)
        {
            if (detektivNames.SelectedItem != null)
            {
                string previous = detektivNames.SelectedItem.ToString();
                MafiaItemDatabase database = await MafiaItemDatabase.Instance;
                string[] playerNames = await database.GetPlayersNoRoleAndPresentAsync();
                string selection = await DisplayActionSheet("Name Auswählen", "Abbrechen", "Keiner", playerNames);
                if (selection != null)
                {
                    if (selection.Equals("Keiner"))
                    {
                        await database.SetPlayersRoleAsync(previous, roles.None);
                    }
                    else if (!selection.Equals("Abbrechen"))
                    {
                        await database.SetPlayersRoleAsync(previous, roles.None);
                        await database.SetPlayersRoleAsync(selection, roles.Detektiv);
                    }
                }
                detektivNames.ItemsSource = await database.GetPlayersByRoleAndNumberAsync(roles.Detektiv, await database.GetRoleNumber(roles.Detektiv));
                detektivNames.SelectedItem = null;
            }
        }
        async void OnPopoutDetektiv(object sender, EventArgs e)
        {
            Frame item = detektivFrame;
            if (item.HeightRequest == 50)
            {
                int ok = await CloseAllFrames();
                if (ok == 1)
                {
                    item.HeightRequest = 200;
                }
            }
            else
            {
                item.HeightRequest = 50;
                MafiaItemDatabase database = await MafiaItemDatabase.Instance;
                await database.SetRoleActive(roles.Detektiv, false);
            }
        }

        async void OnChoosePerson(object sender, EventArgs e)
        {
            MafiaItemDatabase database = await MafiaItemDatabase.Instance;
            string[] playerNames = await database.GetPlayersPresentAndAliveAsync();
            string selection = await DisplayActionSheet("Name Auswählen", "Abbrechen", null, playerNames);
            if (selection != null && !selection.Equals("Abbrechen"))
            {
                name.Text = selection;
                role.Text = (await database.GetPlayersRole(selection)).ToString();
            }
        }

        async void OnPopoutBuerger(object sender, EventArgs e)
        {
            Frame item = buergerFrame;
            if (item.HeightRequest == 50)
            {
                MafiaItemDatabase database = await MafiaItemDatabase.Instance;
                int ok = await CloseAllFrames();
                if (ok == 1 )
                {
                    if ((await database.GetPlayersNoRoleAndPresentAsync()).Length == await database.GetRoleNumber(roles.Bürger))  // Wenn alle anderen Rollen festgelgt sind
                    {
                        if (await database.GetBuergerInitialized())
                        {
                            await database.SetBuerger();
                        }
                        buergerNames.ItemsSource = await database.GetPlayersByRoleAndNumberAsync(roles.Bürger,(await database.GetRoleNumber(roles.Bürger)));
                        item.HeightRequest = 200;
                    }
                    else
                    {
                        await DisplayAlert("Warnung", "Du hast noch nicht alle Namen zugewiesen",null, "OK");
                    }
                }
                
            }
            else
            {
                item.HeightRequest = 50;
                MafiaItemDatabase database = await MafiaItemDatabase.Instance;
                await database.SetRoleActive(roles.Bürger, false);
            }
        }
    }
}
