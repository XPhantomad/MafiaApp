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
        public int Round = 1;
        Color roleFinishedColor = Color.GreenYellow;
        Color roleInactiveColor = Color.Gray;
        int numberAmor, numberMafia, numberHexe, numberBuerger, numberDetektiv, numberPenner, numberOpa, numberJaeger;
        private List<Frame> frames;
        
        
        public StartGamePage()
        {
            InitializeComponent();
            frames = new List<Frame> { amorFrame, mafiaFrame, hexeFrame, detektivFrame, electionFrame };
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            GameManagement.ResetGameAsync();
            await SetUpFirstRound();
            Console.WriteLine("Hello World");
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
            numberPenner = (await App.RolesDatabase.GetRoleAsync(Roles.Penner)).Number;
            numberOpa = (await App.RolesDatabase.GetRoleAsync(Roles.Opa)).Number;
            numberJaeger = (await App.RolesDatabase.GetRoleAsync(Roles.Jäger)).Number;
            numberBuerger = (await App.RolesDatabase.GetRoleAsync(Roles.Bürger)).Number;
            if (numberOpa == 0)
            {
                opaFrame.IsVisible = false;
            }
            else
            {
                opaFrame.IsVisible = true;
                opaNames.ItemsSource = await GameManagement.GetPlayersAsync(Roles.Opa, numberOpa);
            }
            if (numberJaeger == 0)
            {
                jaegerFrame.IsVisible = false;
            }
            else
            {
                jaegerFrame.IsVisible = true;
                jaegerNames.ItemsSource = await GameManagement.GetPlayersAsync(Roles.Jäger, numberJaeger);
            }
            if (numberAmor == 0)
            {
                amorFrame.IsVisible = false;
            }
            else
            {
                amorFrame.IsVisible = true;
                amorNames.ItemsSource = await GameManagement.GetPlayersAsync(Roles.Amor, numberAmor);
            }
            if (numberPenner == 0)
            {
                pennerFrame.IsVisible = false;
            }
            else
            {
                pennerFrame.IsVisible = true;
                pennerNames.ItemsSource = await GameManagement.GetPlayersAsync(Roles.Penner, numberPenner);
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
            Round++;
            roundDisp.Text = "Runde: " + Round;
            if (numberOpa != 0)
            {
                List<PlayerItem> opas = await GameManagement.GetPlayersAsync(Roles.Opa, numberOpa);
                opaNames.ItemsSource = opas;
                if (opas.Last().Lives <= 0)
                {
                    opaFrame.BackgroundColor = roleInactiveColor;
                }
            }
            if (numberJaeger != 0)
            {
                List<PlayerItem> jaegers = await GameManagement.GetPlayersAsync(Roles.Jäger, numberJaeger);
                jaegerNames.ItemsSource = jaegers;
                if (jaegers.Last().Lives <= 0)
                {
                    jaegerFrame.BackgroundColor = roleInactiveColor;
                }
            }
            if (numberAmor != 0)
            {
                List<PlayerItem> amors = await GameManagement.GetPlayersAsync(Roles.Amor, numberAmor);
                amorNames.ItemsSource = amors;
                if(amors.Last().Lives <= 0)
                {
                    amorFrame.BackgroundColor = roleInactiveColor;
                }
            }
            if (numberPenner != 0)
            {
                List<PlayerItem> penners = await GameManagement.GetPlayersAsync(Roles.Penner, numberPenner);
                pennerNames.ItemsSource = penners;
                if (penners.Last().Lives <= 0)
                {
                    pennerFrame.BackgroundColor = roleInactiveColor;
                }
                else
                {
                    pennerFrame.BackgroundColor = default;
                }
                accomodation.Text = "Brücke";
            }
            if (numberMafia != 0)
            {
                mafiaNames.ItemsSource = await GameManagement.GetPlayersAsync(Roles.Mafia, numberMafia);
                mafiaFrame.BackgroundColor = default;
                victim.Text = null;
            }
            if (numberHexe != 0)
            {
                List<PlayerItem> witches = await GameManagement.GetPlayersAsync(Roles.Hexe, numberHexe);
                hexeNames.ItemsSource = witches;
                if (witches.Last().Lives <= 0)
                {
                    hexeFrame.BackgroundColor = roleInactiveColor;
                }
                else
                {
                    hexeFrame.BackgroundColor = default;
                }
            }
            if (numberDetektiv != 0)
            {
                List<PlayerItem> detektives = await GameManagement.GetPlayersAsync(Roles.Detektiv, numberDetektiv);
                detektivNames.ItemsSource = detektives;
                if (detektives.Last().Lives <= 0)
                {
                    detektivFrame.BackgroundColor = roleInactiveColor;
                }
                else
                {
                    detektivFrame.BackgroundColor = default;
                }
                uncoveredName.Text = null;
                uncoveredRole.Text = null;
            }
            if(numberBuerger != 0)
            {
                buergerNames.ItemsSource = await GameManagement.GetPlayersAsync(Roles.Bürger, numberBuerger);
                onDayKilledNames.ItemsSource = null;
                if((await GameManagement.GetPlayerNamesAsync(Roles.Bürger)).Count == 0)
                {
                    electionFrame.BackgroundColor = roleInactiveColor;
                }
            }
            return 1;
        }

        //async void OnResetGame(object sender, EventArgs e)
        //{
        //    await GameManagement.ResetGameAsync();
        //    foreach (Frame aFrame in frames)
        //    {
        //        aFrame.BackgroundColor = default;
        //    }
        //    await SetUpFirstRound();
        //}

        void CloseAllFrames()
        {
            object o = new object();
            EventArgs e = new EventArgs();
            if (opaFrame.HeightRequest == 100)
            {
                OnPopoutOpa(o, e);
            }
            if (jaegerFrame.HeightRequest == 100)
            {
                OnPopoutJaeger(o, e);
            }
            if (amorFrame.HeightRequest == 200)
            {
                OnPopoutAmor(o, e);
            }
            if (pennerFrame.HeightRequest == 200)
            {
                OnPopoutPenner(o, e);
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
                if (selection != null && !selection.Equals("Abbrechen"))
                {
                    await GameManagement.SetPlayersRoleAsync(previous, Roles.None);
                    if (!selection.Equals("Keiner"))
                    {
                        await GameManagement.SetPlayersRoleAsync(selection, role);
                    }
                }
                collectionNames.ItemsSource = await GameManagement.GetPlayersAsync(role, (await App.RolesDatabase.GetRoleAsync(role)).Number);
                collectionNames.SelectedItem = null;
            }
            return 1;
        }
        async void OnOpaSelectionChanged(object sender, EventArgs e)
        {
            await ChooseNameAsync(opaNames, Roles.Opa);
        }
        async void OnJaegerSelectionChanged(object sender, EventArgs e)
        {
            await ChooseNameAsync(jaegerNames, Roles.Jäger);
        }
        async void OnAmorSelectionChanged(object sender, EventArgs e)
        {
            await ChooseNameAsync(amorNames, Roles.Amor);
        }
        async void OnPennerSelectionChanged(object sender, EventArgs e)
        {
            await ChooseNameAsync(pennerNames, Roles.Penner);
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

        async void OnPopoutOpa(object sender, EventArgs e)
        {
            Frame item = opaFrame;
            // open
            if (item.HeightRequest == 50)
            {
                CloseAllFrames();
                if (opaNames.ItemsSource != null)
                {
                    if (((List<PlayerItem>)opaNames.ItemsSource).Last().Name.Equals("Keiner"))
                    {
                        await DisplayAlert("Warnung", "Du hast noch nicht alle Spieler für diese Rolle eingetragen.", "Okay");
                    }
                    else
                    {
                        item.HeightRequest = 100;
                    }
                }
            }
            // close
            else
            {
                if (item.BackgroundColor != roleInactiveColor)
                {
                    item.BackgroundColor = roleFinishedColor;
                }
                item.HeightRequest = 50;
            }
        }
        async void OnPopoutJaeger(object sender, EventArgs e)
        {
            Frame item = jaegerFrame;
            // open
            if (item.HeightRequest == 50)
            {
                CloseAllFrames();
                if (jaegerNames.ItemsSource != null)
                {
                    if (((List<PlayerItem>)jaegerNames.ItemsSource).Last().Name.Equals("Keiner"))
                    {
                        await DisplayAlert("Warnung", "Du hast noch nicht alle Spieler für diese Rolle eingetragen.", "Okay");
                    }
                    else
                    {
                        item.HeightRequest = 100;
                    }
                }
            }
            // close
            else
            {
                if (item.BackgroundColor != roleInactiveColor)
                {
                    item.BackgroundColor = roleFinishedColor;
                }
                item.HeightRequest = 50;
            }
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
                    if (item.BackgroundColor != roleInactiveColor)
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
        async void OnPopoutPenner(object sender, EventArgs e)
        {
            Frame item = pennerFrame;
            // open
            if (item.HeightRequest == 50)
            {
                CloseAllFrames();
                if (pennerNames.ItemsSource != null)
                {
                    if (((List<PlayerItem>)pennerNames.ItemsSource).Last().Name.Equals("Keiner"))
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
                if (item.BackgroundColor != roleInactiveColor)
                {
                    item.BackgroundColor = roleFinishedColor;
                }
                item.HeightRequest = 50;
            }
        }
        async void OnAccomodationSelect(object sender, EventArgs e)
        {
            List<string> playerNames = await GameManagement.GetPlayerNamesAsync();
            string selection = await DisplayActionSheet("Quartier auswählen", "Abbrechen", "Brücke", playerNames.ToArray());
            if (selection != null)
            {
                if (selection.Equals("Abbrechen"))
                {
                    return;
                }
                else
                {
                    await GameManagement.SetAccomodationAsync(selection);
                    accomodation.Text = selection;
                }
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
                if (item.BackgroundColor != roleInactiveColor)
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
                if (item.BackgroundColor != roleInactiveColor)
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
                uncoveredName.Text = selection;
                uncoveredRole.Text = (await App.PlayerDatabase.GetPlayerAsync(selection)).Role.ToString();
            }
        }
        async void OnPopoutElection(object sender, EventArgs e)
        {
            Frame item = electionFrame;
            //open
            if (item.HeightRequest == 50)
            {                
                CloseAllFrames();
                bool allRolesFinished = GetAllRolesFinished();
                if(allRolesFinished)
                {
                    // set remaining players to buerger
                    if (Round == 1)
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
                    await GameManagement.SetAccomodationAsync(null);
                    killedNames.ItemsSource = deathPlayer;
                    string win = await GameManagement.CheckWin();
                    if (win != null)
                    {
                        bool input = await DisplayAlert(win, "haben gewonnen!", "Hauptmenü", "Zurück");
                        if (input)
                        {
                            OnBackButtonPressed();
                        }
                        return;
                    }
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
                // Set dayKills
                await GameManagement.GetSetPlayersDeathDayAsync(ListToHashSet((List<string>)onDayKilledNames.ItemsSource));
                string win = await GameManagement.CheckWin();
                if (win != null)
                {
                    bool input2 = await DisplayAlert(win, "haben gewonnen!", "Hauptmenü", "Zurück");
                    if (input2)
                    {
                        OnBackButtonPressed();
                    }
                    //else
                    //{
                    //    OnResetGame(sender, e);
                    //}
                    return;
                }
                await SetupNewRound();

                item.HeightRequest = 50;
            }
        }

        async void OnDayKilledSelectionChanged(object sender, EventArgs e)
        {
            if(onDayKilledNames.SelectedItem != null)
            {
                await SelectDayKill();
                onDayKilledNames.SelectedItem = null;
            }
        }
        async void OnDayKillAdd(object sender, EventArgs e)
        {
            await SelectDayKill();
        }
        async Task<int> SelectDayKill()
        {
            List<string> alivePlayerNames = await GameManagement.GetPlayerNamesAsync();
            List<string> prevItemSource = new List<string>();
            if (onDayKilledNames.ItemsSource != null)
            {
                foreach(string aName in (List<string>)onDayKilledNames.ItemsSource)
                {
                    prevItemSource.Add(aName);
                    alivePlayerNames.Remove(aName);
                }
            }
            string selection = await DisplayActionSheet("Name Auswählen", "Abbrechen", "Keiner", alivePlayerNames.ToArray());
            if (selection != null && !selection.Equals("Abbrechen"))
            {
                if(onDayKilledNames.SelectedItem != null && prevItemSource != null)
                {
                    prevItemSource.Remove((string)onDayKilledNames.SelectedItem);                    
                }
                if (!selection.Equals("Keiner"))
                {
                    prevItemSource.Add(selection);            
                }
                onDayKilledNames.ItemsSource = prevItemSource;

            }
            return 1;
        }
        async Task<HashSet<string>> SetDeathsAndAbilities()
        {
            if(witchSaveSwitch.IsToggled && witchSaveSwitch.IsEnabled && witchVictim.Text == null && witchVictim.IsEnabled)
            {
                witchSaveSwitch.IsEnabled = false;
                return new HashSet<string>{ "Keiner" };
            }
            else
            {
                HashSet<string> result = new HashSet<string> { victim.Text };
                if (witchVictim.Text != null && witchVictim.IsEnabled)
                {
                    result.Add(witchVictim.Text);
                    witchVictim.IsEnabled = false;
                }
                return await GameManagement.GetSetPlayersDeathNightAsync(result);
            }
        }

        HashSet<string> ListToHashSet(List<string> input)
        {

            HashSet<string> result = new HashSet<string>();
            if (input != null) 
            { 
                foreach (string anItem in input)
                {
                    result.Add(anItem);
                }
            }
            return result;
        }

        bool GetAllRolesFinished()
        {
            bool result = true;
            foreach(Frame aFrame in frames)
            {
                if(aFrame.BackgroundColor == default && aFrame != electionFrame && aFrame.IsVisible)
                {
                    result = false;
                }
            }
            return result;
        }
    }
}
