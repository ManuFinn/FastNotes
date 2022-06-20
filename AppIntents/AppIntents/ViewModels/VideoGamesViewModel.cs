using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using AppIntents.Models;
using AppIntents.Services;
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

        public ObservableCollection<VideogameT> Catalogo { get; set; } = new ObservableCollection<VideogameT>();

        VideogameT vgT;

        public VideogameT VideoGame
        {
            get { return vgT; }
            set { vgT = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VideoGame))); }
        }

        private List<ErrorModel> errors;

        public List<ErrorModel> Errors
        {
            get { return errors; }
            set { errors = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Errors))); }
        }

        public Command VistaAgregarCommand { get; set; }
        public Command VistaEditarCommand { get; set; }
        public Command AgregarCommand { get; set; }
        public Command EditarCommand { get; set; }
        public Command EliminarCommand { get; set; }
        public Command CancelarCommand { get; set; }

        public VideoGamesViewModel()
        {
            CancelarCommand = new Command(Cancelar);
            EditarCommand = new Command(Editar);
            EliminarCommand = new Command(Eliminar);
            AgregarCommand = new Command(Agregar);
            VistaAgregarCommand = new Command(VerAgregarAsync);
            VistaEditarCommand = new Command(VerEditarAsync);

            SincronizadorServices.ActuializacionRealizada += SincronizadorService_ActualizacionRealizada;
            SincronizadorService_ActualizacionRealizada();
        }

        private void SincronizadorService_ActualizacionRealizada()
        {
            var videgogos = App.Catalogo.GetAll().ToList();

            foreach (var vge in App.Sincronizador.Buffer)
            {
                if (vge.Estado == Estado.Agregado) { videgogos.Insert(0, vge.VideoGameT); }
                else if (vge.Estado == Estado.Modificado)
                {
                    var vg = videgogos.FirstOrDefault(x => x.Id == vge.VideoGameT.Id);
                    if (vg != null)
                    {
                        vg.NombreVg = vge.VideoGameT.NombreVg;
                        vg.DescripcionVg = vge.VideoGameT.DescripcionVg;
                        vg.PortadaVg = vge.VideoGameT.PortadaVg;
                        vg.FechaSalidaVg = vge.VideoGameT.FechaSalidaVg;
                    }
                }
                else
                {
                    var vg = Catalogo.FirstOrDefault(x => x.Id == vge.VideoGameT.Id);
                    if (vg != null) { videgogos.Remove(vg); }
                }
            }

            Catalogo.Clear();
            foreach (var item in videgogos.OrderByDescending(x => x.Id))
            {
                Catalogo.Add(item);
            }
        }

        private async void VerAgregarAsync()
        {
            if (vistaAgregar == null) { vistaAgregar = new AgregarVG() { BindingContext = this }; }

            VideoGame = new VideogameT();
            Errors = null;

            await Application.Current.MainPage.Navigation.PushAsync(vistaAgregar);
        }

        private async void VerEditarAsync(object vg)
        {
            if (vistaEditar == null) { vistaEditar = new EditarVG() { BindingContext = this }; }

            VideoGame = vg as VideogameT;
            Errors = null;

            await Application.Current.MainPage.Navigation.PushAsync(vistaEditar);
        }

        private async void Agregar()
        {
            var result = await App.Sincronizador.Agregar(VideoGame);

            if (result == null) { await Application.Current.MainPage.Navigation.PopAsync(); }
            else { Errors = result.Select(x => new ErrorModel { Error = x }).ToList(); }
        }

        private async void Eliminar(object n)
        {
            var nota = n as VideogameT;
            var x = await Application.Current.MainPage.DisplayAlert("Confirmar:", $"Seguro de eliminar la nota?", "Si", "No");
            if (x)
            {
                var res = await App.Sincronizador.Eliminar(nota);
                if (res != null) { await Application.Current.MainPage.DisplayAlert("Error", String.Join("\n", res), "Aceptar"); }
            }
        }

        private async void Editar()
        {
            var res = await App.Sincronizador.Editar(VideoGame);
            if (res == null) { await Application.Current.MainPage.Navigation.PopAsync(); }
            else { Errors = res.Select(x => new ErrorModel { Error = x }).ToList(); }
        }

        private void Cancelar()
        {
            VideoGame = null;
            Application.Current.MainPage.Navigation.PopAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
