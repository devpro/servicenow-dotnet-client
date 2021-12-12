using Microsoft.Extensions.DependencyInjection;
using RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestApi;
using RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestApi.DependencyInjection;

namespace RabbidsIncubator.ServiceNowClient.Application.DependencyInjection
{
    public static class InfrastructureExtentions
    {
        /// <summary>
        /// Add Infrastructure repositories in ServiceCollection.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddRepositories(this IServiceCollection services, ServiceNowRestApiConfiguration serviceNowRestApiConfiguration)
        {
            services.AddServiceNowRestApiRepositories(serviceNowRestApiConfiguration);

            return services;
        }
    }
}
