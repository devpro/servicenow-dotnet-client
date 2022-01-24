namespace RabbidsIncubator.ServiceNowClient.Application.Generators.Models
{
    public class QueryModel
    {
        public string Table { get; set; }

        public string Filter { get; set; } = string.Empty;
    }
}
