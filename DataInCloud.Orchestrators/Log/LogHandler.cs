using DataInCloud.Model.Log;
using DataInCloud.Platform.MessageBroker;

namespace DataInCloud.Orchestrators.Log;

public class LogHandler : IMessageHandler<Model.Log.Log>
{

    ILogOrchestrator _loggerOrchestrator;
    public LogHandler(ILogOrchestrator loggerOrchestrator)
    {
        _loggerOrchestrator = loggerOrchestrator;
    }
    public async Task HandleAsync(Model.Log.Log message)
    {
        await _loggerOrchestrator.AddLogAsync(message);
    }
}
