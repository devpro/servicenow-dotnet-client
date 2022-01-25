using Newtonsoft.Json;
using RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestClient.Dto;

namespace RabbidsIncubator.Samples.ServiceNowWebApiSample.Infrastructure
{
    public partial class SwitchDto : IEntityDto
    {
        [JsonProperty("sys_id")]
        public string? SysId { get; set; }

        public string? Name { get; set; }

        [JsonProperty("serial_number")]
        public string? SerialNumber { get; set; }

        [JsonProperty("ip_address")]
        public string? IpAddress { get; set; }

        public Dictionary<string, string>? ToDictionary()
        {
            var dictionary = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(SysId))
            {
                dictionary["sys_id"] = SysId;
            }

            if (!string.IsNullOrEmpty(Name))
            {
                dictionary["name"] = Name;
            }

            if (!string.IsNullOrEmpty(SerialNumber))
            {
                dictionary["serial_number"] = SerialNumber;
            }

            if (!string.IsNullOrEmpty(IpAddress))
            {
                dictionary["ip_address"] = IpAddress;
            }

            return dictionary;
        }
    }
}
