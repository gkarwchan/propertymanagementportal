using Azure.Storage.Blobs;

public class StorageClient
{
    BlobServiceClient _blobServiceClient;
    public StorageClient(string clientConnectionString)
    {
        _blobServiceClient = new(clientConnectionString);
    }

    public BlobContainerClient GetBloblContainerClient(string containerName)
    {
        return _blobServiceClient.GetBlobContainerClient(containerName);
    }

    public async Task<BlobContainerClient> CreateContainerAsync(string containerName)
    {
        return await _blobServiceClient.CreateBlobContainerAsync(containerName);
    }
}