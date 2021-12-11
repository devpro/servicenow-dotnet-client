﻿using System;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestApi.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServiceNowRestApi<T>(this IServiceCollection services, T configuration)
            where T : ServiceNowRestApiConfiguration
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton(configuration);
            services.TryAddTransient<Domain.Repositories.IConfigurationItemRelationshipRepository, Repositories.ConfigurationItemRelationshipRepository>();
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
