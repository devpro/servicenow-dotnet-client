using RabbidsIncubator.ServiceNowClient.Application.DependencyInjection;
using RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestApi;
using RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestApi.DependencyInjection;

// creates the builder

var builder = WebApplication.CreateBuilder(args);

// adds services to the collection

builder.Services.AddAutoMapperConfiguration();
builder.Services.AddServiceNowRestApiRepositories(builder.Configuration.GetSection("ServiceNow:RestApi").Get<ServiceNowRestApiConfiguration>());
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

// creates the application

var app = builder.Build();

// configures the HTTP request pipeline

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

// runs the application

app.Run();

// fix: make Program class public for tests
// see: https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0#basic-tests-with-the-default-webapplicationfactory
#pragma warning disable CA1050 // Declare types in namespaces
public partial class Program { }
#pragma warning restore CA1050 // Declare types in namespaces
