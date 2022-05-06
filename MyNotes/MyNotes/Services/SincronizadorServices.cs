using MyNotes.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MyNotes.Services
{
    public class SincronizadorServices
    {
        public static event Action ActuializacionRealizada;

        public List<NotasEntity> Buffer { get; set; }

        static HttpClient client = new HttpClient()
        {
            BaseAddress = new Uri("http://181g0250.81g.itesrc.net")
        };

        string key = "FechaUltimaActualizacion";

        public ListaNotas Lista { get; set; }

        public SincronizadorServices(ListaNotas n)
        {
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;

            Lista = n;

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

        async Task DescargarPrimeraVezAsync()
        {
            if(Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                var fecha = DateTime.Now;
                var result = await client.GetAsync("api/notas");

                if(result.IsSuccessStatusCode)
                {
                    Preferences.Set(key, fecha);
                    string json = await result.Content.ReadAsStringAsync();
                    List<Notas> notas = JsonConvert.DeserializeObject<List<Notas>>(json);

                    notas.ForEach(x => Lista.InsertOrReplace(x));

                    if(notas.Count > 0) { LanzarEvento(); }
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
            while(true)
            {
                await Descargar();
                Thread.Sleep(TimeSpan.FromMinutes(1));
            }
        }

        async Task CargarBuffer()
        {
            if(Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                if(Buffer.Count > 0)
                {
                    Console.WriteLine("CargarBuffer Iniciado");

                    foreach (var ce in Buffer.ToArray())
                    {
                        switch (ce.Estado)
                        {
                            case Estado.Agregado:
                                var result = await EnviarAPI(ce.Nota, HttpMethod.Post);
                                if (result != null) Buffer.Remove(ce);
                                break;
                            case Estado.Modificado:
                                await EnviarAPI(ce.Nota, HttpMethod.Put);
                                Buffer.Remove(ce);
                                break;
                            case Estado.Eliminado:
                                await EnviarAPI(ce.Nota, HttpMethod.Delete);
                                Buffer.Remove(ce);
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
            if (Connectivity.NetworkAccess == NetworkAccess.Internet && Preferences.ContainsKey(key))
            {
                await CargarBuffer();
                Console.WriteLine("Descargar Iniciado");

                var json = JsonConvert.SerializeObject(Preferences.Get(key, DateTime.MinValue));

                var fecha = DateTime.Now;

                var resp = await client.PostAsync("api/notas/sincronizar", new StringContent(json, Encoding.UTF8, "application/json"));

                if(resp.IsSuccessStatusCode)
                {
                    var datos = await resp.Content.ReadAsStringAsync();

                    List<Notas> lista = JsonConvert.DeserializeObject<List<Notas>>(datos);

                    foreach (var item in lista)
                    {
                        Lista.InsertOrReplace(item);
                    }

                    Preferences.Set(key, fecha);

                    if(lista.Count > 0) { LanzarEvento(); }
                }

                Console.WriteLine("Descargar Terminado");

            }
        }

        public async Task<List<string>> Agregar(Notas n)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet) { return await EnviarAPI(n, HttpMethod.Post); }
            else
            {
                NotasEntity nE = new NotasEntity();
                nE.Nota = n;
                nE.Estado = Estado.Agregado;
                Buffer.Add(nE);
                SerializarBuffer();
                LanzarEvento();
                return null;
            }
        }

        public async Task<List<string>> Editar(Notas n)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet) { return await EnviarAPI(n, HttpMethod.Put); }
            else
            {
                NotasEntity nE = new NotasEntity();
                nE.Nota = n;
                nE.Estado = Estado.Modificado;
                Buffer.Add(nE);
                SerializarBuffer();
                LanzarEvento();
                return null;
            }
        }

        public async Task<List<string>> Eliminar(Notas n)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet) { return await EnviarAPI(n, HttpMethod.Delete); }
            else
            {
                NotasEntity nE = new NotasEntity();
                nE.Nota = n;
                nE.Estado = Estado.Eliminado;
                Buffer.Add(nE);
                SerializarBuffer();
                LanzarEvento();
                return null;
            }
        }

        private async Task<List<string>> EnviarAPI(Notas n, HttpMethod method)
        {
            List<string> errores = new List<string>();

            if(Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    string json = JsonConvert.SerializeObject(n);

                    HttpRequestMessage request = new HttpRequestMessage();
                    request.Method = method;
                    request.RequestUri = new Uri(client.BaseAddress + "api/notas");
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                    var result = await client.SendAsync(request);

                    if(result.IsSuccessStatusCode)
                    {
                        await Descargar();
                        return null;
                    }
                    if(result.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        json = await result.Content.ReadAsStringAsync();
                        var lista = JsonConvert.DeserializeObject<List<string>>(json);
                        return lista;
                    }
                    if(result.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        errores.Add("No se ha encontrado el contacto indicado.");
                        return errores;
                    }
                    if(result.StatusCode == System.Net.HttpStatusCode.InternalServerError)
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
                errores.Add("Conectese al wifi mijo");
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
                    Buffer = JsonConvert.DeserializeObject<List<NotasEntity>>(json);
                }
                else { Buffer = new List<NotasEntity>(); }
            }
            catch { Buffer = new List<NotasEntity>(); }
        }


    }
}
