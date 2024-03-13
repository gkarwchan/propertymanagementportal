# Setup Azure

We are going to create a resource group for our application.  
The resource group will have the pattern {environment}-PropertyManagement, where enviroment will be either: dev, test, production.  
Then we will create a `Service Principal` that will be used by Azure Pipeline to deploy our application to Azure.  

The code is in [setup_azure_dev](../infra/setup_azure_dev.ps1).  

```powershell
$ErrorActionPreference = "Stop"
New-AzResourceGroup -Name 'dev-PropertyManagement' -Location 'CentralUS' -Force -Tag @{'RG'='PM'}

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