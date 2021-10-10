# IdentityGuard
Identity and Access Management tools to enable a safer, more security environment.

# Requirements
* .NET 5 or higher

# Projects
## Application
|Project|Type|Purpose|
|---|---|---|
|IdentityGuard.Api|Azure Functions|HTTP API Layer for the application|
|IdentityGuard.Worker|Azure Functions|Background worker tasks|
|IdentityGuard.Blazor.UI|Blazor WebAssembly|User interface for the application|

## Libraries
|Project|Type|Purpose|
|---|---|---|
|IdentityGuard.Core|.NET Standard Class Library|Main library for non technology specific logic|
|IdentityGuard.Shared.Models|.NET Standard Class Library|Publicly accessible model definitions|
|IdentityGuard.Tests.Shared|.NET Standard Class Library|Unit test setup and mock/fake configuration to be shared between the API and Worker unit test projects|

## Tests
|Project|Type|Purpose|
|---|---|---|
|IdentityGuard.Api.Tests|xUnit Test Library|Unit tests for the API project|
|IdentityGuard.Worker.Tests|xUnit Test Library|Unit tests for the Worker project|

# Actions
This project uses GitHub Actions for builds and releases

|Action|Purpose|
|---|---|
|.github/workflows/ci-build.yml|Continuous Integration build definition, builds and runs tests|
|.github/workflows/deploy-dev.yml|Deploys components to the development environment|
|.github/workflows/deploy-prd.yml|Deploys all components to the production environment|

# Dependencies
To run this project successfully, there are the following dependencies:
|Resource|Purpose|
|---|---|
|[Cosmos DB](https://docs.microsoft.com/en-us/azure/cosmos-db/introduction)| Underlying data store for persisting data|
|[Azure Function](https://docs.microsoft.com/en-us/azure/azure-functions/)| Azure functions used to host the functions applications|
|[Azure KeyVault](https://docs.microsoft.com/en-us/azure/key-vault/general/overview)| All secrets for the application are stored here|
|[Managed Identity](https://docs.microsoft.com/en-us/azure/active-directory/managed-identities-azure-resources/)|Passwordless identifies used to access resources
|[Azure Active Directory](https://azure.microsoft.com/en-us/services/active-directory/)| Azure AD tenant used to authenticate users and the underlying services|
|[Microsoft Graph Api](https://developer.microsoft.com/en-us/graph)|Microsoft Graph API used to obtain data about users and other Azure resources|

# Development Environment Setup
Use the following steps to configure your development environment

1. Pull down the source code from GitHub
2. Build development infrastructure using the Infrastructure as Code definitions in the IdentityGuardInfra repository
3. Add a local.settings.json for the Api and Worker projects with the following content:
```
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "Release:ReleaseDate": "2021-09-15",
    "KeyVault:BaseUri": "https://idguard-dev.vault.azure.net/",
    "Cosmos:BaseUri": "https://idguard-dev.documents.azure.com:443/",
    "ASPNETCORE_ENVIRONMENT": "Development" 
  },
  "Host": {
    "LocalHttpPort": 7071,
    "CORS": "*"
  }
}
```