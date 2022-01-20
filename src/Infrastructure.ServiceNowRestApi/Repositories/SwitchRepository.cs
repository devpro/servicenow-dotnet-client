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
    public class SwitchRepository : ServiceNowRestApiRepositoryBase, ISwitchRepository
    {
        public SwitchRepository(
            ILogger<SwitchRepository> logger,
            IHttpClientFactory httpClientFactory,
            IMapper mapper,
            ServiceNowRestApiConfiguration restApiConfiguration)
            : base(logger, httpClientFactory, mapper, restApiConfiguration)
        {
        }

        public async Task<List<SwitchModel>> FindAllAsync(QueryModel<SwitchModel> query)
        {
            var url = GenerateUrl("cmdb_ci_ip_switch", GetFilterParameters(Mapper.Map<SwitchDto>(query.Filters)), query.StartIndex, query.Limit);
            var resultList = await GetAsync<ResultListDto<SwitchDto>>(url);
            return Mapper.Map<List<SwitchModel>>(resultList.Result);
        }

        private Dictionary<string, string> GetFilterParameters(SwitchDto filters)
        {
            return filters?.ToDictionary() ?? new Dictionary<string, string>();
        }
    }
}
