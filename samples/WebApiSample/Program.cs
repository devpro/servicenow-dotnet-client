var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDefaultServices(builder.Configuration, new GeneratedServiceNowRestClientMappingProfile(), new InfrastructureMappingProfile());
builder.Services.AddWebApiSampleInfrastructureRepositories();
builder.Services.AddServiceNowRestClientGeneratedRepositories();
builder.Services.AddSqlServerClientClientGeneratedRepositories();

var app = builder.Build();
app.AddDefaultMiddlewares(builder.Configuration);

app.Run();

// fix: make Program class public for tests
// see: https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0#basic-tests-with-the-default-webapplicationfactory
#pragma warning disable CA1050 // Declare types in namespaces
public partial class Program { }
#pragma warning restore CA1050 // Declare types in namespaces
