using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using DataInCloud.Platform.MessageBroker;


namespace DataInCloud.Platform.EventHub;

public static class EventHubInjectionExtension
{
    public static void AddEventHubSubscriber<TMessage, THandler>(this ContainerBuilder builder,
        string connectionString,
        string hubName,
        string blobConnectionString,
        string blobName)
    {
        builder.RegisterType<THandler>().As<IMessageHandler<TMessage>>();
        builder.RegisterType<EventHubSubscriber<TMessage>>()
            .WithParameter("connectionString", connectionString)
            .WithParameter("hubName", hubName)
            .WithParameter("blobConnectionString", blobConnectionString)
            .WithParameter("blobContainer", blobName)
            .WithParameter(
                (p, c) => p.ParameterType == typeof(IServiceProvider),
                (p, c) => c.Resolve<IServiceProvider>()
            )
            .AsImplementedInterfaces()
            .SingleInstance();
    }

    public static void AddEventHubPublisher<TMessage>(this ContainerBuilder builder,
        string connectionString,
        string hubName)
    {
        builder.RegisterType<EventHubPublisher<TMessage>>()
            .WithParameter("connectionString", connectionString)
            .WithParameter("hubName", hubName)
            .AsImplementedInterfaces()
            .SingleInstance();
    }

    public static async Task UseEventHubAsync(this IApplicationBuilder builder)
    {
        var subscribers = builder.ApplicationServices.GetServices<IEventHubSubscriber>();
        foreach (var subscriber in subscribers)
        {
            await subscriber.InitialiseAsync();
            await subscriber.StartReceiveAsync();
        }
    }
}