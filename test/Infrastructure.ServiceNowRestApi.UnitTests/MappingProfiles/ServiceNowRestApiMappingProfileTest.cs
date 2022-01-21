using AutoMapper;
using RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestApi.MappingProfiles;
using Xunit;

namespace RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestApi.UnitTests.MappingProfiles
{
    [Trait("Category", "UnitTests")]
    public class ServiceNowRestApiMappingProfileTest
    {
        [Fact]
        public void ServiceNowRestApiMappingProfile_ShouldProvideValidConfiguration()
        {
            var mappingConfig = new MapperConfiguration(x =>
            {
                x.AddProfile(new ServiceNowRestApiMappingProfile());
                x.AllowNullCollections = true;
            });
            var mapper = mappingConfig.CreateMapper();
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}
