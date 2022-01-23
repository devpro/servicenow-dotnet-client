using System;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestClient.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register services to be able to use the ServiceNow REST API infrastructure library:
        /// - Configuration
        /// - Repositories
        /// - HTTP client
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddServiceNowRestClientRepositories<T>(this IServiceCollection services, T configuration)
            where T : ServiceNowRestClientConfiguration
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton(configuration);
            services.TryAddTransient<Domain.Repositories.IConfigurationItemRelationshipRepository, Repositories.ConfigurationItemRelationshipRepository>();
            services.TryAddTransient<Domain.Repositories.ISwitchRepository, Repositories.SwitchRepository>();
            //services.AddServiceNowRestClientGeneratedRepositories();
            services
                .AddHttpClient(configuration.HttpClientName, client =>
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                        Convert.ToBase64String(Encoding.ASCII.GetBytes($"{configuration.Username}:{configuration.Password}")));
                });

            return services;
        }
    }
}
