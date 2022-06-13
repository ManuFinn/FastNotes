using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastNotesAPI.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FastNotesAPI
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<itesrcne_181g0250Context>(optionsBuilder =>
            optionsBuilder.UseMySql("server=204.93.216.11;database=itesrcne_181g0250;user=itesrcne_jean;password=181G0250", 
            Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.3.29-mariadb")));

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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
