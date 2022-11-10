using Line;
using LineRobot.Domain.Interface;
using LineRobot.Domain.Options;
using LineRobot.Repository;
using LineRobot.Service;
using LineRobot.Web.Handler;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LineRobot.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) => Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            var lineConfiguration = Configuration.GetSection("LineConfiguration").Get<LineConfiguration>();
            services.AddSingleton<ILineConfiguration>(provider => lineConfiguration);

            var mongoDBOptions = Configuration.GetSection("MongoDBOptions").Get<MongoDBOptions>();
            services.AddSingleton(provider => mongoDBOptions);

            var tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();
            services.AddSingleton(provider => tokenOptions);

            services.AddScoped<DocumentRepository, DocumentRepository>();
            services.AddScoped<HandleRepository, HandleRepository>();

            services.AddScoped<CryptographyService, CryptographyService>();

            services.AddScoped<ILineBot, LineBot>();
            services.AddScoped<IHandler, MessageHandler>();
            services.AddScoped<IHandler, BeaconHandler>();

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
