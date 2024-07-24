namespace VerifyTests.NServiceBus;

public class RecordingHandlerContext :
    HandlerContext
{
    public static string DefaultMessageIdString = "c5e12a59-e424-44c8-8875-e7822e534966";
    public static Guid DefaultMessageId { get; } = new(DefaultMessageIdString);

    public  static string DefaultConversationIdString = "cd154c38-ab73-4a83-a66b-7404b9514080";
    public static Guid DefaultConversationId { get; } = new(DefaultConversationIdString);

    public static string DefaultCorrelationIdString = "87027093-7b35-4125-aa36-b5c15b9ea478";
    public static Guid DefaultCorrelationId { get; } = new(DefaultCorrelationIdString);

    public static string DefaultReplyToAddress = "ReplyToAddress";

    static Dictionary<string, string> defaultHeaders = new()
    {
        { Headers.MessageId, DefaultMessageIdString },
        { Headers.ConversationId, DefaultConversationIdString },
        { Headers.CorrelationId, DefaultCorrelationIdString },
        { Headers.ReplyToAddress, DefaultReplyToAddress },
        { "NServiceBus.TimeSent", "2000-01-01 13:00:00:000000 Z" }
    };

    public RecordingHandlerContext(Dictionary<string, string>? headers = null)
    {
        if (headers == null)
        {
            messageHeaders = defaultHeaders;
        }
        else
        {
            messageHeaders = new(headers);
            messageHeaders.TryAdd(Headers.MessageId, DefaultMessageIdString);
            messageHeaders.TryAdd(Headers.ConversationId, DefaultConversationIdString);
            messageHeaders.TryAdd(Headers.CorrelationId, DefaultCorrelationIdString);
            messageHeaders.TryAdd(Headers.ReplyToAddress, DefaultReplyToAddress);
            messageHeaders.TryAdd("NServiceBus.TimeSent", "2000-01-01 13:00:00:000000 Z");
        }
    }

    public IReadOnlyDictionary<string, string> MessageHeaders => messageHeaders;
    Dictionary<string, string> messageHeaders;

    public Cancel CancellationToken { get; } = Cancel.None;
    public ContextBag Extensions { get; } = new();

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

    public string MessageId { get; } = DefaultMessageIdString;
    public string ReplyToAddress { get; } = DefaultReplyToAddress;

    public virtual void DoNotContinueDispatchingCurrentMessageToHandlers() =>
        DoNotContinueDispatchingCurrentMessageToHandlersWasCalled = true;

    public virtual bool DoNotContinueDispatchingCurrentMessageToHandlersWasCalled { get; private set; }

    public ISynchronizedStorageSession SynchronizedStorageSession =>
        throw new NotImplementedException();
}