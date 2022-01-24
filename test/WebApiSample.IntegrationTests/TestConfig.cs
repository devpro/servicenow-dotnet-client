using System;

namespace RabbidsIncubator.ServiceNowClient.WebApi.IntegrationTests
{
    public class TestConfig
    {
        public static bool IsLocalhostEnvironment => bool.Parse(Environment.GetEnvironmentVariable("IsLocalhostEnvironment") ?? "true");

        public static string ApiUrl => Environment.GetEnvironmentVariable("ApiUrl") ?? "http://localhost";
    }
}
