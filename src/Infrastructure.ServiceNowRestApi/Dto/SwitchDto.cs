using Newtonsoft.Json;

namespace RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestApi.Dto
{
    internal partial class SwitchDto
    {
        public bool? Stack { get; set; }

        [JsonProperty("serial_number")]
        public string? SerialNumber { get; set; }

        [JsonProperty("sys_id")]
        public string? SysId { get; set; }

        [JsonProperty("ip_address")]
        public string? IpAddress { get; set; }
    }
}
