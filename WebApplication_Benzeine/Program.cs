using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebApplication_Benzeine.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Encodings.Web;

namespace WebApplication_Benzeine
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var webHost = CreateWebHostBuilder(args).Build();

            // DB seeding
            using (var provider = webHost.Services.CreateScope())
                await DbInitializer.SeedData(provider.ServiceProvider);

            webHost.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
          WebHost.CreateDefaultBuilder(args)
              .UseStartup<Startup>();
    }
}
