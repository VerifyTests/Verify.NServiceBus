static class RecordingState
{
    public static void Add(string action, object? message, ExtendableOptions options, Type? eventType)
    {
        RecordedMessage recorded;
        if (options.HasValue())
        {
            recorded = new(message, options, eventType);
        }
        else
        {
            recorded = new(message, null, eventType);
        }

        Recording.TryAdd("message", new KeyValuePair<string, RecordedMessage>(action, recorded));
    }

    public static void Publish(object message, PublishOptions options) =>
        Add("Publish", message, options, null);

    public static void Reply(object message, ReplyOptions options) =>
        Add("Reply", message, options, null);

    public static void Send(object message, SendOptions options) =>
        Add("Send", message, options, null);

    public static void Unsubscribe(Type eventType, UnsubscribeOptions options) =>
        Add("Unsubscribe", null, options, eventType);

    public static void Subscribe(Type eventType, SubscribeOptions options) =>
        Add("Subscribe", null, options, eventType);
}