using Azure;
using Azure.Data.Tables;
using Azure.Storage.Blobs;

namespace PropertyManagement.Storage;

public interface IStorageRegisterer
{
    Task<BlobContainerClient> CreateBuildingContainerIfNotExistAsync(string buildingId); 
}
public class StorageRegisterer(BlobServiceClient blobServiceClient, TableServiceClient tableServiceClient)
    : IStorageRegisterer
{
    private const string PartitionKey = "buildingId";
    private const string TableName = "buildingContainerMap";
    private const string ContainerField = "containerName";

    public StorageRegisterer(string clientConnectionString)  : 
        this(new BlobServiceClient(clientConnectionString), (new TableServiceClient(clientConnectionString)))
    {
    }


    public async Task<string> RegisterContainerForBuildingAsync(string buildingId)
    {
        var tableClient = tableServiceClient.GetTableClient(TableName);
        var buildingEntry = await tableClient.GetEntityIfExistsAsync<TableEntity>(PartitionKey, buildingId);
        if (buildingEntry?.Value != null) return buildingEntry.Value.GetString(ContainerField); 
        var uuid = (Guid.NewGuid()).ToString();
        
        var entry = new TableEntity(PartitionKey, buildingId)
        {
            {ContainerField , uuid }
        };
        await tableClient.AddEntityAsync(entry);
        return uuid;
    }

    private async Task<BlobContainerClient> CreateContainerIfNotExistClientAsync(string? containerName)
    {
        var container = blobServiceClient.GetBlobContainerClient(containerName);
        await container.CreateIfNotExistsAsync();
        return container;
    }    

    public async Task<BlobContainerClient> CreateBuildingContainerIfNotExistAsync(string buildingId)
    {
        var containerName = await RegisterContainerForBuildingAsync(buildingId);
        var container = await CreateContainerIfNotExistClientAsync(containerName);
        return container;
    }
}