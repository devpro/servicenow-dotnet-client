# ServiceNow .NET Client

[![GitLab Pipeline Status](https://gitlab.com/rabbids-incubator/servicenow-dotnet-client/badges/main/pipeline.svg)](https://gitlab.com/rabbids-incubator/servicenow-dotnet-client/-/pipelines)

This is the codebase of .NET components (API & libraries) to integrate with [ServiceNow](https://www.servicenow.com/) from any system (Linux, MacOS, Windows).

## How to have a ServiceNow instance

### ServiceNow Developer program

* Register to [ServiceNow Developer program](https://developer.servicenow.com/dev.do) (free)

## How to get knowledge about ServiceNow

### ServiceNow resources

* [REST API Explorer](https://dev12345.service-now.com/nav_to.do?uri=%2F$restapi.do) (dev12345 to be replaced with your instance name)
* [Youtube channel](https://www.youtube.com/channel/UCdXorgCT87YlFRN9n8oJ7_A)
* [Developer community](https://community.servicenow.com/community?id=community_forum&sys_id=75291a2ddbd897c068c1fb651f9619f3)
* [Product documentation](https://docs.servicenow.com/)
  * [Now Platform App Engine > Web services > REST API (Rome)](https://docs.servicenow.com/bundle/rome-application-development/page/integrate/inbound-rest/concept/c_RESTAPI.html)

### Local documentation

* [Community](./docs/community.md)
* [ServiceNow CMDB](./docs/servicenow-cmdb.md)

## How to build the solution

### Requirements

* [.NET 6.0 SDK](https://dotnet.microsoft.com/download)

### Build

* Commands from the root folder of the repository

```bash
# restores .NET packages
dotnet restore

# builds the .NET solution
dotnet build
```

### Configuration

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

### Run

* Commands from the root folder of the repository

```bash
# runs the Console project
dotnet run --project src/ConsoleApp

# runs the Console dll with options
dotnet src/ConsoleApp/bin/Debug/net6.0/RabbidsIncubator.ServiceNowClient.ConsoleApp.dll -v
```

### Debug

#### Debug in Visual Studio 2022

* Add breakpoint(s)
* Click on `Debug` > `Start Debugging` (`F5`)

### Quality

* Commands from the root folder of the repository

```bash
# review and update source files from the rules defined in .editorconfig file
dotnet format
```
