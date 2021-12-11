using System.Collections.Generic;

namespace RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestApi.Dto
{
    internal class ResultListDto<T>
    {
        public ResultListDto()
        {
            Result = new List<T>();
        }

        public List<T> Result { get; set; }
    }
}
