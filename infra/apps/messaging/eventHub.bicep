@description('solution ID to make resource name unique')
param solutionId string

@description('region where the deployment will be')
param location string = resourceGroup().location

@description('event hub account SKU')
param eventHubSku string

var resourceName = take('eventhub${solutionId}', 24)

var eventHubNamespaceName = '${resourceName}ns'
var eventHubName = resourceName

resource eventHubNamespace 'Microsoft.EventHub/namespaces@2021-11-01' = {
  name: eventHubNamespaceName
  location: location
  sku: {
    name: eventHubSku
    tier: eventHubSku
    capacity: 1
  }
  properties: {
    isAutoInflateEnabled: false
    maximumThroughputUnits: 0
  }
}

resource eventHub 'Microsoft.EventHub/namespaces/eventhubs@2021-11-01' = {
  parent: eventHubNamespace
  name: eventHubName
  properties: {
    messageRetentionInDays: 7
    partitionCount: 1
  }
}


output eventHubName string = resourceName
