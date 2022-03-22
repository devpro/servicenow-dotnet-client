using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RabbidsIncubator.ServiceNowClient.Application.Middlewares
{
    /// <summary>
    /// Middleware that will add information to diagnostics activity.
    /// </summary>
    /// <remarks>https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/write</remarks>
    public class ActivityEnrichmentMiddleware
    {
        // idea: other idea to achieve the same goal (https://rehansaeed.com/optimally-configuring-open-telemetry-tracing-for-asp-net-core/)

        private readonly RequestDelegate _next;

        public ActivityEnrichmentMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Activity.Current?.AddTag("user.name2", (context.User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.Name)?.Value);
            Activity.Current?.AddTag("user.name", (context.User.Identity as ClaimsIdentity)?.FindFirst("name")?.Value);

            await _next(context);
        }
    }
}
