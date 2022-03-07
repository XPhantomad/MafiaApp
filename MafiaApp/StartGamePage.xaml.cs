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
        Color roleFinishedColor = Color.Chocolate;
        Color inactiveColor = Color.Gray;
        int numberAmor, numberMafia, numberHexe, numberBuerger, numberDetektiv;
        
        public StartGamePage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await SetUpFirstRound();
        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.PopModalAsync();
            //TODO: Reset all
            GameManagement.ResetGameAsync();
            return true;
        }

        private async Task<int> SetUpFirstRound()
        {
            numberMafia = (await App.RolesDatabase.GetRoleAsync(Roles.Mafia)).Number;
            numberAmor = (await App.RolesDatabase.GetRoleAsync(Roles.Amor)).Number;
            numberHexe = (await App.RolesDatabase.GetRoleAsync(Roles.Hexe)).Number;
            numberDetektiv = (await App.RolesDatabase.GetRoleAsync(Roles.Detektiv)).Number;
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

        private async Task<int> SetupNewRound()
        {
            numberMafia = (await App.RolesDatabase.GetRoleAsync(Roles.Mafia)).Number;
            numberAmor = (await App.RolesDatabase.GetRoleAsync(Roles.Amor)).Number;
            numberHexe = (await App.RolesDatabase.GetRoleAsync(Roles.Hexe)).Number;
            numberDetektiv = (await App.RolesDatabase.GetRoleAsync(Roles.Detektiv)).Number;
            numberBuerger = (await App.RolesDatabase.GetRoleAsync(Roles.Bürger)).Number;
            if (numberAmor != 0)
            {
                amorNames.ItemsSource = await GameManagement.GetPlayersAsync(Roles.Amor, numberAmor);
            }
            if (numberMafia != 0)
            {
                mafiaNames.ItemsSource = await GameManagement.GetPlayersAsync(Roles.Mafia, numberMafia);
            }
            if (numberHexe != 0)
            {
                hexeNames.ItemsSource = await GameManagement.GetPlayersAsync(Roles.Hexe, numberHexe);
            }
            if (numberDetektiv != 0)
            {
                detektivNames.ItemsSource = await GameManagement.GetPlayersAsync(Roles.Detektiv, numberDetektiv);
            }
            return 1;
        }

        async void OnResetGame(object sender, EventArgs e)
        {
            await GameManagement.ResetGameAsync();
            await SetUpFirstRound();
        }

        void CloseAllFrames()
        {
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
            if (electionFrame.HeightRequest == 200)
            {
                OnPopoutElection(o, e);
            }
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
                CloseAllFrames();
                if (amorNames.ItemsSource != null)
                {
                    if (((List<PlayerItem>)amorNames.ItemsSource).Last().Name.Equals("Keiner"))
                    {
                        await DisplayAlert("Warnung", "Du hast noch nicht alle Spieler für diese Rolle eingetragen.", "Okay");
                    }                
                    else
                    {
                        item.HeightRequest = 200;
                    }
                }
            }
            // close
            else
            {
                if (spouse1.Text != null)
                {
                    if (item.BackgroundColor != inactiveColor)
                    {
                        item.BackgroundColor = roleFinishedColor;
                    }
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
                CloseAllFrames();
                if (mafiaNames.ItemsSource != null)
                {
                    if (((List<PlayerItem>)mafiaNames.ItemsSource).Last().Name.Equals("Keiner"))
                    {
                        await DisplayAlert("Warnung", "Du hast noch nicht alle Spieler für diese Rolle eingetragen.", "Okay");
                    }
                    else
                    {
                        item.HeightRequest = 200;
                    }
                }
            }
            //close
            else
            {
                if (victim.Text != null)
                {
                    item.BackgroundColor = roleFinishedColor;
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
                    victim.Text = showVictim.Text = selection;
                }
            }
        }
        async void OnPopoutHexe(object sender, EventArgs e)
        {
            Frame item = hexeFrame;
            if (item.HeightRequest == 50)
            {
                CloseAllFrames();
                if (hexeNames.ItemsSource != null)
                {
                    if (((List<PlayerItem>)hexeNames.ItemsSource).Last().Name.Equals("Keiner"))
                    {
                        await DisplayAlert("Warnung", "Du hast noch nicht alle Spieler für diese Rolle eingetragen.", "Okay");
                    }                
                    else if (victim.Text == null)
                    {
                        await DisplayAlert("Warnung", "Du hast noch kein Opfer ausgewählt.", "Okay");
                    }
                    else
                    {
                        item.HeightRequest = 200;
                    }
                }
            }
            else
            {
                if (item.BackgroundColor != inactiveColor)
                {
                    item.BackgroundColor = roleFinishedColor; 
                }
                item.HeightRequest = 50;
            }
        }
        async void OnWitchKill(object sender, EventArgs e)
        {
            List<string> playerNames = await GameManagement.GetPlayerNamesAsync();
            string selection = await DisplayActionSheet("Name Auswählen", "Abbrechen", "Keiner", playerNames.ToArray());
            if (selection != null)
            {
                if (selection.Equals("Keiner"))
                {
                    witchVictim.Text = null;
                }
                else if (!selection.Equals("Abbrechen"))
                {
                    witchVictim.Text = selection;
                }
            }
        }
        async void OnPopoutDetektiv(object sender, EventArgs e)
        {
            Frame item = detektivFrame;
            if (item.HeightRequest == 50)
            {
                CloseAllFrames();
                if (detektivNames.ItemsSource != null)
                {
                    if (((List<PlayerItem>)detektivNames.ItemsSource).Last().Name.Equals("Keiner"))
                    {
                        await DisplayAlert("Warnung", "Du hast noch nicht alle Spieler für diese Rolle eingetragen.", "Okay");
                    }
                    else
                    {
                        item.HeightRequest = 200;
                    }
                }
            }
            else
            {
                item.HeightRequest = 50;
                if (item.BackgroundColor != inactiveColor)
                {
                    item.BackgroundColor = roleFinishedColor;
                }
            }
        }

        async void OnChoosePerson(object sender, EventArgs e)
        {
            List<string> playerNames = await GameManagement.GetPlayerNamesAsync();
            string selection = await DisplayActionSheet("Name Auswählen", "Abbrechen", null, playerNames.ToArray());
            if (selection != null && !selection.Equals("Abbrechen"))
            {
                name.Text = selection;
                role.Text = (await App.PlayerDatabase.GetPlayerAsync(selection)).Role.ToString();
            }
        }

        async void OnPopoutElection(object sender, EventArgs e)
        {
            Frame item = electionFrame;
            //open
            if (item.HeightRequest == 50)
            {                
                CloseAllFrames();
                bool allRolesFinished = amorFrame.BackgroundColor == roleFinishedColor && mafiaFrame.BackgroundColor == roleFinishedColor &&
                                        hexeFrame.BackgroundColor == roleFinishedColor && detektivFrame.BackgroundColor == roleFinishedColor;
                if(allRolesFinished)
                {
                    // set remaining players to buerger
                    if (round == 1)
                    {
                        List<string> remainingNames = await GameManagement.GetPlayerNamesAsync(Roles.None);
                        if (remainingNames.Count == (await App.RolesDatabase.GetRoleAsync(Roles.Bürger)).Number)  // number of remaining Players equals number of buergers
                        {                           
                            await GameManagement.SetPlayersRoleAsync(remainingNames, Roles.Bürger);
                            buergerNames.ItemsSource = await GameManagement.GetPlayersAsync(Roles.Bürger, (await App.RolesDatabase.GetRoleAsync(Roles.Bürger)).Number);                            
                        }
                        else
                        {
                            await DisplayAlert("Warnung", "Du hast noch nicht alle Namen zugewiesen", null, "Okay");
                            return;
                        }
                        
                    }
                    // Calculate deaths and so on
                    HashSet<string> deathPlayer = await SetDeathsAndAbilities();
                    killedNames.ItemsSource = deathPlayer;
                    item.HeightRequest = 200;
                }
                else
                {
                    await DisplayAlert("Warnung", "Es sind noch nicht alle Rollen aufgewacht", "Okay");
                }
            }
            //close
            else
            {
                bool input = await DisplayAlert("Warnung", "Willst du wirklich die nächste Nacht beginnen?", "Ja", "Nein");
                if (input == false)
                {
                    return;
                }
                round = 2;
                await SetupNewRound();

                item.HeightRequest = 50;
            }
        }

        async Task<HashSet<string>> SetDeathsAndAbilities()
        {
            if(witchSaveSwitch.IsToggled == true)
            {
                witchSaveSwitch.IsEnabled = false;
                return new HashSet<string>{ "Keiner" };
            }
            else
            {
                HashSet<string> result = new HashSet<string>();
                result.Add(victim.Text);
                if(witchVictim.Text != null)
                {
                    result.Add(witchVictim.Text);
                    witchVictim.IsEnabled = false;
                }
                return await GameManagement.GetSetPlayersDeathAsync(result);
            }
        }
    }
}
