
using Azure;
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
    public async Task RegisterContainerIfNotExistAsync_ReturnsExistingUuid()
    {
        // Arrange
        var buildingId = "existingBuildingId";
        var existingUuid = "existing-uuid";
        var existingEntity = new TableEntity(PartitionKey, buildingId)
        {
            { ContainerField, existingUuid }
        };
        var response = Response.FromValue(existingEntity, new Mock<Response>().Object);
        
        _mockTableClient.Setup(x => x.GetEntityIfExistsAsync<TableEntity>(PartitionKey, buildingId, null, CancellationToken.None))
            .ReturnsAsync(response);

        // Act
        var result = await testUnit.RegisterContainerForBuildingAsync(buildingId);

        // Assert
        Assert.Equal(existingUuid, result);
    }
    
    [Fact]
    public async Task RegisterContainerIfNotExistAsync_AddsNewEntityAndReturnsUuid()
    {
        // Arrange
        var buildingId = "newBuildingId";
        TableEntity nullEntity = null;
        var response = Response.FromValue(nullEntity, new Mock<Response>().Object);
        
        _mockTableClient.Setup(x => 
                x.GetEntityIfExistsAsync<TableEntity>(PartitionKey, buildingId, null, CancellationToken.None))
            .ReturnsAsync(response);

        // Act
        var result = await testUnit.RegisterContainerForBuildingAsync(buildingId);

        // Assert
        Assert.NotNull(result);
        Guid uuid;
        Assert.True(Guid.TryParse(result, out uuid)); // Assert that result is a valid GUID
    }

    public void Dispose()
    {
        
    }
}