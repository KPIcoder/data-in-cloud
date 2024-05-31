
using System.Text.Json;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;
using DataInCloud.Platform.MessageBroker;
using Microsoft.Extensions.DependencyInjection;

namespace DataInCloud.Platform.EventHub;
public class EventHubSubscriber<TMessage> : IEventHubSubscriber, IAsyncDisposable, IDisposable
{
    private readonly SemaphoreSlim _semaphoreSlim;
    private bool _isInit;

    protected EventProcessorClient Processor;

    private readonly string _connectionString;
    private readonly string _hubName;
    private readonly string _blobConnectionString;
    private readonly string _blobContainer;
    private readonly IServiceProvider _serviceProvider;

    public EventHubSubscriber(
        string connectionString,
        string hubName,
        string blobConnectionString,
        string blobContainer,
        IServiceProvider serviceProvider
    )
    {
        _connectionString = connectionString;
        _hubName = hubName;
        _blobConnectionString = blobConnectionString;
        _blobContainer = blobContainer;
        _serviceProvider = serviceProvider;
        _semaphoreSlim = new SemaphoreSlim(1, 1);
    }

    public async Task InitialiseAsync()
    {
        if (!_isInit)
        {
            await _semaphoreSlim.WaitAsync();
            try
            {
                if (!_isInit)
                {
                    var blobClient = new BlobContainerClient(_blobConnectionString, _blobContainer);
                    Processor = CreateProcessor(blobClient);

                    _isInit = true;
                }
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }
    }

    private EventProcessorClient CreateProcessor(BlobContainerClient blobClient)
    {
        var consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;

        return new EventProcessorClient(blobClient, consumerGroup, _connectionString, _hubName);
    }

    public async Task StartReceiveAsync()
    {
        Processor.ProcessEventAsync += ProcessorOnProcessEventAsync;
        Processor.ProcessErrorAsync += ProcessorOnProcessErrorAsync;

        await Processor.StartProcessingAsync();
    }

    public async Task StopReceiveAsync()
    {
        await Processor.StopProcessingAsync();

        Processor.ProcessEventAsync -= ProcessorOnProcessEventAsync;
        Processor.ProcessErrorAsync -= ProcessorOnProcessErrorAsync;
    }

    protected virtual async Task ProcessorOnProcessEventAsync(ProcessEventArgs args)
    {
        var message = await DeserialiseMessageAsync(args.Data.EventBody.ToStream());
        using var scope = _serviceProvider.CreateScope();
        var handler = scope.ServiceProvider.GetService<IMessageHandler<TMessage>>();

        await handler.HandleAsync(message);
    }

    protected virtual Task ProcessorOnProcessErrorAsync(ProcessErrorEventArgs args)
    {
        return Task.CompletedTask;
    }

    protected async Task<TMessage> DeserialiseMessageAsync(Stream message)
    {
        return await JsonSerializer.DeserializeAsync<TMessage>(message);
    }

    public async ValueTask DisposeAsync()
    {
        if (Processor != null)
        {
            await Processor.StopProcessingAsync();
        }
    }

    public void Dispose()
    {
        Processor?.StopProcessingAsync().GetAwaiter().GetResult();
    }
}