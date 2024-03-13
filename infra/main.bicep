@description('region to deploy the resources')
param location string = resourceGroup().location

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
