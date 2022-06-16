using LoginApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace LoginApp.Services
{
    public class UserService
    {
        public async Task<string> GetToken()
        {
            var x = await SecureStorage.GetAsync("MiToken");
            return x;
        }

        public ClaimsIdentity Identity { get; set; }
        public DateTime ExpDate { get; set; }

        public bool IsLoggedIn 
        { 
            get
            {
                var res = GetToken().Result;
                if (res == null) return false;

                var thandler = new JwtSecurityTokenHandler();
                var des = thandler.ReadJwtToken(res);
                Identity = new ClaimsIdentity(des.Claims);
                ExpDate = des.ValidTo;
                var salida = res != null && DateTime.UtcNow < ExpDate;
                return salida;
            }
        }

        public async Task<bool> Renovar()
        {
            var user = await SecureStorage.GetAsync("User");
            var pass = await SecureStorage.GetAsync("Passowrd");

            if (user != null && pass != null)
            {
                return await IniciarSesion(new LoginModel
                {
                    User = user,
                    Password = pass
                });
            }
            return false;
        }

        public async Task<bool> IniciarSesion(LoginModel lm)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://181g0250.81g.itesrc.net/");

            var json = JsonConvert.SerializeObject(lm);

            var result = await client.PostAsync("api/Login",
                new StringContent(json, Encoding.UTF8, "application/json"));

            SecureStorage.RemoveAll();

            if(result.IsSuccessStatusCode)
            {
                var token = await result.Content.ReadAsStringAsync();

                await SecureStorage.SetAsync("MiToken", token);
                await SecureStorage.SetAsync("User", lm.User);
                await SecureStorage.SetAsync("Password", lm.Password);

                var thand = new JwtSecurityTokenHandler();
                var des = thand.ReadJwtToken(token);

                Identity = new ClaimsIdentity(des.Claims);
                ExpDate = des.ValidTo;

                return true;
            }
            else if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized) { return false; }

            return true;
        }

        public void BackToLogin()
        {
            App.Current.MainPage = new Views.ListaProductosView();
        }

        public void CerrarSesion()
        {
            SecureStorage.RemoveAll();
            Identity = null;
            ExpDate = DateTime.MinValue;
            App.Current.MainPage = new Views.LoginView();
        }
    }
}
