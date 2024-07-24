namespace VerifyTests.NServiceBus;

public class RecordingMessageSession :
    IMessageSession
{
    public Task Send<T>(Action<T> messageConstructor, SendOptions sendOptions, Cancel cancel = default) =>
        throw new NotImplementedException();

    public IReadOnlyCollection<Published> Published => published;
    ConcurrentQueue<Published> published = new();

    public virtual Task Publish(object message, PublishOptions options, Cancel cancel = default)
    {
        var item = new Published(message, options);
        RecordingState.Publish(item);
        published.Enqueue(item);
        return Task.CompletedTask;
    }

    public Task Publish<T>(Action<T> messageConstructor, PublishOptions publishOptions, Cancel cancel = default) =>
        throw new NotImplementedException();

    public IReadOnlyCollection<Sent> Sent => sent;
    ConcurrentQueue<Sent> sent = new();

    public virtual Task Send(object message, SendOptions options, Cancel cancel = default)
    {
        var item = new Sent(message, options);
        RecordingState.Send(item);
        sent.Enqueue(item);
        return Task.CompletedTask;
    }

    public IReadOnlyCollection<Unsubscribed> Unsubscribed => unsubscribed;
    ConcurrentQueue<Unsubscribed> unsubscribed = new();

    public virtual Task Unsubscribe(Type eventType, UnsubscribeOptions options, Cancel cancel = default)
    {
        var item = new Unsubscribed(eventType, options);
        RecordingState.Unsubscribe(item);
        unsubscribed.Enqueue(item);
        return Task.CompletedTask;
    }

    public IReadOnlyCollection<Subscribed> Subscribed => subscribed;
    ConcurrentQueue<Subscribed> subscribed = new();

    public virtual Task Subscribe(Type eventType, SubscribeOptions options, Cancel cancel = default)
    {
        var item = new Subscribed(eventType, options);
        RecordingState.Subscribe(item);
        subscribed.Enqueue(item);
        return Task.CompletedTask;
    }
}