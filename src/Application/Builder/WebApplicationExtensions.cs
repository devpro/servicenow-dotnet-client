using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace RabbidsIncubator.ServiceNowClient.Application.Builder
{
    public static class WebApplicationExtensions
    {
        public static WebApplication AddDefaultMiddlewares(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.MapHealthChecks("/health");

            return app;
        }
    }
}
