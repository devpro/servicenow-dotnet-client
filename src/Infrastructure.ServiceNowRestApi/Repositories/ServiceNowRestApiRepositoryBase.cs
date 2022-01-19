using System.Net.Http;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestApi.Repositories
{
    /// <summary>
    /// Abstract class for repositories.
    /// </summary>
    public abstract class ServiceNowRestApiRepositoryBase : Withywoods.Net.Http.HttpRepositoryBase
    {
        private readonly ServiceNowRestApiConfiguration _restApiConfiguration;

        protected ServiceNowRestApiRepositoryBase(
            ILogger logger,
            IHttpClientFactory httpClientFactory,
            IMapper mapper,
            ServiceNowRestApiConfiguration restApiConfiguration)
            : base(logger, httpClientFactory)
        {
            Mapper = mapper;
            _restApiConfiguration = restApiConfiguration;
        }

        protected override string HttpClientName => _restApiConfiguration.HttpClientName;

        protected IMapper Mapper { get; private set; }

        /// <summary>
        /// Generate URL from parameters.
        /// </summary>
        /// <param name="tableName">Table name.</param>
        /// <param name="offset">Start index of the items to be returned, default to 0</param>
        /// <param name="limit">Limit number of items to be returned, default to 10.</param>
        /// <returns></returns>
        protected string GenerateUrl(string tableName, int offset = 0, int limit = 10)
        {
            return $"{_restApiConfiguration.BaseUrl}/table/{tableName}?sysparm_offset={offset}&sysparm_limit={limit}";
        }
    }
}
