using LoginApp.Services;
using LoginApp.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LoginApp
{
    public partial class App : Application
    {
        public static UserService User { get; private set; } = new UserService();

        public App()
        {
            InitializeComponent();

            if (User.IsLoggedIn || User.Renovar().Result) { User.BackToLogin(); }
            else { MainPage = new LoginView(); }

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
