namespace RabbidsIncubator.ServiceNowClient.Infrastructure.SqlServerClient
{
    public class SqlServerClientConfiguration
    {
        public string? DataSource { get; set; }

        public string? UserId { get; set; }

        public string? Password { get; set; }

        public string? InitialCatalog { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(DataSource)
                && !string.IsNullOrEmpty(UserId)
                && !string.IsNullOrEmpty(Password)
                && !string.IsNullOrEmpty(InitialCatalog);
        }
    }
}
