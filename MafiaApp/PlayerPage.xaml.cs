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
            player.ItemsSource = await database.GetItemsAsync();
        }

        async void OnPlayerAdded(object sender, EventArgs e)
        {
            string newplayer = await DisplayPromptAsync("Namen Eingeben", "hier soll keine unterschrift hin");
            await 
        }
    }
}