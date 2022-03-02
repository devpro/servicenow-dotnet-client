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
            if (bool.TryParse(configuration["Application:IsSwaggerEnabled"], out var isSwaggerEnabled) && isSwaggerEnabled)
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            if (bool.TryParse(configuration["Application:IsHttpsEnforced"], out var isHttpsEnforced) && isHttpsEnforced)
            {
                app.UseHttpsRedirection();
            }

            app.UseAuthorization();
            app.MapControllers();
            app.MapHealthChecks("/health");

            return app;
        }
    }
}
