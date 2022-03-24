using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public SettingsPage()
        {
            InitializeComponent();

        }
        protected override async void OnAppearing()
        {
            rolesView.ItemsSource = await App.RolesDatabase.GetRolesAsync();
            base.OnAppearing();
            //await database.DeleteRolesTableAsync();
            if((await App.RolesDatabase.GetRolesAsync()).Count == 0)
            {
                await RolesInitializer.Initialize();
            }
            int playersPresentNumber = (await App.PlayerDatabase.GetPlayersPresentAliveAsync()).Count;
            playersPresentNumberDisp.Text = playersPresentNumber.ToString();

            await RolesInitializer.SetNumbersAuto(playersPresentNumber);
            rolesView.ItemsSource = await App.RolesDatabase.GetRolesAsync();

        }
        async void OnStartGame(object sender, EventArgs e)
        {
            if((await App.RolesDatabase.GetRoleAsync(Roles.Mafia)).Number <= 0)
            {
                await DisplayAlert("Warnung", "Zahl der Mafias darf nicht 0 sein", "Okay");
                return;
            }
            await Navigation.PushModalAsync(new StartGamePage());
        }

        async void OnAutoSwitch(object sender, EventArgs e)
        {
            //MafiaItemDatabase database = await MafiaItemDatabase.Instance;
            //await database.SetRoleNumbersAuto();
            //rolesView.ItemsSource = await database.GetRolesAsync();
        }

        //async void OnReset(object sender, EventArgs e)
        //{
        //    if (rolesView.SelectedItem != null)
        //    {
        //        await App.RolesDatabase.DeleteRoleAsync((RolesItem)rolesView.SelectedItem);
        //        rolesView.ItemsSource = await App.RolesDatabase.GetRolesAsync();
        //    }
        //}

        async void OnAbilitiesShow(object sender, EventArgs e)
        {
            if (rolesView.SelectedItem != null)
                await DisplayAlert("Fähigkeit:", ((RolesItem)rolesView.SelectedItem).Ability, "Okay");
            else
                await DisplayAlert("Warnung", "Wähle zuerst eine Rolle aus", "Okay");
        }

        async void OnIncrease(object sender, EventArgs e) //erhöhen Bürger werden automatisch weniger
        {
            object selecItem = rolesView.SelectedItem;
            if (rolesView.SelectedItem != null)
            {
                int numbBuerger = (await App.RolesDatabase.GetRoleAsync(Roles.Bürger)).Number;
                if ((numbBuerger == 0) || ((RolesItem)selecItem).Role == Roles.Bürger)
                {
                    rolesView.SelectedItem = null;
                    return;// noch Warnung einfügen
                }
                else
                    await RolesInitializer.SetNumbersManual(((RolesItem)selecItem).Role, true);
                rolesView.ItemsSource = await App.RolesDatabase.GetRolesAsync();
            }
            else
                await DisplayAlert("Warnung", "Wähle zuerst eine Rolle aus", "Okay");
            rolesView.SelectedItem = null;
        }

        async void OnDecrease(object sender, EventArgs e) //verringern, Bürger werden automatisch mehr
        {
            object selecItem = rolesView.SelectedItem;
            if (selecItem != null)
            {
                int numbRole = (await App.RolesDatabase.GetRoleAsync(((RolesItem)selecItem).Role)).Number;
                if ((numbRole == 0) || ((RolesItem)selecItem).Role == Roles.Bürger)
                {
                    rolesView.SelectedItem = null;
                    return; //noch eine Warnung machen
                }
                else
                    await RolesInitializer.SetNumbersManual(((RolesItem)rolesView.SelectedItem).Role, false);
                rolesView.ItemsSource = await App.RolesDatabase.GetRolesAsync();
            }
            else
                await DisplayAlert("Warnung", "Wähle zuerst eine Rolle aus", "Okay");
            rolesView.SelectedItem = null;
        }
    }
}