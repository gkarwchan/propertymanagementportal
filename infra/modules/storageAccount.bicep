@description('solution ID to make resource name unique')
param solutionId string

@description('region where the deployment will be')
param location string = resourceGroup().location

@description('storage account SKU')
param storageSku string


resource boardStorage 'Microsoft.Storage/storageAccounts@2023-01-01' = {
  name: 'brdStrg${solutionId}'
  location: location
  sku: {
    name: storageSku
  }
  kind: 'StorageV2'
}
