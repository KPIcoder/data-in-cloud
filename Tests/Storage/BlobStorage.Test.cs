using Azure.Storage.Blobs;
using DataInCloud.Model.Meal;
using Moq;

public class BlobStorageTests
{
    private readonly Mock<BlobContainerClient> _mockContainerClient;
    private readonly Mock<BlobClient> _mockBlobClient;
    private readonly BlobStorage _blobStorage;

    public BlobStorageTests()
    {
        _mockContainerClient = new Mock<BlobContainerClient>();
        _mockBlobClient = new Mock<BlobClient>();

        _mockContainerClient.Setup(c => c.GetBlobClient(It.IsAny<string>())).Returns(_mockBlobClient.Object);

        _blobStorage = new BlobStorage(_mockContainerClient.Object);
    }

    [Fact]
    public async Task CreateFileAsync_ShouldCreateFile()
    {
        // Arrange
        var fileName = "test.json";
        var entity = new Meal { Id = 1, Name = "Pizza", Price = 10, IsAvailable = true };

        // Act
        await _blobStorage.CreateFile(fileName, entity);

        // Assert
        _mockBlobClient.Verify(c => c.UploadAsync(It.IsAny<Stream>(), true, default), Times.Once);
    }


    [Fact]
    public async Task FileExistsAsync_ShouldReturnTrueIfFileExists()
    {
        // Arrange
        var fileName = "test.json";
        _mockBlobClient.Setup(c => c.ExistsAsync(default)).ReturnsAsync(Azure.Response.FromValue(true, Mock.Of<Azure.Response>()));

        // Act
        var result = await _blobStorage.FileExistsAsync(fileName);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task AppendToFileAsync_ShouldThrowFileNotFoundException_WhenFileDoesNotExist()
    {
        // Arrange
        var fileName = "nonexistent.json";
        var entity = new Meal { Id = 2, Name = "Burger", Price = 15, IsAvailable = false };

        _mockBlobClient.Setup(c => c.ExistsAsync(default)).ReturnsAsync(Azure.Response.FromValue(false, Mock.Of<Azure.Response>()));

        // Act & Assert
        await Assert.ThrowsAsync<FileNotFoundException>(() => _blobStorage.AppendToFile(fileName, entity));
    }

    [Fact]
    public async Task ReadFileAsync_ShouldThrowFileNotFoundException_WhenFileDoesNotExist()
    {
        // Arrange
        var fileName = "nonexistent.json";

        _mockBlobClient.Setup(c => c.ExistsAsync(default)).ReturnsAsync(Azure.Response.FromValue(false, Mock.Of<Azure.Response>()));

        // Act & Assert
        await Assert.ThrowsAsync<FileNotFoundException>(() => _blobStorage.ReadFileAsync<Meal>(fileName));
    }
}

