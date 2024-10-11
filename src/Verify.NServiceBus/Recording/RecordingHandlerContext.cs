namespace VerifyTests.NServiceBus;

public class RecordingHandlerContext :
    HandlerContext
{
    static FrozenDictionary<string, string> defaultHeaders = FrozenDictionary
        .ToFrozenDictionary<string, string>(
        [
            new(Headers.MessageId, VerifyNServiceBus.DefaultMessageIdString),
            new(Headers.ConversationId, VerifyNServiceBus.DefaultConversationIdString),
            new(Headers.CorrelationId, VerifyNServiceBus.DefaultCorrelationIdString),
            new(Headers.ReplyToAddress, VerifyNServiceBus.DefaultReplyToAddress),
            new("NServiceBus.TimeSent", "2000-01-01 13:00:00:000000 Z")
        ]);

    public RecordingHandlerContext(IEnumerable<KeyValuePair<string, string>>? headers = null)
    {
        if (headers == null)
        {
            MessageHeaders = defaultHeaders;
            return;
        }

        var messageHeaders = new Dictionary<string, string>(headers);
        messageHeaders.TryAdd(Headers.MessageId, VerifyNServiceBus.DefaultMessageIdString);
        messageHeaders.TryAdd(Headers.ConversationId, VerifyNServiceBus.DefaultConversationIdString);
        messageHeaders.TryAdd(Headers.CorrelationId, VerifyNServiceBus.DefaultCorrelationIdString);
        messageHeaders.TryAdd(Headers.ReplyToAddress, VerifyNServiceBus.DefaultReplyToAddress);
        messageHeaders.TryAdd("NServiceBus.TimeSent", "2000-01-01 13:00:00:000000 Z");
        MessageHeaders = messageHeaders;
    }

    public IReadOnlyDictionary<string, string> MessageHeaders { get; }

    public Cancel CancellationToken { get; } = Cancel.None;
    public ContextBag Extensions { get; } = new(SharedContextBag);
    public static ContextBag SharedContextBag { get; } = new();

    public IReadOnlyCollection<Sent> Sent => sent;
    ConcurrentQueue<Sent> sent = new();

    public virtual Task Send(object message, SendOptions options)
    {
        var item = new Sent(message, options);
        RecordingState.Send(item);
        sent.Enqueue(item);
        return Task.CompletedTask;
    }

    public Task Send<T>(Action<T> messageConstructor, SendOptions options) =>
        throw new NotImplementedException();

    public IReadOnlyCollection<Published> Published => published;
    ConcurrentQueue<Published> published = new();

    public virtual Task Publish(object message, PublishOptions options)
    {
        var item = new Published(message, options);
        RecordingState.Publish(item);
        published.Enqueue(item);
        return Task.CompletedTask;
    }

    public Task Publish<T>(Action<T> messageConstructor, PublishOptions publishOptions) =>
        throw new NotImplementedException();

    public IReadOnlyCollection<Replied> Replied => replied;

    ConcurrentQueue<Replied> replied = new();

    public virtual Task Reply(object message, ReplyOptions options)
    {
        var item = new Replied(message, options);
        RecordingState.Reply(item);
        replied.Enqueue(item);
        return Task.CompletedTask;
    }

    public Task Reply<T>(Action<T> messageConstructor, ReplyOptions options) =>
        throw new NotImplementedException();

    public IReadOnlyCollection<string> Forwarded => forwarded;
    ConcurrentQueue<string> forwarded = new();

    public virtual Task ForwardCurrentMessageTo(string destination)
    {
        forwarded.Enqueue(destination);
        return Task.CompletedTask;
    }

    public string MessageId { get; } = VerifyNServiceBus.DefaultMessageIdString;
    public string ReplyToAddress { get; } = VerifyNServiceBus.DefaultReplyToAddress;

    public virtual void DoNotContinueDispatchingCurrentMessageToHandlers() =>
        DoNotContinueDispatchingCurrentMessageToHandlersWasCalled = true;

    public virtual bool DoNotContinueDispatchingCurrentMessageToHandlersWasCalled { get; private set; }

    public ISynchronizedStorageSession SynchronizedStorageSession =>
        throw new NotImplementedException();
}