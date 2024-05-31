namespace DataInCloud.Platform.MessageBroker;

public interface ISubscriber
{
    Task StartReceiveAsync();
    Task StopReceiveAsync();
}