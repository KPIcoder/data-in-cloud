namespace DataInCloud.Platform.MessageBroker;

public interface IReceiver<TMessage>
{
    Task<TMessage> HandleMessageAsync();
}