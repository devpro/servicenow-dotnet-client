using System.Collections.Generic;

namespace RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestClient.Dto
{
    public class ResultListDto<T>
    {
        public ResultListDto()
        {
            Result = new List<T>();
        }

        public List<T> Result { get; set; }
    }
}
