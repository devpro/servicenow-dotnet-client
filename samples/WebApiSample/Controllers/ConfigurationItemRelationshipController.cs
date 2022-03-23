using Microsoft.AspNetCore.Mvc;
using RabbidsIncubator.Samples.ServiceNowWebApiSample.Domain;

namespace RabbidsIncubator.Samples.ServiceNowWebApiSample.Controllers
{
    [ApiController]
    [Route("configuration-item-relationships")]
    public class ConfigurationItemRelationshipController : RabbidsIncubator.ServiceNowClient.Application.Mvc.ControllerBase
    {
        private readonly IConfigurationItemRelationshipRepository _configurationItemRelationshipRepository;

        public ConfigurationItemRelationshipController(
            ILogger<ConfigurationItemRelationshipController> logger,
            IConfigurationItemRelationshipRepository configurationItemRelationshipRepository)
            : base(logger)
        {
            _configurationItemRelationshipRepository = configurationItemRelationshipRepository;
        }

        [HttpGet(Name = "GetConfigurationItemRelationships")]
        public async Task<List<ConfigurationItemRelationshipModel>> Get()
        {
            var items = await _configurationItemRelationshipRepository.FindAllAsync();
            ReportListCount(items.Count);
            return items;
        }
    }
}
