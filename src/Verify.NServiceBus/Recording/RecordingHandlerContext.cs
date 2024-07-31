namespace VerifyTests.NServiceBus;

public class RecordingHandlerContext :
    HandlerContext
{
    public static string DefaultMessageIdString = "c5e12a59-e424-44c8-8875-e7822e534966";
    public static Guid DefaultMessageId { get; } = new(DefaultMessageIdString);

    public static string DefaultConversationIdString = "cd154c38-ab73-4a83-a66b-7404b9514080";
    public static Guid DefaultConversationId { get; } = new(DefaultConversationIdString);

    public static string DefaultCorrelationIdString = "87027093-7b35-4125-aa36-b5c15b9ea478";
    public static Guid DefaultCorrelationId { get; } = new(DefaultCorrelationIdString);
    public static string DefaultOriginatingEndpoint = "DefaultOriginatingEndpoint";

    public static string DefaultReplyToAddress = "ReplyToAddress";

    static FrozenDictionary<string, string> defaultHeaders = FrozenDictionary
        .ToFrozenDictionary<string, string>(
        [
            new(global::NServiceBus.Headers.MessageId, DefaultMessageIdString),
            new(global::NServiceBus.Headers.ConversationId, DefaultConversationIdString),
            new(global::NServiceBus.Headers.CorrelationId, DefaultCorrelationIdString),
            new(global::NServiceBus.Headers.ReplyToAddress, DefaultReplyToAddress),
            new(global::NServiceBus.Headers.OriginatingEndpoint, DefaultOriginatingEndpoint),
            new("NServiceBus.TimeSent", "2000-01-01 13:00:00:000000 Z")
        ]);

    public static void AddSharedHeader(string key, string value)
    {
        var dictionary = new Dictionary<string, string>(defaultHeaders)
        {
            [key] = value
        };
        defaultHeaders = dictionary.ToFrozenDictionary();
    }

    public static void AddSharedHeaders(IEnumerable<KeyValuePair<string, string>> headers)
    {
        var dictionary = new Dictionary<string, string>(defaultHeaders);
        foreach (var pair in headers)
        {
            dictionary[pair.Key] = pair.Value;
        }
        defaultHeaders = dictionary.ToFrozenDictionary();
    }

    public RecordingHandlerContext(IEnumerable<KeyValuePair<string, string>>? headers = null)
    {
        if (headers == null)
        {
            return;
        }

        var messageHeaders = new Dictionary<string, string>(headers);
        messageHeaders.TryAdd(global::NServiceBus.Headers.MessageId, DefaultMessageIdString);
        messageHeaders.TryAdd(global::NServiceBus.Headers.ConversationId, DefaultConversationIdString);
        messageHeaders.TryAdd(global::NServiceBus.Headers.CorrelationId, DefaultCorrelationIdString);
        messageHeaders.TryAdd(global::NServiceBus.Headers.ReplyToAddress, DefaultReplyToAddress);
        messageHeaders.TryAdd(global::NServiceBus.Headers.OriginatingEndpoint, DefaultOriginatingEndpoint);
        messageHeaders.TryAdd("NServiceBus.TimeSent", "2000-01-01 13:00:00:000000 Z");
        writableHeaders = messageHeaders;
    }

    Dictionary<string, string>? writableHeaders;
    public Dictionary<string, string> Headers
    {
        get
        {
            if (writableHeaders == null)
            {
                writableHeaders = new(defaultHeaders);
            }
            return writableHeaders;
        }
        set => writableHeaders = value;
    }

    IReadOnlyDictionary<string, string> IMessageProcessingContext.MessageHeaders
    {
        get
        {
            if (writableHeaders == null)
            {
                return defaultHeaders;
            }
            return writableHeaders;
        }
    }

    public static ContextBag SharedContextBag { get; } = new();
    public Cancel CancellationToken { get; } = Cancel.None;
    public ContextBag Extensions { get; } = new(SharedContextBag);

    public IReadOnlyCollection<Sent> Sent => sent;
    ConcurrentQueue<Sent> sent = new();

    public virtual Task Send(object message, SendOptions options)
    {
        var item = new Sent(message, options);
        RecordingState.Send(item);
        sent.Enqueue(item);
        return Task.CompletedTask;
    }

    Task IPipelineContext.Send<T>(Action<T> messageConstructor, SendOptions options) =>
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

    Task IPipelineContext.Publish<T>(Action<T> messageConstructor, PublishOptions publishOptions) =>
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

    public string MessageId { get; } = DefaultMessageIdString;
    public string ReplyToAddress { get; } = DefaultReplyToAddress;

    public virtual void DoNotContinueDispatchingCurrentMessageToHandlers() =>
        DoNotContinueDispatchingCurrentMessageToHandlersWasCalled = true;

    public virtual bool DoNotContinueDispatchingCurrentMessageToHandlersWasCalled { get; private set; }

    ISynchronizedStorageSession HandlerContext.SynchronizedStorageSession =>
        throw new NotImplementedException();

    [EditorBrowsable(EditorBrowsableState.Never)]
    public override int GetHashCode() =>
        // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
        base.GetHashCode();

    [EditorBrowsable(EditorBrowsableState.Never)]
    public override string? ToString() =>
        base.ToString();

    [EditorBrowsable(EditorBrowsableState.Never)]
    public override bool Equals(object? obj) =>
        // ReSharper disable once BaseObjectEqualsIsObjectEquals
        base.Equals(obj);
}