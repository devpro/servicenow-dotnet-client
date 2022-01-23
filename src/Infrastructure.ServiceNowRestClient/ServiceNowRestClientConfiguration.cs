namespace RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestClient
{
    public class ServiceNowRestClientConfiguration
    {
        /// <summary>
        /// ServiceNow REST Api base URL.
        /// </summary>
        /// <example>https://dev12345.service-now.com/api/now</example>
        public string? BaseUrl { get; set; }

        public string HttpClientName { get; } = "ServiceNowRestClient";

        /// <summary>
        /// Username of the ServiceNow REST Api user account.
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// Password of the ServiceNow REST Api user account.
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Is the configuration valid?
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(BaseUrl)
                && !string.IsNullOrEmpty(HttpClientName)
                && !string.IsNullOrEmpty(Username)
                && !string.IsNullOrEmpty(Password);
        }
    }
}
