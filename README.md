# ServiceNow .NET Client

This is the .NET codebase that will provide client components (API & libraries) to interact with [ServiceNow](https://www.servicenow.com/), that will work on any system (Linux, MacOS, Windows) and are free to use.

## How to have a ServiceNow instance

### ServiceNow Developer program

* Register to [ServiceNow Developer program](https://developer.servicenow.com/dev.do)
* Open the [REST API Explorer](https://devXXXXX.service-now.com/nav_to.do?uri=%2F$restapi.do) (XXXX to be replaced in the link)

## How to get knowledge about ServiceNow

### ServiceNow resources

* [Youtube channel](https://www.youtube.com/channel/UCdXorgCT87YlFRN9n8oJ7_A)
* [Developer community](https://community.servicenow.com/community?id=community_forum&sys_id=75291a2ddbd897c068c1fb651f9619f3)
* [Product documentation](https://docs.servicenow.com/)
  * [Now Platform App Engine > Web services > REST API (Rome)](https://docs.servicenow.com/bundle/rome-application-development/page/integrate/inbound-rest/concept/c_RESTAPI.html)

## How to build the solution

### Build requirements

* [.NET 6.0 SDK](https://dotnet.microsoft.com/download)

### Build steps

* Commands from the root folder of the repository

```bash
# restores packages
dotnet restore

# builds the solution
dotnet build
```

### Run steps

* Commands from the root folder of the repository

```bash
# runs the project
dotnet run --project src/ConsoleApp

# runs the dll with options
dotnet src/ConsoleApp/bin/Debug/net6.0/RabbidsIncubator.ServiceNowClient.ConsoleApp.dll -v
```

### Debug steps

#### Visual Studio 2019/2022

* Add breakpoint(s)
* Click on `Debug` > `Start Debugging` (`F5`)

### Quality steps


* Commands from the root folder of the repository

```bash
# review and update source files from the rules defined in .editorconfig file
dotnet format
```
