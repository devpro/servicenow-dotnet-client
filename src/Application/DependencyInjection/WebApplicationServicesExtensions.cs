using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbidsIncubator.ServiceNowClient.Infrastructure.InMemory.DependencyInjection;
using RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestClient.DependencyInjection;

namespace RabbidsIncubator.ServiceNowClient.Application.DependencyInjection
{
    public static class WebApplicationServicesExtensions
    {
        public static IServiceCollection AddDefaultServices(
            this IServiceCollection services,
            ConfigurationManager configuration,
            params AutoMapper.Profile[] additionalProfiles)
        {
            services.AddAutoMapperConfiguration(additionalProfiles);
            services.AddInMemoryRepositories(configuration.GetSection("Cache:InMemory").Get<Infrastructure.InMemory.InMemoryConfiguration>());
            services.AddServiceNowRestClientRepositories(configuration.GetSection("ServiceNow:RestApi").Get<Infrastructure.ServiceNowRestClient.ServiceNowRestClientConfiguration>());
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddHealthChecks();
            return services;
        }
    }
}
