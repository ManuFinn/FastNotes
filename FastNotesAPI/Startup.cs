using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastNotesAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace FastNotesAPI
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        //public Startup(IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //}

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services, IHostBuilder builder)
        {
            string llave = Configuration["JwtAuth:Key"];
            string issuer = Configuration["JwtAuth:Issuer"];
            string aud = Configuration["JwtAuth:Audience"];

            services.AddDbContext<itesrcne_181g0250Context>(optionsBuilder =>
            optionsBuilder.UseMySql("server=204.93.216.11;database=itesrcne_181g0250;user=itesrcne_jean;password=181G0250", 
            Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.3.29-mariadb")));

            services.AddAuthentication(
                JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidAudience = aud,
                        ValidIssuer = issuer,
                        IssuerSigningKey =
                        new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(llave)),
                        ValidateIssuerSigningKey = true
                    };
                });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
               app.UseDeveloperExceptionPage();
            //}

            app.UseRequestLocalization("es-MX");

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
