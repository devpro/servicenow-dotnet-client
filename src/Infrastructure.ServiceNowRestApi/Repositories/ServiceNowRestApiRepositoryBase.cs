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
        /// <param name="limit">Limit of items to be returned, default to 1.</param>
        /// <returns></returns>
        protected string GenerateUrl(string tableName, int limit = 1)
        {
            return $"{_restApiConfiguration.BaseUrl}/table/{tableName}?sysparm_limit={limit}";
        }
    }
}
