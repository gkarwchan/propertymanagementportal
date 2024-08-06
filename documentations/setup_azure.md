# Setup Azure

The build pipeline in this project has all capabilities to build and deploy the full application.  
It has all CI/CD to deploy the application.  
Only two things are missing, that we have to setup before running the pipeline:  

1. Creating the resource grup that will be the target of the deployment, and all provisioned Azure services deployed through the CD are going to be added to that resource group.
2. Create the Service Principal that we will be referenced in Azure DevOps project, which has the permissions to deploy to that resource group.  

Both setup steps are added to the powershell script: [setup_azure_dev.ps1](../scripts/setup_azure_dev.ps1).

## Pre-requirement:
Before even running the setup script, we need to make sure that we have Powershell installed, and [Azure Powershell](https://learn.microsoft.com/en-us/powershell/azure/install-azure-powershell?view=azps-12.1.0) installed, and you already logged in to get Azure Powershell context.  
To make sure you already logged in, run the following:  

```pwsh
Get-AzContext # to get the current active session
Get-AzContext -ListAvailable # to get all available
```
If you are not logged in, run the following command: 

```pwsh
Connect-AzAccount
```
If you have multiple subscriptions/tenats, use the following command to chose the correct one

```pwsh
# to set the active context run one of the following:
Set-AzContext -Context (Get-AzContext -Name <put here your desired context name>) 
Set-AzContext -Name <put here your desired context name>
```
You should see your active 

### Create the resource group
We are going to create a resource group for our application.  

The resource group will have the pattern {environment}-PropertyManagement, where enviroment will be either: dev, test, production.  

```powershell
$ErrorActionPreference = "Stop"
New-AzResourceGroup -Name 'dev-PropertyManagement' -Location 'CentralUS' -Force -Tag @{'RG'='PM'}
```


# create a keyvault that will have all our secrets with template deployment enabled
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
    
# Get-AzKeyVaultSecret -VaultName "dev-keyvault-gkar" -Name "sqlPassword" -AsPlainText

# $keyVaultName = 'YOUR-KEY-VAULT-NAME'
# $login = Read-Host "Enter the login name" -AsSecureString
# $password = Read-Host "Enter the password" -AsSecureString

# New-AzKeyVault -VaultName $keyVaultName -Location westus3 -EnabledForTemplateDeployment
# Set-AzKeyVaultSecret -VaultName $keyVaultName -Name 'sqlServerAdministratorLogin' -SecretValue $login
# Set-AzKeyVaultSecret -VaultName $keyVaultName -Name 'sqlServerAdministratorPassword' -SecretValue $password
```