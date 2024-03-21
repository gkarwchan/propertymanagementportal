using Azure.Data.Tables;
using Azure.Storage.Blobs;

public interface IStorageClient
{
    Task<BlobContainerClient> CreateBuildingContainerIfNotExistAsync(string buildingId); 
}
public class StorageClient : IStorageClient
{
    const string PARTITION_KEY = "buildingId";
    const string TABLE_NAME = "buildingContainerMap";
    const string CONTAINER_FIELD = "containerName";
    
    BlobServiceClient _blobServiceClient;
    TableClient _tableClient;
    public StorageClient(string clientConnectionString)
    {
        _blobServiceClient = new(clientConnectionString);
        var _tableServiceClient = new TableServiceClient(clientConnectionString);
        _tableClient = _tableServiceClient.GetTableClient("buildingContainerMap");
    }

    private async Task<string> RegisterContainerIfNotExistAsync(string buildingId)
    {
        var entity = await _tableClient.GetEntityIfExistsAsync<TableEntity>(PARTITION_KEY, buildingId);
        if (entity.HasValue) return entity.Value.GetString(CONTAINER_FIELD);
        var uuid = (Guid.NewGuid()).ToString();
        
        var entry = new TableEntity(PARTITION_KEY, buildingId)
        {
           {CONTAINER_FIELD , uuid }
        };
        await _tableClient.AddEntityAsync(entry);
        return uuid;
    }

    private async Task<BlobContainerClient> CreateContainerIfNotExistClientAsync(string containerName)
    {
        var container = _blobServiceClient.GetBlobContainerClient(containerName);
        await container.CreateIfNotExistsAsync();
        return container;
    }    

    public async Task<BlobContainerClient> CreateBuildingContainerIfNotExistAsync(string buildingId)
    {
        var containerName = await RegisterContainerIfNotExistAsync(buildingId);
        var container = await CreateContainerIfNotExistClientAsync(containerName);
        return container;
    }
}