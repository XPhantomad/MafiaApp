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
            amor.Text = (await database.GetPlayersByRoleAsync(roles.Amor))[0]; // 1 von String 
            
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

        void OnPopout(object sender, EventArgs e)
        {
            if (amorFrame.HeightRequest == 50)
            {
                amorFrame.HeightRequest = 200;
            }
            else
            {
                amorFrame.HeightRequest = 50;
            }  
        }
    }
}
