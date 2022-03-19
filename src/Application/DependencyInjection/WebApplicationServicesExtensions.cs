using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
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
            ILoggingBuilder logging,
            params AutoMapper.Profile[] additionalProfiles)
        {
            services.AddAuthentication(configuration, out var isSecuredByAzureAd);
            services.AddOpenTelemetry(configuration, logging);
            services.AddAutoMapper(additionalProfiles);
            services.AddRepositories(configuration);
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGenWithOpenApiInfo(configuration, isSecuredByAzureAd);
            services.AddHealthChecks();
            return services;
        }

        private static IServiceCollection AddAuthentication(this IServiceCollection services, ConfigurationManager configuration, out bool isSecuredByAzureAd)
        {
            if (bool.TryParse(configuration[ConfigurationConstants.IsSecuredByAzureAdConfigKey], out isSecuredByAzureAd) && isSecuredByAzureAd)
            {
                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddMicrosoftIdentityWebApi(configuration.GetSection(ConfigurationConstants.AzureAdConfigKey))
                        .EnableTokenAcquisitionToCallDownstreamApi()
                        .AddInMemoryTokenCaches();
            }

            return services;
        }

        private static IServiceCollection AddSwaggerGenWithOpenApiInfo(this IServiceCollection services, ConfigurationManager configuration, bool isSecured)
        {
            services.AddSwaggerGen(c =>
            {
                var openApi = configuration.GetSectionValue<OpenApiInfo>(ConfigurationConstants.OpenApiConfigKey);
                c.SwaggerDoc(openApi.Version, openApi);
                if (isSecured)
                {
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme.\r\n\r\nEnter your token in the text input below.",
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

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddInMemoryRepositories(configuration.GetSectionValue<Infrastructure.InMemory.InMemoryConfiguration>(ConfigurationConstants.InMemoryCacheConfigKey));

            services.AddServiceNowRestClientRepositories(configuration.GetSectionValue<Infrastructure.ServiceNowRestClient.ServiceNowRestClientConfiguration>(ConfigurationConstants.ServiceNowRestApiConfigKey));
            if (configuration.TryGetSection<Infrastructure.SqlServerClient.SqlServerClientConfiguration>(ConfigurationConstants.ServiceNowSqlServerConfigKey) != null)
            {
                services.AddSqlServerClientRepositories(configuration.GetSectionValue<Infrastructure.SqlServerClient.SqlServerClientConfiguration>(ConfigurationConstants.ServiceNowSqlServerConfigKey));
            }

            return services;
        }

        private static IServiceCollection AddOpenTelemetry(this IServiceCollection services, ConfigurationManager configuration, ILoggingBuilder logging)
        {
            if (bool.TryParse(configuration[ConfigurationConstants.IsOpenTelemetryEnabledConfigKey], out var isOpenTelemetryEnabled) && isOpenTelemetryEnabled)
            {
                var openTelemetryCollectorEndpoint = configuration[ConfigurationConstants.OpenTelemetryOtlpExporterEndpointConfigKey];

                var openTelemetryMetricsSource = configuration[ConfigurationConstants.OpenTelemetryMetricsSourceConfigKey];
                if (!string.IsNullOrEmpty(openTelemetryMetricsSource))
                {
                    services.AddOpenTelemetryMetrics(builder =>
                    {
                        builder.AddAspNetCoreInstrumentation();
                        builder.AddHttpClientInstrumentation();
                        builder.AddMeter(openTelemetryMetricsSource);
                        builder.AddOtlpExporter(options => options.Endpoint = new Uri(openTelemetryCollectorEndpoint));
                    });
                }

                var openTelemetryTracingSource = configuration[ConfigurationConstants.OpenTelemetryTracingSourceConfigKey];
                if (!string.IsNullOrEmpty(openTelemetryTracingSource))
                {
                    services.AddOpenTelemetryTracing(builder =>
                    {
                        builder.AddAspNetCoreInstrumentation();
                        builder.AddHttpClientInstrumentation();
                        builder.AddSqlClientInstrumentation();
                        builder.AddSource(openTelemetryTracingSource);
                        builder.AddOtlpExporter(options => options.Endpoint = new Uri(openTelemetryCollectorEndpoint));
                    });
                }

                logging.AddOpenTelemetry(builder =>
                {
                    builder.IncludeFormattedMessage = true;
                    builder.IncludeScopes = true;
                    builder.ParseStateValues = true;
                    builder.AddOtlpExporter(options => options.Endpoint = new Uri(openTelemetryCollectorEndpoint));
                });
            }

            return services;
        }
    }
}
