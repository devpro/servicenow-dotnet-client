namespace RabbidsIncubator.ServiceNowClient.Application
{
    public static class ConfigurationConstants
    {
        public const string IsHttpsEnforcedConfigKey = "Application:IsHttpsEnforced";

        public const string IsSecuredByAzureAdConfigKey = "Application:IsSecuredByAzureAd";

        public const string IsSwaggerEnabledConfigKey = "Application:IsSwaggerEnabled";

        public const string IsOpenTelemetryEnabledConfigKey = "Application:IsOpenTelemetryEnabled";

        public const string AzureAdConfigKey = "AzureAd";

        public const string InMemoryCacheConfigKey = "Cache:InMemory";

        public const string OpenApiConfigKey = "OpenApi";

        public const string OpenTelemetryOtlpExporterEndpointConfigKey = "OpenTelemetry:OtlpExporter:Endpoint";

        public const string OpenTelemetryServiceConfigKey = "OpenTelemetry:Service";

        public const string ServiceNowRestApiConfigKey = "ServiceNow:RestApi";

        public const string ServiceNowSqlServerConfigKey = "ServiceNow:SqlServer";
    }
}
