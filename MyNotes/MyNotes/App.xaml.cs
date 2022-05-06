using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyNotes.Models;
using MyNotes.Services;

namespace MyNotes
{
    public partial class App : Application
    {
        public static ListaNotas Lista { get; set; } = new ListaNotas();
        public static SincronizadorServices Sincronizador { get; set; }

        public App()
        {
            InitializeComponent();

            Sincronizador = new SincronizadorServices(Lista);

            MainPage = new MainPage();
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
