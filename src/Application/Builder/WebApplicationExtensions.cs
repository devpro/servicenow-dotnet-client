using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using RabbidsIncubator.ServiceNowClient.Application.Configuration;

namespace RabbidsIncubator.ServiceNowClient.Application.Builder
{
    public static class WebApplicationExtensions
    {
        /// <summary>
        /// Add default middleware.
        /// Expected configuration elements: "Application:IsSwaggerEnabled", "Application:IsHttpsEnforced".
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static WebApplication AddDefaultMiddlewares(
            this WebApplication app,
            ConfigurationManager configuration)
        {
            if (bool.TryParse(configuration[ConfigurationConstants.IsSwaggerEnabledConfigKey], out var isSwaggerEnabled) && isSwaggerEnabled)
            {
                var openApi = configuration.GetSectionValue<OpenApiInfo>(ConfigurationConstants.OpenApiConfigKey);
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint($"/swagger/{openApi.Version}/swagger.json",
                    $"{openApi.Title} {openApi.Version}"));
            }

            if (bool.TryParse(configuration[ConfigurationConstants.IsHttpsEnforcedConfigKey], out var isHttpsEnforced) && isHttpsEnforced)
            {
                app.UseHttpsRedirection();
            }

            if (bool.TryParse(configuration[ConfigurationConstants.IsSecuredByAzureAdConfigKey], out var isSecuredByAzureAd) && isSecuredByAzureAd)
            {
                app.UseAuthentication();
            }

            app.UseActivityEnrichment();
            app.UseAuthorization();
            app.MapControllers();
            app.MapHealthChecks("/health");

            return app;
        }
    }
}
