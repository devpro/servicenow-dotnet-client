using System.Collections.Generic;

namespace RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestClient.Dto
{
    public interface IEntityDto
    {
        Dictionary<string, string>? ToDictionary();
    }
}
