using Azure.Data.Tables;
using Azure.Storage.Blobs;

public class StorageClient
{
    BlobServiceClient _blobServiceClient;
    TableServiceClient _tableServiceClient;
    
    public StorageClient(string clientConnectionString)
    {
        _blobServiceClient = new(clientConnectionString);
        _tableServiceClient = new TableServiceClient(clientConnectionString);
    }

    public async Task CreateEntry() 
    {
        _tableServiceClient.GetTableClient("dab");
    }

    public async Task<BlobContainerClient> CreateBloblContainerClientAsync(string containerName)
    {
        var container = _blobServiceClient.GetBlobContainerClient(containerName);
        await container.CreateIfNotExistsAsync();
        return container;
    }
    public async Task UploadFile(string containerName, string fileName)
    {
        var container = _blobServiceClient.GetBlobContainerClient(containerName);
        var blob = container.GetBlobClient("shareholders-4.pdf");

        await blob.UploadAsync("d:\\code\\shareholders-4.pdf", true);
    }
}