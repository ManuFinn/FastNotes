using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyNotes.Models;
using MyNotes.Services;
using MarcTron.Plugin.Controls;

[assembly: ExportFont("LDFComicSans.otf", Alias = "ComicSans")]
namespace MyNotes
{
    public partial class App : Application
    {
        public static ListaNotas Lista { get; set; } = new ListaNotas();
        public static SincronizadorServices Sincronizador { get; set; }

        public App()
        {
            InitializeComponent();

            MTAdView ads = new MTAdView();

            Sincronizador = new SincronizadorServices(Lista);

            MainPage = new NavigationPage(new Views.NotasView());
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
