using System;
using Microsoft.Extensions.Configuration;

namespace RabbidsIncubator.ServiceNowClient.Application.Configuration
{
    public static class ConfigurationExtensions
    {
        public static T TryGetSection<T>(this IConfiguration configuration, string sectionKey)
        {
            var section = configuration.GetSection(sectionKey);
            return section.Get<T>();
        }

        public static T GetSectionValue<T>(this IConfiguration configuration, string sectionKey)
        {
            var value = configuration.TryGetSection<T>(sectionKey);
            if (value == null)
            {
                throw new ArgumentException($"Invalid configuration section \"{sectionKey}\" for type \"{typeof(T)}\"",
                    nameof(sectionKey));
            }

            return value;
        }
    }
}
