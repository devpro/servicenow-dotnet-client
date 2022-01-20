using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using RabbidsIncubator.ServiceNowClient.Domain.Models;
using RabbidsIncubator.ServiceNowClient.Domain.Repositories;
using RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestApi.Dto;

namespace RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestApi.Repositories
{
    public class ConfigurationItemRelationshipRepository : ServiceNowRestApiRepositoryBase, IConfigurationItemRelationshipRepository
    {
        public ConfigurationItemRelationshipRepository(
            ILogger<ConfigurationItemRelationshipRepository> logger,
            IHttpClientFactory httpClientFactory,
            IMapper mapper,
            ServiceNowRestApiConfiguration restApiConfiguration)
            : base(logger, httpClientFactory, mapper, restApiConfiguration)
        {
        }

        public async Task<List<ConfigurationItemRelationshipModel>> FindAllAsync()
        {
            var url = GenerateUrl("cmdb_rel_ci", null, 0, 10);
            var resultList = await GetAsync<ResultListDto<ConfigurationItemRelationshipDto>>(url);
            return Mapper.Map<List<ConfigurationItemRelationshipModel>>(resultList.Result);
        }
    }
}
