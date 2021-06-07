using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CachingDemo.Infrastructure;
using CachingDemo.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CachingDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddHttpClient();
            services.AddStackExchangeRedisCache(options => {
                options.Configuration = Configuration.GetSection("Redis")["ConnectionString"];
            });

            // DI
            services.AddScoped<UserService>();
            services.AddScoped<IUserService, CachedUserService>();
            services.AddScoped<ICacheUserService, CacheUserService>();
            services.AddScoped<ICacheProvider, CacheProvider>();
            services.AddScoped<IHttpClient, HttpClient>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
