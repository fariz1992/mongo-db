using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.API.Data
{
    public static class AddMongoDbSettingsExtension
    {
        public static IServiceCollection AddMongoDbSettings(this IServiceCollection services, IConfiguration configuration)
        {
            return services.Configure<MongoDbSettings>(options =>
            {
                options.ConnectionString = configuration.GetSection(nameof(MongoDbSettings) + ":" + MongoDbSettings.ConnectionStringValue).Value;
                options.DatabaseName = configuration.GetSection(nameof(MongoDbSettings) + ":" + MongoDbSettings.DatabaseNameValue).Value;
            });
        }
    }
}
