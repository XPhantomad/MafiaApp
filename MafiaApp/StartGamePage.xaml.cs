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
            BindingContext = await database.GetPlayersAsync();
        }

        async void OnChangeNameAmor(object sender, EventArgs e)
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
        async void OnSpouseChange(object sender, EventArgs e)
        {
            MafiaItemDatabase database = await MafiaItemDatabase.Instance;
            string[] playerNames = await database.GetPlayersUnmarriedAsync(null);
            string s1 = await DisplayActionSheet("Ehepartner 1 auswählen", "Abbrechen", null, playerNames);
            if (s1.Equals("Abbrechen"))
            {
                return;
            }
            playerNames = await database.GetPlayersUnmarriedAsync(s1);
            string s2 = await DisplayActionSheet("Ehepartner 1 auswählen", "Abbrechen", null, playerNames);
            if (!s1.Equals("Abbrechen") && !s2.Equals("Abbrechen"))
            {
                await database.SetPlayerSpouseAsync(s1, s2);

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
