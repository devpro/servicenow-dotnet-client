using Newtonsoft.Json;

namespace RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestClient.Dto
{
    public partial class ConfigurationItemRelationshipDto
    {
        [JsonProperty("connection_strength")]
        public string? ConnectionStrength { get; set; }

        public LinkValueDto? Parent { get; set; }

        [JsonProperty("sys_mod_count")]
        public string? SysModCount { get; set; }

        [JsonProperty("sys_updated_on")]
        public string? SysUpdatedOn { get; set; }

        [JsonProperty("sys_tags")]
        public string? SysTags { get; set; }

        [JsonProperty("type")]
        public LinkValueDto? ConfigurationItemRelationshipType { get; set; }

        [JsonProperty("sys_id")]
        public string? SysId { get; set; }

        [JsonProperty("sys_updated_by")]
        public string? SysUpdatedBy { get; set; }

        public string? Port { get; set; }

        [JsonProperty("sys_created_on")]
        public string? SysCreatedOn { get; set; }

        [JsonProperty("percent_outage")]
        public string? PercentOutage { get; set; }

        [JsonProperty("sys_created_by")]
        public string? SysCreatedBy { get; set; }

        public LinkValueDto? Child { get; set; }
    }
}
