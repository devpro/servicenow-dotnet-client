using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbidsIncubator.ServiceNowClient.Domain.Models;
using RabbidsIncubator.ServiceNowClient.Domain.Repositories;
using RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestApi.Dto;

namespace RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestApi.Repositories
{
    public class ConfigurationItemRelationshipRepository : Withywoods.Net.Http.HttpRepositoryBase, IConfigurationItemRelationshipRepository
    {
        private readonly ServiceNowRestApiConfiguration _restApiConfiguration;

        public ConfigurationItemRelationshipRepository(
            ILogger<ConfigurationItemRelationshipRepository> logger,
            IHttpClientFactory httpClientFactory,
            ServiceNowRestApiConfiguration restApiConfiguration)
            : base(logger, httpClientFactory)
        {
            _restApiConfiguration = restApiConfiguration;
        }

        protected override string HttpClientName => _restApiConfiguration.HttpClientName;

        public async Task<List<ConfigurationItemRelationshipModel>> FindAllAsync()
        {
            var url = GenerateUrl("cmdb_rel_ci");
            var resultList = await GetAsync<ResultListDto<ConfigurationItemRelationshipDto>>(url);
            // TODO: use AutoMapper
            return resultList.Result
                .Select(x => new ConfigurationItemRelationshipModel
                {
                    Id = x.SystemId,
                    ParentId = x.Parent.Value,
                    TypeId = x.ConfigurationItemRelationshipType.Value
                }).ToList();
        }

        private string GenerateUrl(string tableName, int limit = 1)
        {
            return $"{_restApiConfiguration.BaseUrl}/table/{tableName}?sysparm_limit={limit}";
        }
    }
}
