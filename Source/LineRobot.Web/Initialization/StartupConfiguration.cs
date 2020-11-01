using Line;
using LineRobot.Domain.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LineRobot.Web.Initialization
{
    public static class StartupConfiguration
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration Configuration)
        {
            var lineConfiguration = Configuration.GetSection("LineConfiguration").Get<LineConfiguration>();
            services.AddSingleton<ILineConfiguration>(provider => lineConfiguration);

            var mongoDBOptions = Configuration.GetSection("MongoDBOptions").Get<MongoDBOptions>();
            services.AddSingleton<MongoDBOptions>(provider => mongoDBOptions);

            var tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();
            services.AddSingleton<TokenOptions>(provider => tokenOptions);
        }
    }
}
