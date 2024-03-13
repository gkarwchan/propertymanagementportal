$ErrorActionPreference = "Stop"
# Create a resource group
New-AzResourceGroup -Name 'dev-PropertyManagement' -Location 'CentralUS' -Force -Tag @{'RG'='PM'}
$resourceGroupId = (Get-AzResourceGroup -Name dev-PropertyManagement).ResourceId

# create a keyvault that will have all our secrets with template deployment enabled
New-AzKeyVault -Name "gkar-property-dev-vault" -ResourceGroupName "dev-PropertyManagement" `
   -Location "CentralUS" -EnabledForTemplateDeployment

# Create a Service Principal that will be used by Azure DevOps
$servicePrincipal = New-AzADServicePrincipal -DisplayName pipeline-propertyManagement-dev
$serviceSecret = $servicePrincipal.PasswordCredentials.SecretText

Write-Output "Service Principal application ID: $($servicePrincipal.AppId)"
Write-Output "Service Principal ID: $($serviceSecret)"
Write-Output "Azure Tenant ID: $((Get-AzContext).Tenant.Id)"

New-AzRoleAssignment `
    -ApplicationId $servicePrincipal.AppId `
    -RoleDefinitionName Contributor `
    -Scope $resourceGroupId `
    -Description "The deployment pipeline for the company's website needs to be able to create resources within the resource group."
