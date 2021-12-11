namespace RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestApi
{
    public class ServiceNowRestApiConfiguration
    {
        /// <summary>
        /// ServiceNow REST Api base URL.
        /// </summary>
        /// <example>https://dev12345.service-now.com/api/now</example>
        public string? BaseUrl { get; set; }

        public string HttpClientName { get; } = "ServiceNowRestApiClient";

        public string? Username { get; set; }

        public string? Password { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(BaseUrl)
                && !string.IsNullOrEmpty(HttpClientName)
                && !string.IsNullOrEmpty(Username)
                && !string.IsNullOrEmpty(Password);
        }
    }
}
