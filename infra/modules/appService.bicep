@description('solution ID to make resource name unique')
param solutionId string

@description('region where the deployment will be')
param location string = resourceGroup().location


@description('app server plan SKU')
param appServicePlanSku object
@description('instance count for the appservice')
param appServicePlanInstanceCount int


resource appServicePlan 'Microsoft.Web/serverFarms@2023-01-01' = {
  name: 'webfarm-${solutionId}'
  location: location
  sku: {
    name: appServicePlanSku.name
    tier: appServicePlanSku.tier
    capacity: appServicePlanInstanceCount
  }
}

resource appServiceApp 'Microsoft.Web/sites@2023-01-01' = {
  name: 'webapp-${solutionId}'
  location: location
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
  }
}

output appServiceHostName string = appServiceApp.properties.defaultHostName
