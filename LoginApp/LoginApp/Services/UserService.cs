using LoginApp.Models;
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
    internal class UserService
    {
        public async Task<string> GetToken()
        {
            var x = await SecureStorage.GetAsync("myToken");
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

        //public async Task<bool> Renovar()
        //{
        //    var user = await SecureStorage.GetAsync("User");
        //    var pass = await SecureStorage.GetAsync("Passowrd");

        //    if(user != null && pass != null)
        //    {
        //        return await IniciarSesion(new LoginModel
        //        {
        //            User = user,
        //            Password = pass
        //        });
        //    }
        //    return false;
        //}

        //public async Task<bool> IniciarSesion(LoginModel lm)
        //{
        //    HttpClient client = new HttpClient();
        //    client.BaseAddress = new Uri("");
        //}
    }
}
