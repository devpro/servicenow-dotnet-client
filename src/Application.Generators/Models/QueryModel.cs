namespace RabbidsIncubator.ServiceNowClient.Application.Generators.Models
{
    /// <summary>
    /// Query model.
    /// </summary>
    public class QueryModel
    {
        /// <summary>
        /// ServiceNow REST API table.
        /// </summary>
        public string ServiceNowRestApiTable { get; set; } = string.Empty;

        /// <summary>
        /// SQL Server database table.
        /// </summary>
        public string SqlServerDatabaseTable { get; set; } = string.Empty;

        /// <summary>
        /// Query filter.
        /// </summary>
        public string Filter { get; set; } = string.Empty;
    }
}
