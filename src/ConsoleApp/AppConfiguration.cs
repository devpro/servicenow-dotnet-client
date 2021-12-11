using Microsoft.Extensions.Configuration;
using RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestApi;

namespace RabbidsIncubator.ServiceNowClient.ConsoleApp
{
    internal class AppConfiguration
    {
        private readonly IConfigurationRoot _configurationRoot;

        public AppConfiguration(IConfigurationRoot configurationRoot)
        {
            _configurationRoot = configurationRoot;
        }

        public ServiceNowRestApiConfiguration ServiceNowRestApiConfiguration => _configurationRoot.GetSection("ServiceNow:RestApi").Get<ServiceNowRestApiConfiguration>();
    }
}
