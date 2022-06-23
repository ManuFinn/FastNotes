using MarcTron.Plugin;
using MyNotes.Models;
using MyNotes.Services;
using MyNotes.Views;
using Plugin.LatestVersion;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace MyNotes.ViewModels
{
    public class NotasViewModel : INotifyPropertyChanged
    {
        AgregarNotaView vistaAgregar;
        EditarNotaView vistaEditar;
        InfoView vistaInfo;

        Notas notas1;

        public Notas Nota
        {
            get { return notas1; }
            set { notas1 = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Nota))); }
        }

        private List<ErrorModel> errors;

        public List<ErrorModel> Errors
        {
            get { return errors; }
            set { errors = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Errors)));}
        }

        public ObservableCollection<Notas> Notas { get; set; } = new ObservableCollection<Notas>();

        public Command VistaAgregarCommand { get; set; }
        public Command VistaEditarCommand { get; set; }
        public Command AgregarCommand { get; set; }
        public Command EditarCommand { get; set; }
        public Command EliminarCommand { get; set; }
        public Command CancelarCommand { get; set; }
        public Command InfoCommand { get; set; }
        public Command VerficicarUpdatesCommand { get; set; }

        public Command apoyoAnuncioCommand { get; set; }

        public NotasViewModel()
        {
            CancelarCommand = new Command(Cancelar);
            EditarCommand = new Command(Editar);
            EliminarCommand = new Command(Eliminar);
            AgregarCommand = new Command(Agregar);
            VistaAgregarCommand = new Command(VerAgregarAsync);
            VistaEditarCommand = new Command(VerEditarAsync);
            InfoCommand = new Command(verInfo);
            apoyoAnuncioCommand = new Command(mostrarApoyo);
            VerficicarUpdatesCommand = new Command(VerificarUpdates);

            CrossMTAdmob.Current.LoadInterstitial("ca-app-pub-3940256099942544/1033173712");
            

            SincronizadorServices.ActuializacionRealizada += SincronizadorService_ActualizacionRealizada;
            SincronizadorService_ActualizacionRealizada();
        }

        private void mostrarApoyo()
        {
            CrossMTAdmob.Current.LoadRewardedVideo("ca-app-pub-3940256099942544/5224354917");
            CrossMTAdmob.Current.ShowRewardedVideo();
        }

        private async void VerificarUpdates()
        {
            var isLatest = await CrossLatestVersion.Current.IsUsingLatestVersion();

            if (!isLatest)
            {
                var update = await Application.Current.MainPage.DisplayAlert("Nueva version disponible", "Hola, existe una nueva version en la PlayStore, te gustaria instalarla?", "Si", "No");

                if (update)
                {
                    await CrossLatestVersion.Current.OpenAppInStore();
                }
            }
        }

        private void SincronizadorService_ActualizacionRealizada()
        {
            var notas = App.Lista.GetAll().ToList();

            foreach (var nE in App.Sincronizador.Buffer)
            {
                if(nE.Estado == Estado.Agregado) { notas.Insert(0,nE.Nota); }
                else if (nE.Estado == Estado.Modificado) {
                    var n = notas.FirstOrDefault(x => x.Id == nE.Nota.Id);
                    if(n != null) { 
                        n.Titulo = nE.Nota.Titulo; 
                        n.Contenido = nE.Nota.Contenido; 
                    }
                }
                else
                {
                    var n = Notas.FirstOrDefault(x => x.Id == nE.Nota.Id);
                    if(n != null) { notas.Remove(n); }
                }
            }

            Notas.Clear();
            foreach(var item in notas.OrderByDescending(x => x.Id))
            {
                Notas.Add(item);
            }
        }

        private async void VerAgregarAsync()
        {
            if(vistaAgregar == null) { vistaAgregar = new AgregarNotaView() { BindingContext = this }; }

            Nota = new Notas();
            Errors = null;

            await Application.Current.MainPage.Navigation.PushAsync(vistaAgregar);

            
            CrossMTAdmob.Current.ShowInterstitial();
        }

        private async void VerEditarAsync(object n)
        {
            if(vistaEditar == null) { vistaEditar = new EditarNotaView() { BindingContext = this }; }

            Nota = n as Notas;
            Errors = null;

            await Application.Current.MainPage.Navigation.PushAsync(vistaEditar);
        }

        private async void verInfo()
        {
            vistaInfo = new InfoView();
            await Application.Current.MainPage.Navigation.PushAsync(vistaInfo);

        }

        private async void Agregar()
        {
            var result = await App.Sincronizador.Agregar(Nota);

            if(result == null) { await Application.Current.MainPage.Navigation.PopAsync(); }
            else { Errors = result.Select(x => new ErrorModel { Error = x }).ToList(); }
        }

        private async void Eliminar(object n)
        {
            var nota = n as Notas;
            var x = await Application.Current.MainPage.DisplayAlert("Confirmar:", $"Seguro de eliminar la nota?", "Si", "No");
            if(x)
            {
                var res = await App.Sincronizador.Eliminar(nota);
                if(res != null) { await Application.Current.MainPage.DisplayAlert("Error", String.Join("\n", res), "Aceptar"); }
            }
        }

        private async void Editar()
        {
            var res = await App.Sincronizador.Editar(Nota);
            if(res == null) { await Application.Current.MainPage.Navigation.PopAsync(); }
            else { Errors = res.Select(x => new ErrorModel { Error =x }).ToList(); }
        }

        private void Cancelar()
        {
            Nota = null;
            Application.Current.MainPage.Navigation.PopAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
