using LoginApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Text;

namespace LoginApp.ViewModels
{
    public class ProductosViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ProductosModel> Lista { get; set; } = new ObservableCollection<ProductosModel>();

        public ProductosViewModel()
        {
            CargarDatos();
        }

        async void CargarDatos()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://181g0250.81g.itesrc.net/");
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {await App.User.GetToken()}");

            var result = await client.GetAsync("api/productos");
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
