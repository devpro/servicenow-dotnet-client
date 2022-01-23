using Microsoft.Extensions.Configuration;
using RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestClient;

namespace RabbidsIncubator.ServiceNowClient.ConsoleApp
{
    internal class AppConfiguration
    {
        private readonly IConfigurationRoot _configurationRoot;

        public AppConfiguration(IConfigurationRoot configurationRoot)
        {
            _configurationRoot = configurationRoot;
        }

        public ServiceNowRestClientConfiguration ServiceNowRestClientConfiguration =>
            _configurationRoot.GetSection("ServiceNow:RestApi").Get<ServiceNowRestClientConfiguration>();
    }
}
