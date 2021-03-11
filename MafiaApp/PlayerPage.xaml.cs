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
            MafiaItemDatabase dps = await MafiaItemDatabase.Instance;
            //dps = (MafiaItemDatabase)player.SelectedItems;

            Console.WriteLine("Items Selected");
           
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
        // alle abwesend funktion
        // ausgewählte anwesend funktion

        
        

        
    }
}