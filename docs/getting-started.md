# Getting Started

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
