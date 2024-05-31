namespace DataInCloud.Platform.MessageBroker;

public interface IMessageHandler<in T>
{
    Task HandleAsync(T message);
}