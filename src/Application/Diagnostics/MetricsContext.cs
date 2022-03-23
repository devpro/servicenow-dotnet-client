using System.Collections.Generic;
using System.Diagnostics.Metrics;
using Microsoft.Extensions.Configuration;
using RabbidsIncubator.ServiceNowClient.Domain.Diagnostics;

namespace RabbidsIncubator.ServiceNowClient.Application.Diagnostics
{
    public class MetricsContext : IMetricsContext
    {
        private readonly string _name;

        private readonly Dictionary<string, Instrument> _instruments = new();

        private Meter? _meter;

        public MetricsContext(IConfiguration configuration)
        {
            _name = configuration[ConfigurationConstants.OpenTelemetryMetricsMeterConfigKey];
        }

        public Meter Meter
        {
            get
            {
                if (_meter == null)
                {
                    _meter = new Meter(_name);
                }
                return _meter;
            }
        }

        public void AddToCounter<T>(string name, T value, KeyValuePair<string, object?>? tag = null)
            where T : struct
        {
            if (!_instruments.ContainsKey(name))
            {
                CreateCounter<T>(name);
            }

            if (_instruments.ContainsKey(name) && _instruments[name] is Counter<T> counter)
            {
                if (tag == null)
                {
                    counter.Add(value);
                }
                else
                {
                    counter.Add(value, tag.Value);
                }
            }
        }

        private Counter<T> CreateCounter<T>(string name)
            where T : struct
        {
            var counter = Meter.CreateCounter<T>(name);
            _instruments.Add(name, counter);
            return counter;
        }
    }
}
