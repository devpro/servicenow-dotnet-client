using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using RabbidsIncubator.ServiceNowClient.Application.Configuration;
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

            services.AddSwaggerGen(c =>
            {
                var openApi = configuration.GetSectionValue<OpenApiInfo>(ConfigurationConstants.OpenApiConfigKey);
                c.SwaggerDoc(openApi.Version, openApi);
                if (isSecuredByAzureAd)
                {
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme.\r\n\r\nEnter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        Scheme = "Bearer"
                    });
                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                Name = "Bearer",
                                In = ParameterLocation.Header,
                            },
                            new List<string>()
                        }
                    });
                }
            });

            services.AddHealthChecks();

            return services;
        }
    }
}
