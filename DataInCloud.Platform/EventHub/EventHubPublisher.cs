using System.Text.Json;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;

namespace DataInCloud.Platform.EventHub;

public class EventHubPublisher<TMessage> : IEventHubPulisher<TMessage>
{
    private bool _isInit;
    private readonly string _connectionString;
    private readonly string _hubName;
    protected EventHubProducerClient Producer;
    private readonly SemaphoreSlim _semaphoreSlim;

    public EventHubPublisher(string connectionString, string hubName)
    {
        _connectionString = connectionString;
        _hubName = hubName;
        _semaphoreSlim = new SemaphoreSlim(1, 1);
    }
    public async Task PublishAsync(TMessage model)
    {
        await InitAsync();

        var serialised = JsonSerializer.Serialize(model);

        var batch = new[]
        {
            new EventData(serialised)
        };

        await Producer.SendAsync(batch);
    }

    private async Task InitAsync()
    {
        await _semaphoreSlim.WaitAsync();
        try
        {
            if (!_isInit)
            {
                Producer = new EventHubProducerClient(_connectionString, _hubName);
                _isInit = true;
            }
        }
        finally
        {
            _semaphoreSlim.Release();
        }

    }
}
