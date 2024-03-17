@description('region to deploy the resources')
param location string = resourceGroup().location

@description('storage Account SKU')
param storageAccountSku string

@description('environment name')
@allowed([
  'dev'
  'test'
  'pre'
  'prod'
])
param environmentName string

@description('app service plan instance count')
@minValue(1)
@maxValue(10)
param appServicePlanInstanceCount int
@description('app service sku')
param appServiceSku object

var solutionId = uniqueString(resourceGroup().id)

@description('deploy storage account')
module storageAccount 'modules/storageAccounts.bicep' = {
  name: 'storageAccount'
  params: {
    solutionId: solutionId
    location: location
    storageSku: storageAccountSku
  }
}


@description('App Service components')
module appService 'modules/appService.bicep'={
  name: 'appService'
  params: {
    location: location
    solutionId: solutionId
    appServicePlanSku: appServiceSku
    appServicePlanInstanceCount: appServicePlanInstanceCount
  }
}

output appServiceHostName string = appService.outputs.appServiceHostName
