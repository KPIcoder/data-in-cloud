namespace DataInCloud.Model.Storage;
public interface IBlobStorage
{
    Task<bool> FileExistsAsync(string fileName);

    Task<T> ReadFileAsync<T>(string fileName);

    Task AppendToFileAsync<T>(string fileName, T entity);

    Task CreateFileAsync<T>(string fileName, T entity);
}