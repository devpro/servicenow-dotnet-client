using System.Collections.Generic;
using RabbidsIncubator.ServiceNowClient.Domain.Diagnostics;

namespace RabbidsIncubator.ServiceNowClient.Application.Diagnostics
{
    internal class NoMetricsContext : IMetricsContext
    {
        public void AddToCounter<T>(string name, T value, KeyValuePair<string, object?>? tag = null) where T : struct
        {
        }
    }
}
