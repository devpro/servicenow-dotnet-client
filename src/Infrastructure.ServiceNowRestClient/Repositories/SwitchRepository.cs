using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using RabbidsIncubator.ServiceNowClient.Domain.Models;
using RabbidsIncubator.ServiceNowClient.Domain.Repositories;
using RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestClient.Dto;

namespace RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestClient.Repositories
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
