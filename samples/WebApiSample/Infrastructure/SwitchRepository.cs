using AutoMapper;
using RabbidsIncubator.Samples.ServiceNowWebApiSample.Domain;
using RabbidsIncubator.ServiceNowClient.Domain.Models;
using RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestClient;
using RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestClient.Repositories;

namespace RabbidsIncubator.Samples.ServiceNowWebApiSample.Infrastructure
{
    public class SwitchRepository : ServiceNowRestClientRepositoryBase, ISwitchRepository
    {
        public SwitchRepository(
            ILogger<SwitchRepository> logger,
            IHttpClientFactory httpClientFactory,
            IMapper mapper,
            ServiceNowRestClientConfiguration restApiConfiguration)
            : base(logger, httpClientFactory, mapper, restApiConfiguration)
        {
        }

        public async Task<List<SwitchModel>> FindAllAsync(QueryModel<SwitchModel> query)
        {
            return await FindAllAsync<SwitchModel, SwitchDto>("cmdb_ci_ip_switch", query);
        }
    }
}
