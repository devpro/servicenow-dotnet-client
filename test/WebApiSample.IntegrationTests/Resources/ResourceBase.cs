using System;
using System.Net.Http;
using AutoFixture;
using Microsoft.AspNetCore.Mvc.Testing;
using Withywoods.WebTesting.Rest;
using Xunit;
using Xunit.Abstractions;

namespace RabbidsIncubator.Samples.ServiceNowWebApiSample.IntegrationTests.Resources
{
    public class ResourceBase : RestClient, IClassFixture<WebApplicationFactory<Program>>
    {
        protected ResourceBase(WebApplicationFactory<Program> factory, ITestOutputHelper testOutput)
            : base(TestConfig.IsLocalhostEnvironment ? factory.CreateClient() : new HttpClient { BaseAddress = new Uri(TestConfig.ApiUrl) })
        {
            TestOutput = testOutput;
        }

        protected TestConfig Configuration { get; } = new TestConfig();

        protected Fixture Fixture { get; } = new Fixture();

        protected ITestOutputHelper TestOutput { get; }
    }
}
