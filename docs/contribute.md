# Contribute

## Requirements

* [git CLI](https://git-scm.com/)
* [.NET 6.0 SDK](https://dotnet.microsoft.com/download) (or above)
* (Optional) [Docker Engine](https://docs.docker.com/engine/install/ubuntu/)

## Steps

All commands are to be ran from the root folder of the repository (where the sln file is).

### Build

```bash
# restores .NET packages
dotnet restore

# builds the .NET solution
dotnet build
```

### Configure

* A ServiceNow user is needed to run the integration tests and sample projects (see [Getting Started](./getting-started.md))

* Create the file `src/ConsoleApp/Properties/launchSettings.json` 
(can be done from Visual Studio by opening `Properties` window of ConsoleApp project then `Debug` > `General`)

```json
# update with the values of your environment
{
  "profiles": {
    "ConsoleApp": {
      "commandName": "Project",
      "commandLineArgs": "-v",
      "environmentVariables": {
        "ServiceNow__RestApi__Username": "admin",
        "ServiceNow__RestApi__Password": "*********",
        "ServiceNow__RestApi__BaseUrl": "https://devxxxxx.service-now.com/api/now"
      }
    }
  }
}
```

* Create the file `src/WebApi/appsettings.Development.json`

```json
# update with the values of your environment
{
  "ServiceNow": {
    "RestApi": {
      "BaseUrl": "https://devxxxxx.service-now.com/api/now",
      "Username": "admin",
      "Password": ""*********"
    }
  }
}
```

* (Optional) Create at the root of the repositoty a file `Local.runsettings`

```xml
<?xml version="1.0" encoding="utf-8"?>
<RunSettings>
  <RunConfiguration>
    <EnvironmentVariables>
      <IsLocalhostEnvironment>true</IsLocalhostEnvironment>
      <ApiUrl></ApiUrl>
    </EnvironmentVariables>
  </RunConfiguration>
</RunSettings>
```

### Run the sample applications

```bash
# runs the Console project
dotnet run --project src/ConsoleApp

# runs the Console dll with options
dotnet src/ConsoleApp/bin/Debug/net6.0/RabbidsIncubator.ServiceNowClient.ConsoleApp.dll -v

# runs the Web Api project (will be accessible at https://localhost:7079/swagger)
dotnet run --project src/WebApi

# checks api health (should returned "Healthy")
curl -k https://localhost:7079/health
```

### Debug the sample Web API in Visual Studio 2022

* Add breakpoint(s) in the files
* Select `WebApi` in the startup project list
* Click on `Debug` > `Start Debugging` (`F5`)

### Debug a test Visual Studio 2022

* Add breakpoint(s) in the files
* (Optional) In the top menu, click on `Test` > `Configure Run Settings` > `Selection Solution Wide runsettings File`
and select `Local.runsettings` at the root level of the solution
* Select a test in the `Test Explorer` and click on Debug

### Run the tests

```bash
# runs all tests (unit + integration)
dotnet test
```

### Format the code

```bash
# review and update source files from the rules defined in .editorconfig file
dotnet format
```

### Run the sample Web Api in a container

```bash
# creates a Docker image for the webapi sample
docker build . -t rabbidsincubator/servicenowclientapisample -f samples/WebApiSample/Dockerfile --no-cache

# runs locally the webapi sample in a container
docker run -it --rm -p 8080:80 --name servicenowclientapisample \
  -e ASPNETCORE_ENVIRONMENT=Development \
  -e ServiceNow__RestApi__BaseUrl="xxx" -e ServiceNow__RestApi__Username="xxx" -e ServiceNow__RestApi__Password="xxx" \
  rabbidsincubator/servicenowclientapisample

# (once) logs in remote Docker repository (Artifactory)
docker login devpro.jfrog.io

# tags the image
docker tag <IMAGE_ID> devpro.jfrog.io/rabbidsincubator-docker-local/servicenowclientapisample

# pushes the image to the repository (Artifactory)
docker push devpro.jfrog.io/rabbidsincubator-docker-local/servicenowclientapisample

# pulls the image from the repository (Artifactory)
docker pull devpro.jfrog.io/rabbidsincubator-docker-local/servicenowclientapisample
```

### Troubleshoot issues

* Code generation can be sensitive, if case of stange behaviors:
  * Close and open again Visual Studio
  * Run `dotnet clean` and `dotnet build` from the command line (outside Visual Studio)
  * Manually delete all bin/ and obj/ folders from the root folder
  * Add a breakpoint in the ***Generator.cs file

  ```cs
    public void Execute(GeneratorExecutionContext context)
    {
  #if DEBUG
        if (!System.Diagnostics.Debugger.IsAttached)
        {
            System.Diagnostics.Debugger.Launch();
        }
  #endif

        var files = GetMappingFiles(context);
        files?.ToList().ForEach(x => GenerateCode(context, x));
    }
  ```
