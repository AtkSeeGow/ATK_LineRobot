using LineRobot.Web.Initialization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NLog.Web;
using System.Net;

namespace LineRobot.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseNLog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(serverOptions =>
                    {
                        serverOptions.Listen(IPAddress.Any, 5000);
                        serverOptions.Listen(IPAddress.Any, 5001, listenOptions =>
                        {
                            listenOptions.UseHttps(@"/certificate/certificate.pfx", "000000");
                        });
                    })
                    .UseStartup<Startup>();
                });
    }
}
