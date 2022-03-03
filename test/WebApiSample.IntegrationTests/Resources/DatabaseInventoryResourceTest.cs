using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using RabbidsIncubator.Samples.ServiceNowWebApiSample.Domain.Models;
using Xunit;
using Xunit.Abstractions;

namespace RabbidsIncubator.Samples.ServiceNowWebApiSample.IntegrationTests.Resources
{
    [Trait("Category", "IntegrationTests")]
    public class DatabaseInventoryResourceTest : ResourceBase
    {
        private const string ResourceEndpoint = "db-inventories";

        public DatabaseInventoryResourceTest(WebApplicationFactory<Program> factory, ITestOutputHelper testOutput)
            : base(factory, testOutput)
        {
        }

        [Fact]
        [Trait("Mode", "Readonly")]
        public async Task DatabaseInventory_Get_ReturnsNotEmptyList()
        {
            var output = await GetAsync<List<DatabaseInventoryModel>>($"/{ResourceEndpoint}");
            output.Should().NotBeNull();
            output.Count.Should().Be(2);
        }
    }
}
