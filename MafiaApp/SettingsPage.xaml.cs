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
            //await database.DeleteRolesTableAsync();
            if((await App.RolesDatabase.GetRolesAsync()).Count == 0)
            {
                await RolesInitializer.Initialize();
            }

            playerPresentNumber.Text = (await App.PlayerDatabase.GetPlayersAsync()).Count.ToString();
            rolesView.ItemsSource = await App.RolesDatabase.GetRolesAsync();
        }

        async void OnAutoSwitch(object sender, EventArgs e)
        {
            //MafiaItemDatabase database = await MafiaItemDatabase.Instance;
            //await database.SetRoleNumbersAuto();
            //rolesView.ItemsSource = await database.GetRolesAsync();
        }

        async void OnReset(object sender, EventArgs e)
        {
            //if (rolesView.SelectedItem != null)
            //{
            //    MafiaItemDatabase database = await MafiaItemDatabase.Instance;
            //    await database.DeleteRolesTableAsync((RolesItem)rolesView.SelectedItem);
            //    rolesView.ItemsSource = await database.GetRolesAsync();
            //}
        }
        //async void OnAbilitiesShow(object sender, EventArgs e)
        //{

        //}

        async void OnIncrease(object sender, EventArgs e) //erhöhen Bürger werden automatisch weniger
        {
            //if (rolesView.SelectedItem != null)
            //{
            //    MafiaItemDatabase database = await MafiaItemDatabase.Instance;
            //    int numbBuerger = await database.GetRoleNumber(roles.Bürger);
            //    if ((numbBuerger == 0) || ((RolesItem)rolesView.SelectedItem).Role == roles.Bürger)
            //    {
            //        rolesView.SelectedItem = null;
            //        return;// noch Warnung einfügen
            //    }
            //    else
            //        await database.SetRoleNumbersManual(((RolesItem)rolesView.SelectedItem).Role, true);
            //    rolesView.ItemsSource = await database.GetRolesAsync();
            //}
            //else
            //    await DisplayAlert("Warnung", "Wähle zuerst eine Person aus", "Okay");
            //rolesView.SelectedItem = null;
        }

        async void OnDecrease(object sender, EventArgs e) //verringern, Bürger werden automatisch mehr
        {
        //    if (rolesView.SelectedItem != null)
        //    {
        //        MafiaItemDatabase database = await MafiaItemDatabase.Instance;
        //        int numbRole = await database.GetRoleNumber(((RolesItem)rolesView.SelectedItem).Role);
        //        if ((numbRole == 0) || ((RolesItem)rolesView.SelectedItem).Role == roles.Bürger)
        //        {
        //            rolesView.SelectedItem = null;
        //            return; //noch eine Warnung machen
        //        }
        //        else
        //            await database.SetRoleNumbersManual(((RolesItem)rolesView.SelectedItem).Role, false);
        //        rolesView.ItemsSource = await database.GetRolesAsync();
        //    }
        //    else
        //        await DisplayAlert("Warnung", "Wähle zuerst eine Rolle aus", "Okay");
        //    rolesView.SelectedItem = null;
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {

            }
        }
    }
}