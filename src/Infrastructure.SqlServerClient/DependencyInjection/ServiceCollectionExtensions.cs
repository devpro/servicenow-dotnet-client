using System;
using Microsoft.Extensions.DependencyInjection;

namespace RabbidsIncubator.ServiceNowClient.Infrastructure.SqlServerClient.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSqlServerClientRepositories<T>(this IServiceCollection services, T configuration)
            where T : SqlServerClientConfiguration
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration), "SQL Server configuration is missing.");
            }

            if (!configuration.IsValid())
            {
                throw new ArgumentNullException(nameof(configuration), "SQL Server configuration invalid. Make sure all fields are correctly set");
            }

            services.AddSingleton(configuration);

            return services;
        }
    }
}
