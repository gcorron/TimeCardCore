using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using TimeCard.Repo.Repos;

namespace TimeCardCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string envContainer = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER");
            string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

            var builder = WebApplication.CreateBuilder(args);
            var cb = new ConfigurationBuilder();
            cb.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            if (env == "Production")
            {
                cb.AddJsonFile("appsettings.Production.json");
            }
            cb.Build();

            
            builder.Services.AddRazorPages();
            builder.Host.UseSerilog((ctx, loggerConfiguration) =>
            {
                loggerConfiguration.ReadFrom.Configuration(ctx.Configuration);
            });
            builder.Services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();
            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
            builder.Services.AddAuthorization();
            var mvc = builder.Services.AddControllersWithViews()
                .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null)
                .AddRazorRuntimeCompilation();

             var ConnString = builder.Configuration.GetConnectionString("TimeCard");
            builder.Services.AddScoped<TimeCard.Repo.Repos.IRepo>(c => new LookupRepo(ConnString));

            var app = builder.Build();
            if (env == "Development")
            {
                //app.UseExceptionHandler("/Error/ErrorHandler");
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // app.UseExceptionHandler("/Error/ErrorHandler");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseWebSockets();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            string defaultController = "Account";
            app.MapControllerRoute(
                name: "default",
                pattern: $"{{controller={defaultController}}}/{{action=Index}}/{{id?}}");
            app.MapRazorPages();
            app.Run();
        }
    }
}
