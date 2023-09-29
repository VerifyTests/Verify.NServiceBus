namespace VerifyTests.NServiceBus;

public class RecordingMessageContext :
    TestableMessageHandlerContext
{
    static AsyncLocal<State?> asyncLocal = new();

    public RecordingMessageContext() =>
        asyncLocal.Value = new();

    internal static IReadOnlyList<LogEntry>? Stop()
    {
        var state = asyncLocal.Value;
        asyncLocal.Value = null;
        return state?.Events;
    }

    class State
    {
        internal List<LogEntry> Events = new();

        public void Add(object message, ExtendableOptions options)
        {
            if (!options.HasValue())
            {
                Events.Add(new(message, null));
            }
            else
            {
                Events.Add(new(message, options));
            }
        }
    }

    public override Task Publish(object message, PublishOptions options)
    {
        asyncLocal.Value?.Add(message, options);
        return base.Publish(message, options);
    }

    public override Task Reply(object message, ReplyOptions options)
    {
        asyncLocal.Value?.Add(message, options);
        return base.Reply(message, options);
    }

    public override Task Send(object message, SendOptions options)
    {
        asyncLocal.Value?.Add(message, options);
        return base.Send(message, options);
    }

    internal class LogEntry(object message, ExtendableOptions? options)
    {
        public object Message { get; } = message;
        public ExtendableOptions? Options { get; } = options;
    }
}