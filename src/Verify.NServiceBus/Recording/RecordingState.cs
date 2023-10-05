class RecordingState
{
    static AsyncLocal<State?> asyncLocal = new();
    readonly State state;

    public RecordingState() =>
        asyncLocal.Value = state =new();

    internal static List<KeyValuePair<string, LogEntry>>? Stop()
    {
        var state = asyncLocal.Value;
        asyncLocal.Value = null;
        return state?.Events;
    }

    class State
    {
        internal List<KeyValuePair<string, LogEntry>> Events = new();

        public void Add(string action, object? message, ExtendableOptions options, Type? eventType)
        {
            LogEntry logEntry;
            if (!options.HasValue())
            {
                logEntry = new(message, null, eventType);
            }
            else
            {
                logEntry = new(message, options, eventType);
            }
            Events.Add(new(action, logEntry));
        }
    }

    public void Publish(object message, PublishOptions options) =>
        state.Add("Publish", message, options, null);

    public void Reply(object message, ReplyOptions options) =>
        state.Add("Reply", message, options, null);

    public void Send(object message, SendOptions options) =>
        state.Add("Send", message, options, null);



    public void Unsubscribe(Type eventType, UnsubscribeOptions options) =>
        state.Add("Unsubscribe", null, options, eventType);

    public void Subscribe(Type eventType, SubscribeOptions options) =>
        state.Add("Subscribe", null, options, eventType);
}