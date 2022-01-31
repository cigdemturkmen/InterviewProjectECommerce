using ECommerce.Data.Abstract;
using ECommerce.Data.Concrete;
using ECommerce.Entities;
using ECommerce.Entities.Concrete;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.UI
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ECommerceDbContext>(option =>
            {
                option.UseSqlServer("Server=.;Database=ECommerce;User Id=sa;Password=Password1;");
            });

            services.AddScoped<IRepository<Category>, EFRepository<Category>>();
            services.AddScoped<IRepository<Product>, EFRepository<Product>>();
            services.AddScoped<IRepository<Role>, EFRepository<Role>>();
            services.AddScoped<IRepository<User>, EFRepository<User>>();
            services.AddScoped<IRepository<ProductImage>, EFRepository<ProductImage>>();

            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); // !!


            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(option =>
                {
                    option.LoginPath = "/auth/login";
                    option.ExpireTimeSpan = TimeSpan.FromHours(1);
                });


            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                       name: "default",
                       pattern: "{controller}/{action}/{id?}",
                       defaults: new { controller = "Product", action = "List" }
                      );
            });
        }
    }
}
