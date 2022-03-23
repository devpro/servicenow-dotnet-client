using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RabbidsIncubator.ServiceNowClient.Application.Middlewares
{
    /// <summary>
    /// Middleware that will add information to diagnostics activity.
    /// </summary>
    /// <remarks>
    /// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/write
    /// </remarks>
    public class DiagnosticsEnrichmentMiddleware
    {
        private readonly RequestDelegate _next;

        public DiagnosticsEnrichmentMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, Domain.Diagnostics.IMetricsContext metricsContext)
        {
            metricsContext.AddToCounter("http.requests", 1, KeyValuePair.Create<string, object?>("path", context.Request.Path.ToString()));

            Activity.Current?.AddTag("user.name", (context.User.Identity as ClaimsIdentity)?.FindFirst("name")?.Value);

            await _next(context);
        }
    }
}
