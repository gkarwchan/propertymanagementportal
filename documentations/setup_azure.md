# Prepare Azure

Our DevOps's CD pipeline will deploy to a resource group. So to prepare Azure, it means:  
1. Create the resource group.
2. Create a Service Principal that has permissions to deploy to that resource group, and assign it to the CD pipeline.  

The full code is in file: [setup azure script](../scripts/setup_azure_dev.ps1)

### Create the resource group
We are going to create a resource group for our application.  

The resource group will have the pattern {environment}-PropertyManagement, where enviroment will be either: dev, test, production.  

```powershell
$ErrorActionPreference = "Stop"
New-AzResourceGroup -Name 'dev-PropertyManagement' -Location 'CentralUS' -Force -Tag @{'RG'='PM'}
```


# create a keyvault that will have all our secrets with template deployment enabled

```pwsh
# the 'gkar' in the following are to give a unique name to services. Feel free to change it.
New-AzKeyVault -Name "gkarproperty-dev-keyvault" -ResourceGroupName "dev-PropertyManagement" `
   -Location "CentralUS" -EnabledForTemplateDeployment

$servicePrincipal = New-AzADServicePrincipal -DisplayName pipeline-propertyManagement-dev
$serviceSecret = $servicePrincipal.PasswordCredentials.SecretText

Write-Output "Service Principal application ID: $($servicePrincipal.AppId)"
Write-Output "Service Principal ID: $($serviceSecret)"
Write-Output "Azure Tenant ID: $((Get-AzContext).Tenant.Id)"

New-AzRoleAssignment `
    -ApplicationId $servicePrincipal.AppId `
    -RoleDefinitionName Contributor `
    -Scope '/subscriptions/f0750bbe-ea75-4ae5-b24d-a92ca601da2c/resourceGroups/DevGroup' `
    -Description "The deployment pipeline for the company's website needs to be able to create resources within the resource group."

    $servicePrincipal = New-AzADServicePrincipal -DisplayName MyPipeline -Role Contributor -Scope '/subscriptions/f0750bbe-ea75-4ae5-b24d-a92ca601da2c/resourceGroups/ToyWebsite'

```
