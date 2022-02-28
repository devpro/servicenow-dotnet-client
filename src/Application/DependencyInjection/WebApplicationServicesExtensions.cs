using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbidsIncubator.ServiceNowClient.Infrastructure.InMemory.DependencyInjection;
using RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestClient.DependencyInjection;
using RabbidsIncubator.ServiceNowClient.Infrastructure.SqlServerClient.DependencyInjection;

namespace RabbidsIncubator.ServiceNowClient.Application.DependencyInjection
{
    public static class WebApplicationServicesExtensions
    {
        /// <summary>
        /// Add default services in the service collection.
        /// Expected configuration elements: "Cache:InMemory", "ServiceNow:RestApi", "ServiceNow:SqlServer".
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="additionalProfiles"></param>
        /// <returns></returns>
        public static IServiceCollection AddDefaultServices(
            this IServiceCollection services,
            ConfigurationManager configuration,
            params AutoMapper.Profile[] additionalProfiles)
        {
            services.AddAutoMapperConfiguration(additionalProfiles);
            services.AddInMemoryRepositories(configuration.GetSection("Cache:InMemory").Get<Infrastructure.InMemory.InMemoryConfiguration>());
            services.AddServiceNowRestClientRepositories(configuration.GetSection("ServiceNow:RestApi").Get<Infrastructure.ServiceNowRestClient.ServiceNowRestClientConfiguration>());
            services.AddSqlServerClientRepositories(configuration.GetSection("ServiceNow:SqlServer").Get<Infrastructure.SqlServerClient.SqlServerClientConfiguration>());
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddHealthChecks();
            return services;
        }
    }
}
