namespace DataInCloud.Platform.MessageBroker;

public interface IPublisher<in T>
{
    Task PublishAsync(T model);
}