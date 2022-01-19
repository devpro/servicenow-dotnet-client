using Microsoft.AspNetCore.Mvc;
using RabbidsIncubator.ServiceNowClient.Domain.Models;
using RabbidsIncubator.ServiceNowClient.Domain.Repositories;

namespace RabbidsIncubator.ServiceNowClient.WebApi.Controllers
{
    [ApiController]
    [Route("switches")]
    public class SwitchController : ControllerBase
    {
        private readonly ILogger _logger;

        private readonly ISwitchRepository _switchRepository;

        public SwitchController(ILogger<SwitchController> logger, ISwitchRepository switchRepository)
        {
            _logger = logger;
            _switchRepository = switchRepository;
        }

        [HttpGet(Name = "GetSwitches")]
        public async Task<List<SwitchModel>> Get(string? ipAddress)
        {
            var items = await _switchRepository.FindAllAsync();
            _logger.LogDebug($"Number of items found: {items.Count}");
            return items;
        }
    }
}
