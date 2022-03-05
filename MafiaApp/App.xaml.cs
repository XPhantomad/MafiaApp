using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MafiaApp
{
    public partial class App : Application
    {

        static PlayerDatabase playerDatabase;

        public static PlayerDatabase PlayerDatabase
        {
            get
            {
                if (playerDatabase == null)
                {
                    playerDatabase = new PlayerDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "players.db3"));
                }
                return playerDatabase;
            }
        }

        static RolesDatabase rolesDatabase;

        public static RolesDatabase RolesDatabase
        {
            get
            {
                if (rolesDatabase == null)
                {
                    rolesDatabase = new RolesDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "roles.db3"));
                }
                return rolesDatabase;
            }
        }

        public App()
        {
            InitializeComponent();

            MainPage = new TabPage();
            
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
