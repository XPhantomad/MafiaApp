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
            var irgendwas = (PlayerItem)BindingContext;
            MafiaItemDatabase database = await MafiaItemDatabase.Instance;
            await database.SaveItemAsync(irgendwas);
            await Navigation.PopAsync();

        }

        async void OnPopout(object sender, EventArgs e)
        {
            var iwas = (PlayerItem)BindingContext;
            MafiaItemDatabase database = await MafiaItemDatabase.Instance;
            await database.DeleteItemAsync(iwas);
            await Navigation.PopAsync();
        }

    }
}
