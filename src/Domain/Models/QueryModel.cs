namespace RabbidsIncubator.ServiceNowClient.Domain.Models
{
    public class QueryModel<T>
        where T : class
    {
        public QueryModel(T? filters, int? startIndex, int? limit)
        {
            Filters = filters;
            if (startIndex.HasValue)
            {
                StartIndex = startIndex.Value;
            }
            if (limit.HasValue)
            {
                Limit = limit.Value;
            }
        }

        public T? Filters { get; set; }

        public int StartIndex { get; set; } = 0;

        public int Limit { get; set; } = 10;
    }
}
