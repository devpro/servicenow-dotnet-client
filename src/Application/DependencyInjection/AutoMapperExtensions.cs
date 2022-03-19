using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace RabbidsIncubator.ServiceNowClient.Application.DependencyInjection
{
    public static class AutoMapperExtensions
    {
        /// <summary>
        /// Add AutoMapper configuration in service collection.
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="additionalProfiles">Additional profiles</param>
        /// <returns></returns>
        public static IServiceCollection AddAutoMapper(this IServiceCollection services, params Profile[] additionalProfiles)
        {
            var mappingConfig = new MapperConfiguration(x =>
            {
                x.AddProfile(new Infrastructure.ServiceNowRestClient.MappingProfiles.ServiceNowRestClientMappingProfile());
                x.AddProfile(new Infrastructure.SqlServerClient.MappingProfiles.SqlServerClientMappingProfile());
                if (additionalProfiles != null && additionalProfiles.Length > 0)
                {
                    x.AddProfiles(additionalProfiles);
                }
                x.AllowNullCollections = true;
            });

            var mapper = mappingConfig.CreateMapper();

            mapper.ConfigurationProvider.AssertConfigurationIsValid();
            services.AddSingleton(mapper);
            return services;
        }
    }
}
