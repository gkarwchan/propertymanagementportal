@description('solution ID to make resource name unique')
param solutionId string

@description('region where the deployment will be')
param location string = resourceGroup().location

@description('storage account SKU')
param storageAccountSku string

resource boardDocuments 'Microsoft.Storage/storageAccounts@2023-01-01' = {
  name: take('brdDocs${solutionId}', 24)
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
  name: 'boardDocumentPolicies'
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
                tierToCold: { daysAfterLastTierChangeGreaterThan: 90 }
                tierToArchive: { daysAfterCreationGreaterThan: 550 }
                delete: {daysAfterLastTierChangeGreaterThan: 550 }
              }
            }
            filters: {
              blobTypes: [ 'blockBlob' ]
              prefixMatch: [ 'boardMeetings/' ]
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
                tierToCold: { daysAfterLastTierChangeGreaterThan: 90 }
                tierToArchive: { daysAfterCreationGreaterThan: 265 }
              }
            }
            filters: {
              blobTypes: [ 'blockBlob' ]
              prefixMatch: [ 'legalDocuments/' ]
            }            
          }
        }
      ]
    }
  }

}
