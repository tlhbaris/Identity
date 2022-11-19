using Identity.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity
{
    public class Startup
    {
        public IConfiguration configuration { get; }

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<AppIdentityDbContext>(opts =>
            {
                opts.UseSqlServer(configuration["ConnectionStrings:DefaultConnectionString"]);
            });

            services.AddIdentity<AppUser, AppRole>(opts =>
            {
                opts.Password.RequiredLength = 4;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase= false;              // �ifre do�rulama ayarlar�
                opts.Password.RequireUppercase= false;
                opts.Password.RequireDigit= false;

            }).AddEntityFrameworkStores<AppIdentityDbContext>(); //identity kullan�ca��m�z� belli ediyoruz.






            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage(); // bi hata ald���m�zda o hatayla ilgili a��klay�c� bilgiler sunuyor
            app.UseStatusCodePages(); // herhangi bir i�erik d�nmeyen sayfalarda bilgilendirci yaz�lar d�n�yor. 
            app.UseStaticFiles();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseAuthentication();// identity i kullanabilmek i�in




        }
    }
}
