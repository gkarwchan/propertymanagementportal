resource storageAccount 'Microsoft.Storage/storageAccounts@2022-09-01' = {
  name: 'gktoylaunchstorage'
  location: 'centralus'
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
  properties: {
    accessTier: 'Hot'
  }
}

resource appServicePlan 'Microsoft.Web/serverFarms@2022-03-01' = {
  name: 'gk-toy-product-launch-plan-starter'
  location: 'centralus'
  sku: {
    name: 'F1'
  }
}

resource appServiceApp 'Microsoft.Web/sites@2022-03-01' = {
  name: 'gk-toy-product-launch-68'
  location: 'centralus'
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
  }
}
