﻿using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using RabbidsIncubator.ServiceNowClient.Domain.Repositories;
using RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestApi.DependencyInjection;
using Xunit;

namespace RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestApi.UnitTests.DependencyInjection
{
    [Trait("Category", "UnitTests")]
    public class ServiceCollectionExtensionsUnitTest
    {
        [Fact]
        public void AddServiceNowRestApiRepositories_ShouldProvideRepositories()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();
            var configuration = CreateServiceNowRestApiConfiguration();
            serviceCollection.AddSingleton(new MapperConfiguration(x => { }).CreateMapper());

            // Act
            serviceCollection.AddServiceNowRestApiRepositories(configuration);

            // Assert
            var services = serviceCollection.BuildServiceProvider();
            var organizationRepository = services.GetRequiredService<IConfigurationItemRelationshipRepository>();
            organizationRepository.Should().NotBeNull();
        }

        private static ServiceNowRestApiConfiguration CreateServiceNowRestApiConfiguration()
        {
            return new ServiceNowRestApiConfiguration()
            {
                BaseUrl = "https://dummy.service-now.doesntexist.com/api/now",
                Username = "bondjamesbond",
                Password = "mynameis"
            };
        }
    }
}
