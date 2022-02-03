using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using RabbidsIncubator.Samples.ServiceNowWebApiSample.Domain;
using Xunit;
using Xunit.Abstractions;

namespace RabbidsIncubator.Samples.ServiceNowWebApiSample.IntegrationTests.Resources
{
    //[Trait("Category", "IntegrationTests")]
    public class ConfigurationItemRelationshipResourceTest : ResourceBase
    {
        private const string ResourceEndpoint = "configuration-item-relationships";

        public ConfigurationItemRelationshipResourceTest(WebApplicationFactory<Program> factory, ITestOutputHelper testOutput)
            : base(factory, testOutput)
        {
        }

        //[Fact]
        //[Trait("Mode", "Readonly")]
        public async Task ConfigurationItemRelationshipResource_Get_ReturnsNotEmptyList()
        {
            var output = await GetAsync<List<ConfigurationItemRelationshipModel>>($"/{ResourceEndpoint}");
            output.Should().NotBeNull();
            output.Should().NotBeEmpty();
        }
    }
}
