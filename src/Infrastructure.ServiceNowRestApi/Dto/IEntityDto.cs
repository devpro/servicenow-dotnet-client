using System.Collections.Generic;

namespace RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestApi.Dto
{
    public interface IEntityDto
    {
        Dictionary<string, string>? ToDictionary();
    }
}
