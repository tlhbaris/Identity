using Identity.CustomValidation;
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


            CookieBuilder cookieBuilder = new CookieBuilder();

            cookieBuilder.Name = "MyBlog";
            cookieBuilder.HttpOnly = false;
            cookieBuilder.Expiration = System.TimeSpan.FromDays(60); // cookie geçerlilik süresi
            cookieBuilder.SameSite = SameSiteMode.Lax;  //Baþka site üzerinden cookie gönderilmesini engellenip(Strict) engellenmemesi(Lax) için
            cookieBuilder.SecurePolicy = CookieSecurePolicy.SameAsRequest;


            services.ConfigureApplicationCookie(opts =>
            {
                opts.LoginPath = new PathString("/Home/Login");       //sadece üyelerin eriþeblieceði sayfalara girmek istediðinde yönlendirlicek sayfa
                opts.Cookie = cookieBuilder;
                opts.SlidingExpiration = true; // cookie geçerlilik süresinin yarýsýna geldiðinde uzatmasý için
            });





            services.AddIdentity<AppUser, AppRole>(opts =>
            {


                opts.User.RequireUniqueEmail = true;                //kullanýcý doðrulama ayarlarý
                opts.User.AllowedUserNameCharacters = "abcçdefghýijklmnoöpqrsþtuüvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._";


                opts.Password.RequiredLength = 4;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;              // þifre doðrulama ayarlarý
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;

            }).AddPasswordValidator<CustomPasswordValidator>() //custom password validator ekleniyor.
              .AddUserValidator<CustomUserValidator>()         // custom user validator ekleniyor
              .AddErrorDescriber<CustomIdentityErrorDescriber>() 
              .AddEntityFrameworkStores<AppIdentityDbContext>(); //identity kullanýcaðýmýzý belli ediyoruz.






            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage(); // bi hata aldýðýmýzda o hatayla ilgili açýklayýcý bilgiler sunuyor
            app.UseStatusCodePages(); // herhangi bir içerik dönmeyen sayfalarda bilgilendirci yazýlar dönüyor. 
            app.UseStaticFiles();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseAuthentication();// identity i kullanabilmek için




        }
    }
}
