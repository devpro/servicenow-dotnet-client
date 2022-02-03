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
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddInMemoryRepositories<T>(this IServiceCollection services, T configuration)
            where T : InMemoryConfiguration
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton(configuration);

            services.TryAddSingleton<Microsoft.Extensions.Caching.Memory.IMemoryCache, Microsoft.Extensions.Caching.Memory.MemoryCache>();

            services.TryAddTransient<Domain.Repositories.ICacheRepository, Repositories.InMemoryCacheRepository>();

            return services;
        }
    }
}
