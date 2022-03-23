using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace RabbidsIncubator.ServiceNowClient.Application.Mvc
{
    public abstract class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        protected ILogger Logger { get; private set; }

        protected ControllerBase(ILogger<ControllerBase> logger)
        {
            Logger = logger;
        }

        protected void ReportListCount(int count)
        {
            Logger.LogDebug("Number of items found: {itemsCount}", count);
            Activity.Current?.AddTag("response.list_size", count);
        }
    }
}
