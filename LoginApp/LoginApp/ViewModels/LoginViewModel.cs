using System;
using System.Collections.Generic;
using System.Text;

namespace LoginApp.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public LoginModel LoginModel { get; set; }

        public bool Indicador { get; set; }

        public string Error { get; set; }

        public ICommand LogInCommnad { get; set; }



    }
}
