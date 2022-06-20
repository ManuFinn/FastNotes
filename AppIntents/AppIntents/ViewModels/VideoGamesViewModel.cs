using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using AppIntents.Models;
using AppIntents.Views;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppIntents.ViewModels
{
    class VideoGamesViewModel : INotifyPropertyChanged
    {
        AgregarVG vistaAgregar;
        EditarVG vistaEditar;

        public ObservableCollection<VideogameT> Lista { get; set; } = new ObservableCollection<VideogameT>();

        VideogameT vgT;

        public VideogameT VGT
        {
            get { return vgT; }
            set { vgT = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(vgT))); }
        }

        private string error;

        public string Error 
        { 
            get { return error; }
            set { error = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Error))); } 
        }

        static HttpClient client = new HttpClient()
        {
            BaseAddress = new Uri("https://181g0250.81g.itesrc.net")
        };

        public Command AgregarCommand { get; set; }
        public Command EditarCommand { get; set; }
        public Command EliminarCommand { get; set; }
        public Command CancelarCommand { get; set; }

        public VideoGamesViewModel()
        {
            CargarDatos();
            CancelarCommand = new Command(Cancelar);
            EditarCommand = new Command(Editar);
            EliminarCommand = new Command(Eliminar);
            AgregarCommand = new Command(Agregar);
        }

        async void CargarDatos()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                var result = await client.GetAsync("api/VideoGames");
                string json = await result.Content.ReadAsStringAsync();

                List<VideogameT> productos = JsonConvert.DeserializeObject<List<VideogameT>>(json);

                foreach (var item in productos)
                {
                    Lista.Add(item);
                }
            }
            else { Error = "No esta conectado a internet..."; }
        }

        private async void VerAgregarAsync()
        {
            if (vistaAgregar == null) { vistaAgregar = new AgregarVG() { BindingContext = this }; }

            VGT = new VideogameT();
            Error = null;

            await Application.Current.MainPage.Navigation.PushAsync(vistaAgregar);
        }

        private async void VerEditarAsync(object vg)
        {
            if (vistaEditar == null) { vistaEditar = new EditarVG() { BindingContext = this }; }

            VGT = vg as VideogameT;
            Error = null;

            await Application.Current.MainPage.Navigation.PushAsync(vistaEditar);
        }

        private async void Agregar()
        {
            //var result = await App.Sincronizador.Agregar(Nota);

            //if (result == null) { await Application.Current.MainPage.Navigation.PopAsync(); }
            //else { Errors = result.Select(x => new ErrorModel { Error = x }).ToList(); }
        }

        private async void Eliminar(object n)
        {
            //var nota = n as Notas;
            //var x = await Application.Current.MainPage.DisplayAlert("Confirmar:", $"Seguro de eliminar la nota?", "Si", "No");
            //if (x)
            //{
            //    var res = await App.Sincronizador.Eliminar(nota);
            //    if (res != null) { await Application.Current.MainPage.DisplayAlert("Error", String.Join("\n", res), "Aceptar"); }
            //}
        }

        private async void Editar()
        {
            //var res = await App.Sincronizador.Editar(Nota);
            //if (res == null) { await Application.Current.MainPage.Navigation.PopAsync(); }
            //else { Errors = res.Select(x => new ErrorModel { Error = x }).ToList(); }
        }

        private void Cancelar()
        {
            //Nota = null;
            //Application.Current.MainPage.Navigation.PopAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
