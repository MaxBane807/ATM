using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ATM.Web.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ATM.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var ihost = CreateHostBuilder(args).Build();
            InitalizeDb(ihost);
            ihost.Run();
        }

        private static void InitalizeDb(IHost ihost)
        {
            using (var scope = ihost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<AtmContext>();
                DatabaseInitializer.Initialize(context);
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

    }
}
