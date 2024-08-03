using Azure.Storage.Blobs;

namespace PropertyManagement.Storage;

public class BlobUtil
{

    private StorageRegisterer _storageRegisterer;
    public BlobUtil(string connectionString)
    {
        _storageRegisterer = new StorageRegisterer(connectionString);
    }
    public async Task UploadFile(string localFilePath, string buildingId)
    {
        string fileName = Path.GetFileName(localFilePath);
        var container = await _storageRegisterer.CreateBuildingContainerIfNotExistAsync(buildingId);
        BlobClient blobClient = container.GetBlobClient(fileName);

        await blobClient.UploadAsync(localFilePath, true);
    }

    public async Task UploadStream(string localFilePath, string buildingId)
    {
        string fileName = Path.GetFileName(localFilePath);
        var container = await _storageRegisterer.CreateBuildingContainerIfNotExistAsync(buildingId);
        BlobClient blobClient = container.GetBlobClient(fileName);
        var stream = File.OpenRead(localFilePath);

        await blobClient.UploadAsync(stream, true);
        stream.Close();
    }
}