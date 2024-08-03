
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Moq;

namespace PropertyManagement.Storage.Tests;

public class StorageRegistererTests : IDisposable
{
    private readonly Mock<BlobServiceClient> _mockBlobServiceClient;
    private readonly Mock<TableServiceClient> _mockTableServiceClient;
    private readonly Mock<TableClient> _mockTableClient;
    private const string PartitionKey = "buildingId";
    private const string TableName = "buildingContainerMap";
    private const string ContainerField = "containerName";
    private StorageRegisterer testUnit;

    public StorageRegistererTests()
    {
        _mockBlobServiceClient = new Mock<BlobServiceClient>();
        _mockTableServiceClient = new Mock<TableServiceClient>();
        _mockTableClient = new Mock<TableClient>();
        _mockTableServiceClient.Setup(x => x.GetTableClient(TableName))
            .Returns(_mockTableClient.Object);
        testUnit = new StorageRegisterer(_mockBlobServiceClient.Object, _mockTableServiceClient.Object);
    }

    [Fact]
    public void WhenShouldTest()
    {
    }

    public void Dispose()
    {
        
    }
}