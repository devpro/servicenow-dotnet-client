using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using RabbidsIncubator.ServiceNowClient.Domain.Repositories;
using RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestClient.DependencyInjection;
using Xunit;

namespace RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestClient.UnitTests.DependencyInjection
{
    [Trait("Category", "UnitTests")]
    public class ServiceCollectionExtensionsTest
    {
        [Fact]
        public void AddServiceNowRestClientRepositories_ShouldProvideRepositories()
        {
            // Arrange
            var configuration = CreateConfiguration();
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(new MapperConfiguration(x => { }).CreateMapper());

            // Act
            serviceCollection.AddServiceNowRestClientRepositories(configuration);

            // Assert
            var services = serviceCollection.BuildServiceProvider();
            services.GetRequiredService<IConfigurationItemRelationshipRepository>().Should().NotBeNull();
            services.GetRequiredService<ISwitchRepository>().Should().NotBeNull();
        }

        private static ServiceNowRestClientConfiguration CreateConfiguration()
        {
            return new ServiceNowRestClientConfiguration
            {
                BaseUrl = "https://dummy.service-now.doesntexist.com/api/now",
                Username = "bondjamesbond",
                Password = "mynameis"
            };
        }
    }
}
