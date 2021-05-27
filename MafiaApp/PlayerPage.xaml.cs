using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MafiaApp
{
    public partial class PlayerPage : ContentPage
    {

        bool presentPlayersShown = false;

        public PlayerPage()
        {
            InitializeComponent();
            //Wenn die Button angetippt wird, wird die OnButtonClicked - Methode ausgeführt.Das - 
            //    sender Argument ist das Button für dieses Ereignis verantwortliche Objekt.Sie 
            //    können dies verwenden, um auf das Button Objekt zuzugreifen, oder um zwischen 
            //    mehreren Objekten zu unterscheiden, Button die dasselbe Clicked Ereignis verwenden.
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            MafiaItemDatabase database = await MafiaItemDatabase.Instance;
            player.ItemsSource = await database.GetPlayersAsync();
        }

        async void OnPlayerAdded(object sender, EventArgs e)
        {

            string newplayer = await DisplayPromptAsync("Namen Eingeben", null);
            MafiaItemDatabase database = await MafiaItemDatabase.Instance;
            await database.SavePlayerAsync(newplayer);
            player.ItemsSource = await database.GetPlayersAsync();
        }
        
         async void OnItemsSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (player.SelectedItem != null)
            {
                MafiaItemDatabase database = await MafiaItemDatabase.Instance;
                //dps = (MafiaItemDatabase)player.SelectedItems;
                PlayerItem pla = (PlayerItem)player.SelectedItem;
                if (pla.Present == true)
                {
                    present.Text = "----";
                }
                else
                {
                    present.Text = "++++";
                }

                // Knopf für Anwesenheit ändern
  
            }
            else //aussehen des Knopfes für nichts ausgewählt hinzufügen
            {
                present.Text = "none";
            }
           
        }
        async void OnPlayerDelete(object sender, EventArgs e)
        {
            if (player.SelectedItem != null)
            { 
                MafiaItemDatabase database = await MafiaItemDatabase.Instance;
                PlayerItem del = (PlayerItem)player.SelectedItem;
                await database.DeletePlayerAsync(del);
                player.ItemsSource = await database.GetPlayersAsync();
                player.SelectedItem = null;    // damit Alert wieder eirscheint wenn nichts ausgewählt ist
            }
            else
            {
                await DisplayAlert("Warnung", "Wähle zuerst eine Person aus", "Oaky");
            } 
        }

        async void OnPlayerPresent(object sender, EventArgs e)
        {
            if (player.SelectedItem != null)
            {
                MafiaItemDatabase database = await MafiaItemDatabase.Instance;
                PlayerItem pres = (PlayerItem)player.SelectedItem;
                await database.ChangePlayerPresentAsync(pres);
                player.ItemsSource = await database.GetPlayersAsync();
                player.SelectedItem = null;    // damit Alert wieder eirscheint wenn nichts ausgewählt ist
            }
            else
            {
                await DisplayAlert("Warnung", "Wähle zuerst eine Person aus", "Oaky");
            }
        }
        async void OnPlayerSorted(object sender, EventArgs e)
        {
            MafiaItemDatabase database = await MafiaItemDatabase.Instance;
            string action = await DisplayActionSheet("Nach Name sortieren.", "Abbrechen", "Aufheben", "Aufsteigend", "Absteigend");
            if (action.Equals("Aufsteigend")) {
                player.ItemsSource = await database.GetSortedPlayersAsync(true);
            }
            else if (action.Equals("Absteigend")){
                player.ItemsSource = await database.GetSortedPlayersAsync(false);
            }
            else if (action.Equals("Aufheben")){
                player.ItemsSource = await database.GetPlayersAsync();
            }
            // bei Abbrechen soll es so sortiert bleiben
            // idee: Überladung der GetPlayersAsync Methode um den bool wert für auf und absteigend
        }

        async void OnShowPresentPlayers(object sender, EventArgs e)
        {
            MafiaItemDatabase database = await MafiaItemDatabase.Instance;
            presentPlayersShown = !(presentPlayersShown);
            if (presentPlayersShown == true)
            {
                player.ItemsSource = await database.GetPlayersPresentAsync();
                ShowPresentPlayers.Text = "Anwesende";
            }
            else
            {
                player.ItemsSource = await database.GetPlayersAsync();
                ShowPresentPlayers.Text = "Alle";
            }
            
        }

        // wie modus um mitspieler Festzulegen
        // knopf sagt jetzt hinzufügen, dann verändert sich was, dann bewirkt klick auf eine Person, dass status geändert wird und danach wieder ausgewählte auf null gesetzt damit man nächstes auswählen kann!!!!!

        //mehrere Löschen Funktion dafür neue Datenbank funtkion schreiben dafür Task ansehen
        // alle abwesend funktion
        // ausgewählte anwesend funktion





    }
}