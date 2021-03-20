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

        public StartGamePage()
        {
            InitializeComponent();
            
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            MafiaItemDatabase database = await MafiaItemDatabase.Instance;
            //amorNames.ItemsSource = await database.GetPlayersByRoleAsync(roles.Amor);  
            mafiaNames.ItemsSource = await database.GetPlayersByRoleAsyncSTR(roles.Mafia); // gewünschte Anzahl dahinter
            mafiaNames.
        }

        async void OnChangeNameAmor(object sender, EventArgs e)
        {
            
            string previous = amor.Text;
            MafiaItemDatabase database = await MafiaItemDatabase.Instance;
            string[] playerNames = await database.GetPlayersNoRoleAndPresentAsync();
            string selection = await DisplayActionSheet("Name Auswählen", "Abbrechen", "Keiner", playerNames);
            if (selection.Equals("Keiner"))
            {
                amor.Text = "None";
                await database.SetPlayersRoleAsync(previous, roles.None);
            }
            else if (!selection.Equals("Abbrechen"))
            {
                amor.Text = selection;
                await database.SetPlayersRoleAsync(previous, roles.None);
                await database.SetPlayersRoleAsync(selection, roles.Amor);
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
            string s2 = await DisplayActionSheet("Ehepartner 1 auswählen", "Abbrechen", null, playerNames);
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
            PlayerItem pre = (PlayerItem) mafiaNames.SelectedItem;
            string previous = pre.Name;
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
            mafiaNames.ItemsSource = await database.GetPlayersByRoleAsync(roles.Mafia);
        }
    }
}
