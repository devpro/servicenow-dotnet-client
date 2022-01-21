using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using RabbidsIncubator.ServiceNowClient.Domain.Repositories;
using RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestApi.DependencyInjection;
using Xunit;

namespace RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestApi.UnitTests.DependencyInjection
{
    [Trait("Category", "UnitTests")]
    public class ServiceCollectionExtensionsTest
    {
        [Fact]
        public void AddServiceNowRestApiRepositories_ShouldProvideRepositories()
        {
            // Arrange
            var configuration = CreateConfiguration();
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(new MapperConfiguration(x => { }).CreateMapper());

            // Act
            serviceCollection.AddServiceNowRestApiRepositories(configuration);

            // Assert
            var services = serviceCollection.BuildServiceProvider();
            services.GetRequiredService<IConfigurationItemRelationshipRepository>().Should().NotBeNull();
            services.GetRequiredService<ISwitchRepository>().Should().NotBeNull();
        }

        private static ServiceNowRestApiConfiguration CreateConfiguration()
        {
            return new ServiceNowRestApiConfiguration
            {
                BaseUrl = "https://dummy.service-now.doesntexist.com/api/now",
                Username = "bondjamesbond",
                Password = "mynameis"
            };
        }
    }
}
