using LoginApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using Xamarin.Essentials;

namespace LoginApp.ViewModels
{
    public class ProductosViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ProductosModel> Lista { get; set; } = new ObservableCollection<ProductosModel>();

        public string Error { get; set; }

        public ProductosViewModel()
        {
            CargarDatos();
        }

        async void CargarDatos()
        {
            if(Connectivity.NetworkAccess == NetworkAccess.Internet)
            { 

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://181g0250.81g.itesrc.net/");
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {await App.User.GetToken()}");

            var result = await client.GetAsync("api/productos");

            string json = await result.Content.ReadAsStringAsync();

            List<ProductosModel> productos = JsonConvert.DeserializeObject<List<ProductosModel>>(json);

            foreach(var item in productos)
            {
                Lista.Add(item);
            }
            }
            else { Error = "No esta conectado a internet..."; }

        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
