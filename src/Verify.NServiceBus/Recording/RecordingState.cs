class RecordingState
{
    static AsyncLocal<State?> asyncLocal = new();
    readonly State state;

    public RecordingState() =>
        asyncLocal.Value = state =new();

    internal static List<KeyValuePair<string, RecordedMessage>>? Stop()
    {
        var state = asyncLocal.Value;
        asyncLocal.Value = null;
        return state?.Messages;
    }

    class State
    {
        internal List<KeyValuePair<string, RecordedMessage>> Messages = new();

        public void Add(string action, object? message, ExtendableOptions options, Type? eventType)
        {
            RecordedMessage recordedMessage;
            if (!options.HasValue())
            {
                recordedMessage = new(message, null, eventType);
            }
            else
            {
                recordedMessage = new(message, options, eventType);
            }
            Messages.Add(new(action, recordedMessage));
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