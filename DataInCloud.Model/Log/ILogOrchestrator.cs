namespace DataInCloud.Model.Log;

public interface ILogOrchestrator
{
    Task<Log> AddLogAsync(Log log);
}