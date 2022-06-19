using LoginApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LoginApp.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public LoginModel LoginModel { get; set; }

        public bool Indicador { get; set; }

        public string Error { get; set; }

        public ICommand LogInCommnad { get; set; }

        public LoginViewModel()
        {
            LoginModel = new LoginModel();
            LogInCommnad = new Command(LogIn);
        }

        private async void LogIn()
        {
            

            if(Connectivity.NetworkAccess == NetworkAccess.Internet)
            { 
                Indicador = true;
            if(string.IsNullOrWhiteSpace(LoginModel.User) || string.IsNullOrWhiteSpace(LoginModel.Password))
            {
                Error = "Debe de escribir los datos para poder iniciar sesion.";
            }
            else
            {
                var result = await App.User.IniciarSesion(LoginModel);
                if(!result)
                {
                    Error = "El usuario o la contraseña es incorrecta";
                }
                else { App.User.BackToLogin(); }
            }
            }
            else { Error = "No esta conectado a internet..."; }
            Indicador = false;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
