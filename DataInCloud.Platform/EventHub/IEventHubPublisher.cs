using DataInCloud.Platform.MessageBroker;

namespace DataInCloud.Platform.EventHub;

public interface IEventHubPulisher<in T> : IPublisher<T>
{

}