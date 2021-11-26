using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System.IO;

namespace Chubb.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                 .ConfigureLogging(x =>
                 {
                     x.ClearProviders();
                     x.AddConsole();
                 })
                .UseSerilog((options, logging) =>
                {
                    logging.WriteTo.Console();
                    logging.WriteTo.File(Path.Combine(options.HostingEnvironment.ContentRootPath, "Log/Log.txt"), LogEventLevel.Error, fileSizeLimitBytes: 1024, rollingInterval: RollingInterval.Day);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
