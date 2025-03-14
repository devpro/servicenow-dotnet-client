﻿using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using RabbidsIncubator.ServiceNowClient.Application.DependencyInjection;
using Xunit;

namespace RabbidsIncubator.ServiceNowClient.Application.UnitTests.DependencyInjection
{
    [Trait("Category", "UnitTests")]
    public class AutoMapperExtensionsTest
    {
        [Fact]
        public void AutoMapperExtensions_AddAutoMapper_RegisterIMapper()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();

            // Act
            Profile[] additionalProfiles = [];
            serviceCollection.AddAutoMapper(additionalProfiles);

            // Assert
            var serviceProvider = serviceCollection.BuildServiceProvider();
            serviceProvider.GetRequiredService<AutoMapper.IMapper>().Should().NotBeNull();
        }
    }
}
