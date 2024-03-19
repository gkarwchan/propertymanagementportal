@description('solution ID to make resource name unique')
param solutionId string

@description('region where the deployment will be')
param location string = resourceGroup().location

@description('storage account SKU')
param storageAccountSku string

var resourceName = take('brddocs${solutionId}', 24)

resource boardDocuments 'Microsoft.Storage/storageAccounts@2023-01-01' = {
  name: resourceName
  location: location
  sku: {
    name: storageAccountSku
  }
  kind: 'StorageV2'
  properties: {
    accessTier: 'Hot'
  }
}

resource boardDocumentsPolicies 'Microsoft.Storage/storageAccounts/managementPolicies@2023-01-01' = {
  name: 'default'
  parent:boardDocuments
  properties: {
    policy: {
      rules: [
        {
          enabled: true
          name: 'boardMeeting'
          type: 'Lifecycle'
          definition: {
            actions: {
              baseBlob: {
                tierToCool: { daysAfterCreationGreaterThan: 90 }
                tierToCold: { daysAfterCreationGreaterThan: 180 }
                tierToArchive: { daysAfterCreationGreaterThan: 550 }
                delete: {daysAfterCreationGreaterThan: 1100 }
              }
            }
            filters: {
              blobTypes: [ 'blockBlob' ]
              blobIndexMatch: [ 
                {
                  name : 'category'
                  op : '=='
                  value: 'board-meetings'
                }
              ]
            }
          }
        }
        {
          enabled: true
          name: 'legalDocuments'
          type: 'Lifecycle'
          definition: {
            actions: {
              baseBlob: {
                tierToCool: { daysAfterCreationGreaterThan: 60 }
                tierToCold: { daysAfterCreationGreaterThan: 150 }
                tierToArchive: { daysAfterCreationGreaterThan: 265 }
              }
            }
            filters: {
              blobTypes: [ 'blockBlob' ]
              blobIndexMatch: [
                {
                  name: 'category'
                  op: '=='
                  value: 'legal-documents'
                }
              ]
            }            
          }
        }
      ]
    }
  }

}

resource containerTableService 'Microsoft.Storage/storageAccounts/tableServices@2023-01-01' = {
  name: 'default'
  parent: boardDocuments
  properties: {
    cors: {
      corsRules: [
        {
          allowedHeaders: [  ]
          allowedMethods: [ 'GET', 'PATCH', 'POST', 'DELETE' ]
          allowedOrigins: [ '*' ]
          exposedHeaders: [ ]
          maxAgeInSeconds: 1000
        }
      ]
    }
  }
}

resource containerBuildingMap 'Microsoft.Storage/storageAccounts/tableServices/tables@2023-01-01' = {
  name: 'buildingContainerMap'
  parent: containerTableService
  properties: {
    
  }
}


output storageName string = resourceName
