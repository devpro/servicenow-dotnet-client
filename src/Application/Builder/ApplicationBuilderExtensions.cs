using Microsoft.AspNetCore.Builder;
using RabbidsIncubator.ServiceNowClient.Application.Middlewares;

namespace RabbidsIncubator.ServiceNowClient.Application.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseActivityEnrichment(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ActivityEnrichmentMiddleware>();
        }
    }
}
