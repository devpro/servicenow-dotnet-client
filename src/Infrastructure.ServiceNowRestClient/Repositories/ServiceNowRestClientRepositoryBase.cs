﻿using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using RabbidsIncubator.ServiceNowClient.Domain.Diagnostics;

namespace RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestClient.Repositories
{
    /// <summary>
    /// Abstract class for repositories.
    /// </summary>
    /// <remarks>
    /// https://docs.servicenow.com/bundle/rome-application-development/page/integrate/inbound-rest/concept/c_RESTAPI.html
    /// </remarks>
    public abstract class ServiceNowRestClientRepositoryBase : Withywoods.Net.Http.HttpRepositoryBase
    {
        private readonly ServiceNowRestClientConfiguration _restApiConfiguration;

        protected ServiceNowRestClientRepositoryBase(
            ILogger logger,
            IHttpClientFactory httpClientFactory,
            IMapper mapper,
            ServiceNowRestClientConfiguration restApiConfiguration,
            IMetricsContext metricsContext)
            : base(logger, httpClientFactory)
        {
            Mapper = mapper;
            _restApiConfiguration = restApiConfiguration;
            MetricsContext = metricsContext;
        }

        protected override string HttpClientName => _restApiConfiguration.HttpClientName;

        protected IMapper Mapper { get; private set; }

        protected IMetricsContext MetricsContext { get; private set; }

        /// <summary>
        /// Generate URL from parameters.
        /// </summary>
        /// <param name="tableName">Table name</param>
        /// <param name="filters">Filter dictionary</param>
        /// <param name="offset">Start index of the items to be returned, default to 0</param>
        /// <param name="limit">Limit number of items to be returned, default to 10</param>
        /// <param name="extraParameters">Extra parameters to be added</param>
        /// <returns></returns>
        protected string GenerateUrl(string tableName, Dictionary<string, string>? filters, int offset, int limit, string extraParameters = "")
        {
            var filterParameters = filters == null ? string.Empty : $"&{string.Join("&", filters.Select(kv => kv.Key + "=" + kv.Value).ToArray())}";
            return $"{_restApiConfiguration.BaseUrl}/table/{tableName}?sysparm_offset={offset}&sysparm_limit={limit}{filterParameters}{extraParameters}";
        }

        /// <summary>
        /// Generate URL from parameters.
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <typeparam name="U">Dto</typeparam>
        /// <param name="tableName">Table name</param>
        /// <param name="queryModel">Query model input object</param>
        /// <param name="extraParameters">Extra parameters to be added</param>
        /// <returns></returns>
        protected string GenerateUrl<T, U>(string tableName, Domain.Models.QueryModel<T> queryModel, string extraParameters = "")
            where T : class
            where U : Dto.IEntityDto
        {
            return GenerateUrl(tableName, GetFilterParameters<T, U>(queryModel), queryModel.StartIndex, queryModel.Limit, extraParameters);
        }

        /// <summary>
        /// Get filter parameters as an array.
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <typeparam name="U">Dto</typeparam>
        /// <param name="queryModel">Query model input object</param>
        /// <returns></returns>
        protected virtual Dictionary<string, string> GetFilterParameters<T, U>(Domain.Models.QueryModel<T> queryModel)
            where T : class
            where U : Dto.IEntityDto
        {
            return Mapper.Map<U>(queryModel.Filters)?.ToDictionary() ?? new Dictionary<string, string>();
        }

        /// <summary>
        /// Find all records.
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <typeparam name="U">Dto</typeparam>
        /// <param name="tableName">Table name</param>
        /// <param name="queryModel">Query model input object</param>
        /// <param name="extraParameters">Extra parameters to be added</param>
        /// <returns></returns>
        protected async Task<List<T>> FindAllAsync<T, U>(string tableName, Domain.Models.QueryModel<T> queryModel, string extraParameters = "")
            where T : class
            where U : Dto.IEntityDto
        {
            MetricsContext.AddToCounter("servicenow.restapi_requests", 1);
            var url = GenerateUrl<T, U>(tableName, queryModel, extraParameters);
            var resultList = await GetAsync<Dto.ResultListDto<U>>(url);
            return Mapper.Map<List<T>>(resultList.Result);
        }
    }
}
