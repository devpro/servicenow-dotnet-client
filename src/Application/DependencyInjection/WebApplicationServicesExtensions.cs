using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
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
            if (bool.TryParse(configuration[ConfigurationConstants.IsSecuredByAzureAdConfigKey], out var isSecuredByAzureAd) && isSecuredByAzureAd)
            {
                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddMicrosoftIdentityWebApi(configuration.GetSection(ConfigurationConstants.AzureAdConfigKey))
                        .EnableTokenAcquisitionToCallDownstreamApi()
                        .AddInMemoryTokenCaches();
            }

            //TODO: add OpenTelemetry

            services.AddAutoMapperConfiguration(additionalProfiles);
            services.AddInMemoryRepositories(configuration.GetSectionValue<Infrastructure.InMemory.InMemoryConfiguration>(ConfigurationConstants.InMemoryCacheConfigKey));
            services.AddServiceNowRestClientRepositories(configuration.GetSectionValue<Infrastructure.ServiceNowRestClient.ServiceNowRestClientConfiguration>(ConfigurationConstants.RestApiServiceNowConfigKey));
            if (configuration.TryGetSection<Infrastructure.SqlServerClient.SqlServerClientConfiguration>(ConfigurationConstants.SqlServerServiceNowConfigKey) != null)
            {
                services.AddSqlServerClientRepositories(configuration.GetSectionValue<Infrastructure.SqlServerClient.SqlServerClientConfiguration>(ConfigurationConstants.SqlServerServiceNowConfigKey));
            }
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddHealthChecks();
            return services;
        }

        public static T TryGetSection<T>(this IConfiguration configuration, string sectionKey)
        {
            var section = configuration.GetSection(sectionKey);
            return section.Get<T>();
        }

        public static T GetSectionValue<T>(this IConfiguration configuration, string sectionKey)
        {
            var value = configuration.TryGetSection<T>(sectionKey);
            if (value == null)
            {
                throw new ArgumentException($"Missing section \"{sectionKey}\" in configuration");
            }

            return value;
        }
    }
}
