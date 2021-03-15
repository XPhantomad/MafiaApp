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
            string[] o = await database.GetPlayersNoRoleAndPresentAsync();
            //string[] m = new string[4];
            //m[1] = "name11";
            //m[2] = "name 21";
            string selection = await DisplayActionSheet("Name Auswählen", "Abbrechen", "Keiner", o);
            

        }

        async void OnPopout(object sender, EventArgs e)
        {
            var iwas = (PlayerItem)BindingContext;
            MafiaItemDatabase database = await MafiaItemDatabase.Instance;
            await database.DeleteItemAsync(iwas);
            
        }

    }
}
