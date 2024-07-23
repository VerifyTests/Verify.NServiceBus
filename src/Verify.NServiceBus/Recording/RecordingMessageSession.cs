namespace VerifyTests.NServiceBus;

public class RecordingMessageSession :
    IMessageSession
{
    public Task Send<T>(Action<T> messageConstructor, SendOptions sendOptions, Cancel cancel = default) =>
        throw new NotImplementedException();

    public IReadOnlyCollection<Published> Published => published;
    ConcurrentQueue<Published> published = new();

    public Task Publish(object message, PublishOptions options, Cancel cancel = default)
    {
        RecordingState.Publish(message, options);
        published.Enqueue(new(message, options));
        return Task.CompletedTask;
    }

    public Task Publish<T>(Action<T> messageConstructor, PublishOptions publishOptions, Cancel cancel = default) =>
        throw new NotImplementedException();

    public IReadOnlyCollection<Sent> Sent => sent;
    ConcurrentQueue<Sent> sent = new();

    public Task Send(object message, SendOptions options, Cancel cancel = default)
    {
        RecordingState.Send(message, options);
        sent.Enqueue(new(message, options));
        return Task.CompletedTask;
    }

    public IReadOnlyCollection<Unsubscribed> Unsubscribed => unsubscribed;
    ConcurrentQueue<Unsubscribed> unsubscribed = new();

    public Task Unsubscribe(Type eventType, UnsubscribeOptions options, Cancel cancel = default)
    {
        RecordingState.Unsubscribe(eventType, options);
        unsubscribed.Enqueue(new(eventType, options));
        return Task.CompletedTask;
    }

    public IReadOnlyCollection<Subscribed> Subscribed => subscribed;
    ConcurrentQueue<Subscribed> subscribed = new();

    public Task Subscribe(Type eventType, SubscribeOptions options, Cancel cancel = default)
    {
        RecordingState.Subscribe(eventType, options);
        subscribed.Enqueue(new(eventType, options));
        return Task.CompletedTask;
    }
}