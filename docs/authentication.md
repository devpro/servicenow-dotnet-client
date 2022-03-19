# Authentication

## How to secure an ASP.NET application with Azure AD

* In [Azure Portal](https://portal.azure.com/), in "Azure Active Directory > Application registrations",
select "New registration"
  * Only the same is mandatory
  * Once created, the application is displayed
  * Save the values of "Application (client) ID", "Directory (tenant) ID"
* Update the application
  * "Manifest": manually edit the content (`accessTokenAcceptedVersion` and `allowPublicClient` are null by default)

  ```json
  {
    "accessTokenAcceptedVersion": 2,
    "allowPublicClient": true,
  }
  ```

  * "Certificates & secrets": in "Client Secrets", add a new secret and save the secret value
  * "Api permissions": do "Grant admin consent for Default Directory" (Microsoft Graph > User.Read has been added by default)
  * "Expose an API": set the application ID URI, "api://<client_id>" is the default and correct choice
  * "Expose an API": add a scope, for example "access_as_user" with "Admins and users" for the consent option
