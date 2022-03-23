using AutoMapper;
using RabbidsIncubator.Samples.ServiceNowWebApiSample.Domain;
using RabbidsIncubator.ServiceNowClient.Domain.Diagnostics;
using RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestClient;
using RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestClient.Dto;
using RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestClient.Repositories;

namespace RabbidsIncubator.Samples.ServiceNowWebApiSample.Infrastructure
{
    public class ConfigurationItemRelationshipRepository : ServiceNowRestClientRepositoryBase, IConfigurationItemRelationshipRepository
    {
        public ConfigurationItemRelationshipRepository(
            ILogger<ConfigurationItemRelationshipRepository> logger,
            IHttpClientFactory httpClientFactory,
            IMapper mapper,
            ServiceNowRestClientConfiguration restApiConfiguration,
            IMetricsContext metricsContext)
            : base(logger, httpClientFactory, mapper, restApiConfiguration, metricsContext)
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
