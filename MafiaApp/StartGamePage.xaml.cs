using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SQLite;


namespace MafiaApp
{
    public partial class StartGamePage : ContentPage
    {
        

        public StartGamePage()
        {
            InitializeComponent();
          
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();

            MafiaItemDatabase database = await MafiaItemDatabase.Instance;
            BindingContext = new PlayerItem();
        }

        async void OnChangeName(object sender, EventArgs e)
        {
            MafiaItemDatabase database = await MafiaItemDatabase.Instance;
            string[] playerNames = await database.GetPlayersNoRoleAndPresentAsync();
            string selection = await DisplayActionSheet("Name Auswählen", "Abbrechen", "Keiner", playerNames);
            if (selection.Equals("Keiner"))
            {
                amor.Text = "None";
            }
            else if (!selection.Equals("Abbrechen"))
            {
                amor.Text = selection;
                await database.SetPlayersRoleAsync(selection, roles.Amor);
            }

        }

        async void OnPopout(object sender, EventArgs e)
        {
            var iwas = (PlayerItem)BindingContext;
            MafiaItemDatabase database = await MafiaItemDatabase.Instance;
            await database.DeleteItemAsync(iwas);
            
        }

    }
}
