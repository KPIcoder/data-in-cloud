namespace DataInCloud.Platform.MessageBroker;

public interface IMessageProvider
{
    Task InitialiseAsync();
}