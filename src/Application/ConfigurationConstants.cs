namespace RabbidsIncubator.ServiceNowClient.Application
{
    public static class ConfigurationConstants
    {
        public const string AzureAdConfigKey = "AzureAd";

        public const string IsSecuredByAzureAdConfigKey = "Application:IsSecuredByAzureAd";

        public const string IsSwaggerEnabledConfigKey = "Application:IsSwaggerEnabled";

        public const string IsHttpsEnforcedConfigKey = "Application:IsHttpsEnforced";

        public const string InMemoryCacheConfigKey = "Cache:InMemory";

        public const string RestApiServiceNowConfigKey = "ServiceNow:RestApi";

        public const string SqlServerServiceNowConfigKey = "ServiceNow:SqlServer";
    }
}
