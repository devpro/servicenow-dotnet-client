using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace RabbidsIncubator.ServiceNowClient.Infrastructure.InMemory.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register in-memory services.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddInMemoryRepositories(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAddTransient<Domain.Repositories.ICacheRepository, Repositories.InMemoryCacheRepository>();

            return services;
        }
    }
}
