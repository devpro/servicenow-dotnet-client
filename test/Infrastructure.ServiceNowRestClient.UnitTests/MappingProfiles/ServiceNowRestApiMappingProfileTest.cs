using AutoMapper;
using RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestClient.MappingProfiles;
using Xunit;

namespace RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestClient.UnitTests.MappingProfiles
{
    [Trait("Category", "UnitTests")]
    public class ServiceNowRestClientMappingProfileTest
    {
        [Fact]
        public void ServiceNowRestClientMappingProfile_ShouldProvideValidConfiguration()
        {
            var mappingConfig = new MapperConfiguration(x =>
            {
                x.AddProfile(new ServiceNowRestClientMappingProfile());
                x.AllowNullCollections = true;
            });
            var mapper = mappingConfig.CreateMapper();
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}
