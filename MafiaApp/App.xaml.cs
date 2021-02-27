using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MafiaApp
{
    public partial class App : Application
    {
        static MafiaItemDatabase database;
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new StartGamePage());
        }

        public static MafiaItemDatabase Database
        {
            get
            {
                if (database == null)
                { 
                    database = new MafiaItemDatabase(); 
                }
                return database;
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
