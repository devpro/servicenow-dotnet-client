using System.Collections.Generic;

namespace RabbidsIncubator.ServiceNowClient.Domain.Diagnostics
{
    public interface IMetricsContext
    {
        void AddToCounter<T>(string name, T value, KeyValuePair<string, object?>? tag = null) where T : struct;
    }
}
