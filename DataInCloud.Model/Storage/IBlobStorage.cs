namespace DataInCloud.Model.Storage;
public interface IBlobStorage
{
    Task<bool> FileExistsAsync(string fileName);

    Task<T> ReadFileAsync<T>(string fileName);

    Task AppendToFile<T>(string fileName, T entity);

    Task CreateFile<T>(string fileName, T entity);
}