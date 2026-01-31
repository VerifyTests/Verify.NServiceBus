namespace VerifyTests.NServiceBus;

public class RecordingIncomingPhysicalMessageContext :
    IIncomingPhysicalMessageContext
{
    public RecordingIncomingPhysicalMessageContext(IEnumerable<KeyValuePair<string, string>>? headers = null)
    {
        if (headers == null)
        {
            return;
        }

        writableHeaders = VerifyNServiceBus.MergeHeaders(headers);
    }

    Dictionary<string, string>? writableHeaders;

    public Dictionary<string, string> Headers
    {
        get => writableHeaders ??= new(VerifyNServiceBus.DefaultHeaders);
        set => writableHeaders = value;
    }

    IReadOnlyDictionary<string, string> IMessageProcessingContext.MessageHeaders =>
        writableHeaders == null ? VerifyNServiceBus.DefaultHeaders : writableHeaders;

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

    public string MessageId { get; } = VerifyNServiceBus.DefaultMessageIdString;
    public string ReplyToAddress { get; } = VerifyNServiceBus.DefaultReplyToAddress;

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

    public void UpdateMessage(ReadOnlyMemory<byte> body) =>
        UpdatedMessage = body;

    public ReadOnlyMemory<byte>? UpdatedMessage { get; set; }

#pragma warning disable CS8766 // Nullability doesn't match
    public IncomingMessage? Message { get; set; }
    public IServiceProvider? Builder { get; set; }
#pragma warning restore CS8766
}