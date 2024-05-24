
using System.Text;
using System.Text.Json;
using Azure.Storage.Blobs;
using DataInCloud.Model.Storage;

public class BlobStorage : IBlobStorage
{
    private readonly BlobContainerClient _client;
    public BlobStorage(BlobContainerClient client)
    {
        _client = client;
    }

    public async Task AppendToFile<T>(string fileName, T entity)
    {
        var exists = await FileExistsAsync(fileName);

        if (!exists) throw new FileNotFoundException();

        var blobClient = _client.GetBlobClient(fileName);

        var downloadInfo = await blobClient.DownloadAsync();

        using var reader = new StreamReader(downloadInfo.Value.Content, Encoding.UTF8);

        string existingContent = await reader.ReadToEndAsync();

        string serialisedEntity = JsonSerializer.Serialize(entity);
        string newContent = existingContent + serialisedEntity;

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(newContent));

        await blobClient.UploadAsync(stream, overwrite: true);
    }

    public async Task CreateFile<T>(string fileName, T entity)
    {
        var blobClient = _client.GetBlobClient(fileName);

        string contentJson = JsonSerializer.Serialize(entity);

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(contentJson));
        await blobClient.UploadAsync(stream, overwrite: true);
    }

    public async Task<bool> FileExistsAsync(string fileName)
    {
        return await _client.GetBlobClient(fileName).ExistsAsync();
    }

    public async Task<T> ReadFileAsync<T>(string fileName)
    {
        var exists = await FileExistsAsync(fileName);

        if (!exists) throw new FileNotFoundException();

        var downloaded = await _client.GetBlobClient(fileName).DownloadAsync();

        using var streamReader = new StreamReader(downloaded.Value.Content, Encoding.UTF8);
        var content = await streamReader.ReadToEndAsync();

        return JsonSerializer.Deserialize<T>(content);
    }
}