using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SQLite;
//using :::no - loc(Xamarin.Forms):::;

namespace MafiaApp
{
    public partial class StartGamePage : ContentPage
    {
        //string _fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "notes.txt");
        public StartGamePage()
        {
            InitializeComponent();
            /*
            if (File.Exists(_fileName))
            {
                editor.Text = File.ReadAllText(_fileName);
            }
            */
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();

            MafiaItemDatabase database = await MafiaItemDatabase.Instance;
            BindingContext = new PlayerItem();
        }


        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            //File.WriteAllText(_fileName, editor.Text);

            var irgendwas = (PlayerItem)BindingContext;
            MafiaItemDatabase database = await MafiaItemDatabase.Instance;
            await database.SaveItemAsync(irgendwas);
            await Navigation.PopAsync();

        }

        async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var iwas = (PlayerItem)BindingContext;
            MafiaItemDatabase database = await MafiaItemDatabase.Instance;
            await database.DeleteItemAsync(iwas);
            await Navigation.PopAsync();
            
            /*
             if (File.Exists(_fileName))
             {
                 File.Delete(_fileName);
             }
             editor.Text = string.Empty;
            */

        }
    }
}
