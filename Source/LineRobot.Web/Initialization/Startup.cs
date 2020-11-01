using Line;
using LineRobot.Domain.Interface;
using LineRobot.Repository;
using LineRobot.Service;
using LineRobot.Web.Handler;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LineRobot.Web.Initialization
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) => Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            StartupConfiguration.ConfigureServices(services, Configuration);

            services.AddScoped<DocumentRepository, DocumentRepository>();
            services.AddScoped<HandleRepository, HandleRepository>();

            services.AddScoped<CryptographyService, CryptographyService>();

            services.AddScoped<ILineBot, LineBot>();
            services.AddScoped<ILineEventHandler, LineEventMessageHandler>();
            services.AddScoped<ILineEventHandler, LineEventBeaconHandler>();

            services.AddControllers();
            services.AddHttpClient();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCookiePolicy();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
