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
        public SettingsPage()
        {
            InitializeComponent();

        }

        public async void PlayerPresent()
        {
            MafiaItemDatabase database = await MafiaItemDatabase.Instance;
            List<PlayerItem> names = await database.GetPlayersPresentAsync();
            int presentPlayerNumber = names.Count;
          
        }
    }
}