using AppIntents.Models;
using AppIntents.Services;
using Plugin.FirebasePushNotification;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppIntents
{
    public partial class App : Application
    {
        public static CatalogoVideoGames Catalogo { get; set; } = new CatalogoVideoGames();

        public static SincronizadorServices Sincronizador { get; set; }

        public App()
        {
            InitializeComponent();

            Sincronizador = new SincronizadorServices(Catalogo);

            MainPage = new NavigationPage(new Views.VideoGamesView());


            CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine($"TOKEN : {p.Token}");

                var token = p.Token;

                var token2 = p.Token;

            };

            

            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {

                System.Diagnostics.Debug.WriteLine("Received");
                foreach (var data in p.Data)
                {
                    System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                }

            };
            CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Opened");
                foreach (var data in p.Data)
                {
                    System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                }


            };

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
