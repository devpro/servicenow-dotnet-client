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
        /// <param name="generatedMappingProfile">Generated mapping profile, null if not provided</param>
        /// <returns></returns>
        public static IServiceCollection AddAutoMapperConfiguration(this IServiceCollection services, Profile? generatedMappingProfile = null)
        {
            var mappingConfig = new MapperConfiguration(x =>
            {
                x.AddProfile(new Infrastructure.ServiceNowRestClient.MappingProfiles.ServiceNowRestClientMappingProfile());
                if (generatedMappingProfile != null)
                {
                    x.AddProfile(generatedMappingProfile);
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
