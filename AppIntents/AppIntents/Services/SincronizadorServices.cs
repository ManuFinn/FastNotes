using AppIntents.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppIntents.Services
{
    public class SincronizadorServices
    {
        public static event Action ActuializacionRealizada;

        public List<VideoGamesEntity> Buffer { get; set; }

        string API = "https://181g0250.81g.itesrc.net/api/VideoGames";

        static HttpClient client = new HttpClient()
        {
            BaseAddress = new Uri("https://181g0250.81g.itesrc.net")
        };

        string key = "FechaUltimaActualizacion";

        public CatalogoVideoGames Catalogo { get; set; }

        public SincronizadorServices(CatalogoVideoGames c)
        {
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;

            Catalogo = c;

            DeserializarBuffer();

            if (!Preferences.ContainsKey(key))
            {
                _ = DescargarPrimeraVezAsync();
                Sincronizar();
            }
            else { Sincronizar(); }
        }

        private async void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            await Descargar();
        }

        async Task DescargarPrimeraVezAsync() ///Este si funciona, ya no moverle Jean del futuro
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                var fecha = DateTime.Now;
                var result = await client.GetAsync("api/VideoGames");


                if (result.IsSuccessStatusCode)
                {
                    Preferences.Set("FechaUltimaActualizacion", fecha);
                    string json = await result.Content.ReadAsStringAsync();
                    List<VideogameT> videogogos = JsonConvert.DeserializeObject<List<VideogameT>>(json);

                    videogogos.ForEach(x => Catalogo.InsertOrReplace(x));

                    if (videogogos.Count > 0) { LanzarEvento(); }
                }
            }
        }

        Thread hs;

        void Sincronizar()
        {
            hs = new Thread(new ThreadStart(hiloSincronizador));
            hs.Start();
        }

        async void hiloSincronizador()
        {
            while (true)
            {
                await Descargar();
                Thread.Sleep(TimeSpan.FromMinutes(1));
            }
        }

        async Task CargarBuffer()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                if (Buffer.Count > 0)
                {
                    Console.WriteLine("CargarBuffer Iniciado");

                    foreach (var vge in Buffer.ToArray())
                    {
                        switch (vge.Estado)
                        {
                            case Estado.Agregado:
                                var result = await EnviarAPI(vge.VideoGameT, HttpMethod.Post);
                                Buffer.Remove(vge);
                                break;
                            case Estado.Modificado:
                                await EnviarAPI(vge.VideoGameT, HttpMethod.Put);
                                Buffer.Remove(vge);
                                break;
                            case Estado.Eliminado:
                                await EnviarAPI(vge.VideoGameT, HttpMethod.Delete);
                                Buffer.Remove(vge);
                                break;
                        }
                    }

                    SerializarBuffer();

                    Console.WriteLine("CargarBuffer Iniciado");
                }
            }
        }

        private async Task Descargar()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet && 
                Preferences.ContainsKey("FechaUltimaActualizacion"))
            {
                await CargarBuffer();
                Console.WriteLine("Descargar Iniciado");

                var json = JsonConvert.SerializeObject(
                    Preferences.Get("FechaUltimaActualizacion", 
                    DateTime.MinValue));

                var fecha = DateTime.Now;

                var resp = await client.PostAsync("api/VideoGames/sincronizar",
                    new StringContent(json, Encoding.UTF8, "application/json"));

                if (resp.IsSuccessStatusCode)
                {
                    var datos = await resp.Content.ReadAsStringAsync();

                    List<VideogameT> lista = JsonConvert.DeserializeObject<List<VideogameT>>(datos);

                    foreach (var item in lista)
                    {
                        Catalogo.InsertOrReplace(item);
                    }

                    Preferences.Set("FechaUltimaActualizacion", fecha);

                    if (lista.Count > 0) { LanzarEvento(); }
                }

                Console.WriteLine("Descargar Terminado");
                DependencyService.Get<SnackInterface>().SnackbarShow("Se ha actualizado correctamente");

            }
        }

        public async Task<List<string>> Agregar(VideogameT vgt)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                var postito = await EnviarAPI(vgt, HttpMethod.Post);
                await Descargar();
                return postito;
            }
            else
            {
                VideoGamesEntity vge = new VideoGamesEntity();
                vge.VideoGameT = vgt;
                vge.Estado = Estado.Agregado;
                Buffer.Add(vge);
                SerializarBuffer();
                LanzarEvento();
                return null;
            }
        }

        public async Task<List<string>> Editar(VideogameT vgt)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                var postito = await EnviarAPI(vgt, HttpMethod.Put);
                await Descargar();
                return postito;
            }
            else
            {
                VideoGamesEntity vge = new VideoGamesEntity();
                vge.VideoGameT = vgt;
                vge.Estado = Estado.Modificado;
                Buffer.Add(vge);
                SerializarBuffer();
                LanzarEvento();
                return null;
            }
        }

        public async Task<List<string>> Eliminar(VideogameT vgt)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                var postito = await EnviarAPI(vgt, HttpMethod.Delete);
                await Descargar();
                return postito;
            }
            else
            {
                VideoGamesEntity vge = new VideoGamesEntity();
                vge.VideoGameT = vgt;
                vge.Estado = Estado.Eliminado;
                Buffer.Add(vge);
                SerializarBuffer();
                LanzarEvento();
                return null;
            }
        }

        private async Task<List<string>> EnviarAPI(VideogameT vgt, HttpMethod method)
        {
            List<string> errores = new List<string>();

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    string json = JsonConvert.SerializeObject(vgt);

                    HttpRequestMessage request = new HttpRequestMessage();
                    request.Method = method;
                    request.RequestUri = new Uri(client.BaseAddress + "api/VideoGames");
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                    var result = await client.SendAsync(request);

                    if (result.IsSuccessStatusCode)
                    {
                        //await Descargar();
                        return null;
                    }
                    if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        json = await result.Content.ReadAsStringAsync();
                        var lista = JsonConvert.DeserializeObject<List<string>>(json);
                        return lista;
                    }
                    if (result.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        errores.Add("Error, no existe o no se encontro el videojuego...");
                        return errores;
                        DependencyService.Get<SnackInterface>().SnackbarShow("El videojuego no existe o no se encontro...");
                    }
                    if (result.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                    {
                        json = await result.Content.ReadAsStringAsync();
                        var mensaje = JsonConvert.DeserializeObject<string>(json);
                        errores.Add(mensaje);
                        return errores;
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    errores.Add(ex.Message);
                    return null;
                }
            }
            else
            {
                errores.Add("Error de conexion, intente conectarse a otra red WiFi.");
                DependencyService.Get<SnackInterface>().SnackbarShow("Error de conexion, intente conectandose a otra red...");
                return null;
            }
        }

        private void LanzarEvento()
        {
            Xamarin.Forms.Application.Current.Dispatcher.BeginInvokeOnMainThread(() =>
            { ActuializacionRealizada?.Invoke(); });
        }

        void SerializarBuffer()
        {
            var json = JsonConvert.SerializeObject(Buffer);
            var ruta = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/buffer.json";
            File.WriteAllText(ruta, json);
        }

        void DeserializarBuffer()
        {
            var ruta = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/buffer.json";
            try
            {
                if (File.Exists(ruta))
                {
                    var json = File.ReadAllText(ruta);
                    Buffer = JsonConvert.DeserializeObject<List<VideoGamesEntity>>(json);
                }
                else { Buffer = new List<VideoGamesEntity>(); }
            }
            catch { Buffer = new List<VideoGamesEntity>(); }
        }



    }
}
