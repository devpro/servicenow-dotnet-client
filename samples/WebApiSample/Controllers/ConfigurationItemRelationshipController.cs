using Microsoft.AspNetCore.Mvc;
using RabbidsIncubator.Samples.ServiceNowWebApiSample.Domain;

namespace RabbidsIncubator.Samples.ServiceNowWebApiSample.Controllers
{
    [ApiController]
    [Route("configuration-item-relationships")]
    public class ConfigurationItemRelationshipController : ControllerBase
    {
        private readonly IConfigurationItemRelationshipRepository _configurationItemRelationshipRepository;

        private readonly ILogger _logger;

        public ConfigurationItemRelationshipController(
            ILogger<ConfigurationItemRelationshipController> logger,
            IConfigurationItemRelationshipRepository configurationItemRelationshipRepository)
        {
            _logger = logger;
            _configurationItemRelationshipRepository = configurationItemRelationshipRepository;
        }

        [HttpGet(Name = "GetConfigurationItemRelationships")]
        public async Task<List<ConfigurationItemRelationshipModel>> Get()
        {
            var items = await _configurationItemRelationshipRepository.FindAllAsync();
            _logger.LogDebug("Number of items found: {itemsCount}", items.Count);
            return items;
        }
    }
}
