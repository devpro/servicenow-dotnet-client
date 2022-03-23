# Quickstart

## Requirements

### ServiceNow user account (authentication & permissions)

You can ask your organization for one or use a sandbox environment provided by the free
[ServiceNow Developer program](https://developer.servicenow.com/dev.do).

Users can be created from the "Application Navigator" (URL like "https://devXXXXX.service-now.com/"):

* In "User Administration" > "Users"
  * Create a new User and make sure "Web service access only" option is checked
  * Once created, open the newly created user and edit roles to add the CMDB ones

### .NET SDK

[.NET 6.0 SDK](https://dotnet.microsoft.com/download) (or above) must be installed to be able to create
a project and use NuGet packages.

```bash
dotnet --version
```

Optionally, you can install .NET global tool to help you improve the source code:

* [dotnet format](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-format) ([GitHub](https://github.com/dotnet/format))

```bash
dotnet tool install --global dotnet-format
```

## Create a REST API entirely from the command line

* From a new folder (ideally a new git repository)

```bash
# creates a new .NET solution
dotnet new sln --name SolutionName

# creates a new ASP.NET project
dotnet new webapi --output src/WebApi --name WebApi
# cleans the project from WeatherForecast examples
rm src/WebApi/WeatherForecast.cs src/WebApi/Controllers/WeatherForecastController.cs
# explicits assembly name and root namespace
sed -i 's|<TargetFramework>net6.0</TargetFramework>|<TargetFramework>net6.0</TargetFramework>\n    <AssemblyName>WebApi</AssemblyName>\n    <RootNamespace>WebApi</RootNamespace>|' src/WebApi/WebApi.csproj
# adds the new project in solution file
dotnet sln SolutionName.sln add src/WebApi/WebApi.csproj

# adds package references
dotnet add src/WebApi package RabbidsIncubator.ServiceNowClient.Application
dotnet add src/WebApi package RabbidsIncubator.ServiceNowClient.Application.Generators

# creates entities yaml file that is used to configure entirely the REST API
cat > src/WebApi/entities.yml <<EOF
namespaces:
  root: WebApi
  webApi: WebApi
targetApplication: WebApp
entities:
  - name: Location
    resourceName: locations
    queries:
      findAll:
        table: cmn_location
    fields:
      - name: Name
        serviceNowFieldName: name
      - name: City
        serviceNowFieldName: city
      - name: CountryName
        serviceNowFieldName: country
      - name: Latitude
        serviceNowFieldName: latitude
      - name: Longitude
        serviceNowFieldName: longitude
EOF
# adds entities.yml as WebApi additional file
sed -i 's|</Project>|  <ItemGroup>\n    <AdditionalFiles Include="entities.yml" />\n  </ItemGroup>\n\n</Project>|' src/WebApi/WebApi.csproj

# rewrites configuration file
rm src/WebApi/appsettings.json
cat > src/WebApi/appsettings.json <<EOF
{
  "AllowedHosts": "*",
  "Application": {
    "IsHttpsEnforced": false,
    "IsOpenTelemetryEnabled": false,
    "IsSecuredByAzureAd": false,
    "IsSwaggerEnabled": false
  },
  "AzureAd": {
    "Instance": "https://login.microsoftonline.com/",
    "CallbackPath": "/signin-oidc",
    "SignedOutCallbackPath": "/signout-callback-oidc"
  },
  "Cache": {
    "InMemory": {
      "GeneralTimeoutInSeconds": 3600
    }
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "RabbidsIncubator.ServiceNowClient": "Information"
    }
  },
  "OpenApi": {
    "Title": "ServiceNow Client Web API sample",
    "Version": "v1.0"
  },
  "OpenTelemetry": {
    "Metrics": {
      "Source": "SampleServiceNowRestClientMetrics"
    },
    "Tracing": {
      "Source": "SampleServiceNowRestClientTracing"
    }
  }
}
EOF

# configures the application from ServiceNow authenticication information (this file won't be versioned)
rm src/WebApi/appsettings.Development.json
# /!\ replace the *** with the correct values
cat > src/WebApi/appsettings.Development.json <<EOF
{
  "Application": {
    "IsHttpsEnforced": false,
    "IsOpenTelemetryEnabled": true,
    "IsSecuredByAzureAd": false,
    "IsSwaggerEnabled": true
  },
  "AzureAd": {
    "Domain": "***.onmicrosoft.com",
    "TenantId": "***",
    "ClientId": "***",
    "ClientSecret": "***"
  },
  "Logging": {
    "LogLevel": {
      "RabbidsIncubator.ServiceNowClient": "Debug",
      "Withywoods": "Debug"
    }
  },
  "OpenTelemetry": {
    "OtlpExporter": {
      "Endpoint": "http://localhost:4317"
    }
  },
  "ServiceNow": {
    "RestApi": {
      "BaseUrl": "https://***.service-now.com/api/now",
      "Username": "***",
      "Password": "***"
    },
    "SqlServer": {
      "DataSource": "***",
      "UserId": "***",
      "Password": "***",
      "InitialCatalog": "***"
    }
  }
}
EOF

# simplifies Program.cs file
rm src/WebApi/Program.cs
cat > src/WebApi/Program.cs <<EOF
using RabbidsIncubator.ServiceNowClient.Application.Builder;
using RabbidsIncubator.ServiceNowClient.Application.DependencyInjection;
using WebApi.Infrastructure.ServiceNowRestClient.DependencyInjection;
using WebApi.Infrastructure.ServiceNowRestClient.MappingProfiles;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDefaultServices(builder.Configuration, builder.Logging, new GeneratedServiceNowRestClientMappingProfile());
builder.Services.AddServiceNowRestClientGeneratedRepositories();
builder.Services.AddSqlServerClientClientGeneratedRepositories();

var app = builder.Build();
app.AddDefaultMiddlewares();

app.Run();
EOF

# makes sure everything is ok by building the solution
dotnet build

# launches the application!
dotnet run --project src/WebApi

# checks the application with "/health" and "/swagger" endpoints (the URL is given in the command output)
```

* Improves codebase before git commit

```bash
# creates a .gitignore file from official repository (https://github.com/github/gitignore)
curl -o .gitignore https://raw.githubusercontent.com/github/gitignore/master/VisualStudio.gitignore
# completes the .gitignore file
printf "\n# Local configuration files\nappsettings.Development.json" >> .gitignore

# creates the .editorconfig file
curl -o .editorconfig https://raw.githubusercontent.com/dotnet/roslyn/main/.editorconfig

# runs .NET format global tool
dotnet format
```
