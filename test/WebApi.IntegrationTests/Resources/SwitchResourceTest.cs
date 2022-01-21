using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;

namespace RabbidsIncubator.ServiceNowClient.WebApi.IntegrationTests.Resources
{
    [Trait("Category", "IntegrationTests")]
    public class SwitchResourceTest : ResourceBase
    {
        private const string ResourceEndpoint = "switches";

        public SwitchResourceTest(WebApplicationFactory<Program> factory, ITestOutputHelper testOutput)
            : base(factory, testOutput)
        {
        }

        [Fact]
        [Trait("Mode", "Readonly")]
        public async Task SwitchResource_Get_ReturnsNotEmptyList()
        {
            var output = await GetAsync<List<Domain.Models.SwitchModel>>($"/{ResourceEndpoint}");
            output.Should().NotBeNull();
            output.Should().HaveCount(10);
        }
    }
}
