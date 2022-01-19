using Newtonsoft.Json;

namespace RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestApi.Dto
{
    internal partial class ConfigurationItemRelationshipDto
    {
        [JsonProperty("connection_strength")]
        public string? ConnectionStrength { get; set; }

        public LinkValueDto? Parent { get; set; }

        [JsonProperty("sys_mod_count")]
        public string? SystemModCount { get; set; }

        [JsonProperty("sys_updated_on")]
        public string? SystemUpdatedOn { get; set; }

        [JsonProperty("sys_tags")]
        public string? SystemTags { get; set; }

        [JsonProperty("type")]
        public LinkValueDto? ConfigurationItemRelationshipType { get; set; }

        [JsonProperty("sys_id")]
        public string? SystemId { get; set; }

        [JsonProperty("sys_updated_by")]
        public string? SystemUpdatedBy { get; set; }

        public string? Port { get; set; }

        [JsonProperty("sys_created_on")]
        public string? SystemCreatedOn { get; set; }

        [JsonProperty("percent_outage")]
        public string? PercentOutage { get; set; }

        [JsonProperty("sys_created_by")]
        public string? SystemCreatedBy { get; set; }

        public LinkValueDto? Child { get; set; }
    }
}
