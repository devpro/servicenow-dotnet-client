using Microsoft.AspNetCore.Mvc;
using RabbidsIncubator.Samples.ServiceNowWebApiSample.Domain;
using RabbidsIncubator.ServiceNowClient.Domain.Models;

namespace RabbidsIncubator.Samples.ServiceNowWebApiSample.Controllers
{
    [ApiController]
    [Route("switches")]
    public class SwitchController : RabbidsIncubator.ServiceNowClient.Application.Mvc.ControllerBase
    {
        private readonly ISwitchRepository _switchRepository;

        public SwitchController(ILogger<SwitchController> logger, ISwitchRepository switchRepository)
            : base(logger)
        {
            _switchRepository = switchRepository;
        }

        [HttpGet(Name = "GetSwitches")]
        public async Task<List<SwitchModel>> Get([FromQuery] SwitchModel model, int? startIndex, int? limit)
        {
            var items = await _switchRepository.FindAllAsync(new QueryModel<SwitchModel>(model, startIndex, limit));
            ReportListCount(items.Count);
            return items;
        }
    }
}
