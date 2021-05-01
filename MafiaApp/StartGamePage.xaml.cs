﻿using System;
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

    }

        async void OnSettings(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage());
        }

        async void OnAmorSelectionChanged(object sender, EventArgs e)
        {
            if (amorNames.SelectedItem != null)
            {
                string previous = amorNames.SelectedItem.ToString();
                MafiaItemDatabase database = await MafiaItemDatabase.Instance;
                string[] playerNames = await database.GetPlayersNoRoleAndPresentAsync();
                string selection = await DisplayActionSheet("Name Auswählen", "Abbrechen", "Keiner", playerNames);
                if (selection.Equals("Keiner"))
                {
                    await database.SetPlayersRoleAsync(previous, roles.None);
                }
                else if (!selection.Equals("Abbrechen"))
                {
                    await database.SetPlayersRoleAsync(previous, roles.None);
                    await database.SetPlayersRoleAsync(selection, roles.Amor);
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

        void OnPopoutAmor(object sender, EventArgs e)
        {
            Frame item = amorFrame;
            if (item.HeightRequest == 50)
            {
                item.HeightRequest = 200;
            }
            else
            {
                item.HeightRequest = 50;
            }  
        }
        void OnPopoutMafia(object sender, EventArgs e)
        {
            Frame item = mafiaFrame;
            if (item.HeightRequest == 50)
            {
                item.HeightRequest = 200;
            }
            else
            {
                item.HeightRequest = 50;
            }
        }

        async void OnMafiaSelectionChanged(object sender, EventArgs e)
        {
            if (mafiaNames.SelectedItem != null)
            {
                string previous = mafiaNames.SelectedItem.ToString();
                MafiaItemDatabase database = await MafiaItemDatabase.Instance;
                string[] playerNames = await database.GetPlayersNoRoleAndPresentAsync();
                string selection = await DisplayActionSheet("Name Auswählen", "Abbrechen", "Keiner", playerNames);
                if (selection.Equals("Keiner"))
                {
                    await database.SetPlayersRoleAsync(previous, roles.None);
                }
                else if (!selection.Equals("Abbrechen"))
                {
                    await database.SetPlayersRoleAsync(previous, roles.None);
                    await database.SetPlayersRoleAsync(selection, roles.Mafia);
                }
                mafiaNames.ItemsSource = await database.GetPlayersByRoleAndNumberAsync(roles.Mafia, await database.GetRoleNumber(roles.Amor));
                mafiaNames.SelectedItem = null;
            }       
            
        }

        async void OnVictimSelect(object sender, EventArgs e)
        {
            string prev = victim.Text;
            MafiaItemDatabase database = await MafiaItemDatabase.Instance;
            string[] playerNames = await database.GetPlayersPresentAndAliveAsync();
            string selection = await DisplayActionSheet("Opfer Auswählen", "Abbrechen", null, playerNames);
            if (selection.Equals("Abbrechen"))
            {
                return;
            }
            else
            {
                await database.SetPlayerVictimAsync(selection, true);
                await database.SetPlayerVictimAsync(prev, false);
                victim.Text = selection;
                showVictim.Text = selection;
            }
        }

        async void OnPopoutHexe(object sender, EventArgs e)
        {
            MafiaItemDatabase database = await MafiaItemDatabase.Instance;
            Frame item = hexeFrame;
            if (item.HeightRequest == 50)
            {
                item.HeightRequest = 200;
            }
            else
            {
                item.HeightRequest = 50;
                if (witchSaveSwitch.IsToggled == true && witchSaveSwitch.IsEnabled == true)
                {
                    await database.SetPlayerVictimAsync(showVictim.Text, false);
                    witchSaveSwitch.IsEnabled = false;
                }
                else
                {
                    await database.SetPlayerLiveDown(showVictim.Text);      // sterben des Spielers in log
                    await database.SetPlayerVictimAsync(showVictim.Text, false);
                }

                if (witchKill.Text != null)
                {
                    await database.SetPlayerLiveDown(witchKill.Text);
                    witchKill.IsEnabled = false;
                }
            }
        }
        async void OnWitchKill(object sender, EventArgs e)
        {

            MafiaItemDatabase database = await MafiaItemDatabase.Instance;
            string[] playerNames = await database.GetPlayersNoRoleAndPresentAsync();
            string selection = await DisplayActionSheet("Name Auswählen", "Abbrechen", "Keiner", playerNames);
            if (selection.Equals("Keiner"))
            {
                witchKill.Text = null;
            }
            else if (!selection.Equals("Abbrechen"))
            {
                witchKill.Text = selection;
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
                if (selection.Equals("Keiner"))
                {
                    await database.SetPlayersRoleAsync(previous, roles.None);
                }
                else if (!selection.Equals("Abbrechen"))
                {
                    await database.SetPlayersRoleAsync(previous, roles.None);
                    await database.SetPlayersRoleAsync(selection, roles.Hexe);
                }
                hexeNames.ItemsSource = await database.GetPlayersByRoleAndNumberAsync(roles.Hexe, await database.GetRoleNumber(roles.Hexe));
                hexeNames.SelectedItem = null;
            }
        }
    }
}
