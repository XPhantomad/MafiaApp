using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MafiaApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayerPage : ContentPage
    {

        

        public PlayerPage()
        {
            InitializeComponent();
         
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            MafiaItemDatabase database = await MafiaItemDatabase.Instance;
            player.ItemsSource = await database.GetPlayerAsync();
        }

        async void OnPlayerAdded(object sender, EventArgs e)
        {

            string newplayer = await DisplayPromptAsync("Namen Eingeben", "hier soll keine unterschrift hin");
            MafiaItemDatabase database = await MafiaItemDatabase.Instance;
            await database.SavePlayerAsync(newplayer);
            player.ItemsSource = await database.GetPlayerAsync();
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

                Console.WriteLine("Items Selected");
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
                Console.WriteLine("Löschen Button gedrückt");
                MafiaItemDatabase database = await MafiaItemDatabase.Instance;
                PlayerItem del = (PlayerItem)player.SelectedItem;
                await database.DeletePlayerAsync(del);
                player.ItemsSource = await database.GetPlayerAsync();
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
                ausgabe.Text = "Löschen Button gedrückt";
                MafiaItemDatabase database = await MafiaItemDatabase.Instance;
                PlayerItem pres = (PlayerItem)player.SelectedItem;
                await database.ChangePlayerPresentAsync(pres);
                player.ItemsSource = await database.GetPlayerAsync();
                player.SelectedItem = null;    // damit Alert wieder eirscheint wenn nichts ausgewählt ist
            }
            else
            {
                await DisplayAlert("Warnung", "Wähle zuerst eine Person aus", "Oaky");
            }
        }

        //mehrere Löschen Funktion dafür neue Datenbank funtkion schreiben dafür Task ansehen
        // alle abwesend funktion
        // ausgewählte anwesend funktion





    }
}