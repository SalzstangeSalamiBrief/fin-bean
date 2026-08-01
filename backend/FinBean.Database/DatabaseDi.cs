using FinBean.Database.Options;
using Microsoft.Extensions.DependencyInjection;

namespace FinBean.Database;

public static class DatabaseDi
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddDatabaseOptions()
        {
            services
                .AddOptions<DatabaseOptions>()
                .BindConfiguration(nameof(DatabaseOptions))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            return services;
        }

        public IServiceCollection AddDatabaseContext()
        {
            services.AddDbContext<FinBeanDatabaseContext>();
            return  services;
        }
    }
}