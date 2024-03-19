@description('region to deploy the resources')
param location string = resourceGroup().location

@description('storage Account SKU')
param storageAccountSku string

@description('solution ID to make resource name unique')
param solutionId string


@description('deploy storage account')
module storageAccount 'modules/storageAccounts.bicep' = {
  name: 'storageAccount'
  params: {
    solutionId: solutionId
    location: location
    storageAccountSku: storageAccountSku
  }
}

