using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

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
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            if (bool.TryParse(configuration[ConfigurationConstants.IsHttpsEnforcedConfigKey], out var isHttpsEnforced) && isHttpsEnforced)
            {
                app.UseHttpsRedirection();
            }

            if (bool.TryParse(configuration[ConfigurationConstants.IsSecuredByAzureAdConfigKey], out var isSecuredByAzureAd) && isSecuredByAzureAd)
            {
                app.UseAuthentication();
            }

            app.UseAuthorization();
            app.MapControllers();
            app.MapHealthChecks("/health");

            return app;
        }
    }
}
