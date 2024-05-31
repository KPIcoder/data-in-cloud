using DataInCloud.Model.Log;
using DataInCloud.Model.Storage;

namespace DataInCloud.Orchestrators.Log;

public class LogOrchestrator : ILogOrchestrator
{
    private readonly IBlobStorage _storage;

    public LogOrchestrator(IBlobStorage storage)
    {
        _storage = storage;
    }
    public async Task<Model.Log.Log> AddLogAsync(Model.Log.Log log)
    {

        var today = DateTime.Today.ToString("yyyy-MM-dd");
        var fileName = $"logs_{today}";
        var exists = await _storage.FileExistsAsync(fileName);

        if (!exists) await _storage.CreateFileAsync(fileName, log);
        else await _storage.AppendToFileAsync(fileName, log);

        return log;
    }
}
