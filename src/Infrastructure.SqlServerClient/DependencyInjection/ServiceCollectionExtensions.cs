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

            services.AddSingleton(configuration);

            return services;
        }
    }
}
