# Observability

This solution uses [OpenTelemetry](https://opentelemetry.io/) to send logs, traces and metrics
in a vendor-neutral, robust and standardized way.

## OpenTelemetry

* Blog articles
  * [Monitoring a .NET application using OpenTelemetry](
https://www.meziantou.net/monitoring-a-dotnet-application-using-opentelemetry.htm), by Gérald Barré - November 15, 2021
  * [OpenTelemetry in .NET](
https://rafaelldi.blog/posts/open-telemetry-in-dotnet/) by Rival Abdrakhmanov - January 14, 2022
  * [Optimally Configuring Open Telemetry Tracing for ASP.NET Core](
https://rehansaeed.com/optimally-configuring-open-telemetry-tracing-for-asp-net-core/) by Muhammad Rehan Saeed - February 3, 2022
* Documentation
  * [.NET / Collect metrics](https://docs.microsoft.com/en-us/dotnet/core/diagnostics/metrics-collection)
  * [OpenTelemetry / Instrumentation / .NET](https://opentelemetry.io/docs/instrumentation/net/)
* Projects
  * [ASP.NET Core Instrumentation for OpenTelemetry](
https://github.com/open-telemetry/opentelemetry-dotnet/tree/main/src/OpenTelemetry.Instrumentation.AspNetCore)
    * Current metrics: `http.server.duration` (histogram)
  * [OpenTelemetry Collector](https://github.com/open-telemetry/opentelemetry-collector)
  * [OpenTelemetry Collector Contrib](https://github.com/open-telemetry/opentelemetry-collector-contrib)
* Limitation
  * .NET gRPC: [#3023](https://github.com/open-telemetry/opentelemetry-dotnet/issues/3023)
* Issue
  * OpenTelemetry .NET RC9 not working with metrics: [#3078](https://github.com/open-telemetry/opentelemetry-dotnet/issues/3078)
