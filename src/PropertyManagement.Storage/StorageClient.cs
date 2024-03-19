using Azure.Storage.Blobs;

public class StorageClient
{
    BlobServiceClient _blobServiceClient;
    public StorageClient(string clientConnectionString)
    {
        _blobServiceClient = new(clientConnectionString);
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