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
    public partial class SettingsPage : ContentPage
    {
        int playerPresentNumberC;

        public SettingsPage()
        {
            InitializeComponent();

        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            MafiaItemDatabase database = await MafiaItemDatabase.Instance;
            //database.DeleteRolesTableAsync();
            await database.SetUp();
            playerPresentNumberC = (await database.GetPlayersPresentAsync()).Count;
            playerPresentNumber.Text = playerPresentNumberC.ToString();
            rolesView.ItemsSource = await database.GetRolesAsync();
            await database.SetRoleNumbersAuto(playerPresentNumberC);
        }

        void OnItemSelected(object sender, EventArgs e)
        {

        }
    }
}