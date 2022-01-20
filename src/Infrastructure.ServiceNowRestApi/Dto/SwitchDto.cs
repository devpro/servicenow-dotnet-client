using System.Collections.Generic;
using Newtonsoft.Json;

namespace RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestApi.Dto
{
    internal partial class SwitchDto
    {
        [JsonProperty("sys_id")]
        public string? SysId { get; set; }

        public string? Name { get; set; }

        public bool? Stack { get; set; }

        [JsonProperty("serial_number")]
        public string? SerialNumber { get; set; }

        [JsonProperty("ip_address")]
        public string? IpAddress { get; set; }

        internal Dictionary<string, string>? ToDictionary()
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

            if (Stack.HasValue)
            {
                dictionary["stack"] = Stack.Value.ToString();
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
