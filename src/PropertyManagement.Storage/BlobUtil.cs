using Azure.Storage.Blobs;

public class BlobUtil
{

    private StorageClient _storageClient;
    public BlobUtil(string connectionString)
    {
        _storageClient = new StorageClient(connectionString);
    }
    public async Task UploadFile(string localFilePath, string buildingId)
    {
        string fileName = Path.GetFileName(localFilePath);
        var container = await _storageClient.CreateBuildingContainerIfNotExistAsync(buildingId);
        BlobClient blobClient = container.GetBlobClient(fileName);

        await blobClient.UploadAsync(localFilePath, true);
    }

}