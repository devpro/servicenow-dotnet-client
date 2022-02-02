using Microsoft.Extensions.DependencyInjection.Extensions;

namespace RabbidsIncubator.Samples.ServiceNowWebApiSample.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register additional infrastructure services.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddWebApiSampleInfrastructureRepositories(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAddTransient<Domain.IConfigurationItemRelationshipRepository, ConfigurationItemRelationshipRepository>();

            services.TryAddTransient<Domain.ISwitchRepository, SwitchRepository>();

            return services;
        }
    }
}
