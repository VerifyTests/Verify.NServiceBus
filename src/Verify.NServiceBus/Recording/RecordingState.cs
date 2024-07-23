static class RecordingState
{
    public static void Publish(Published published) =>
        Recording.TryAdd("message", new KeyValuePair<string, Published>("Publish", published));

    public static void Reply(Replied replied) =>
        Recording.TryAdd("message", new KeyValuePair<string, Replied>("Reply", replied));

    public static void Send(Sent sent) =>
        Recording.TryAdd("message", new KeyValuePair<string, Sent>("Send", sent));

    public static void Unsubscribe(Unsubscribed unsubscribed) =>
        Recording.TryAdd("message", new KeyValuePair<string, Unsubscribed>("Unsubscribe", unsubscribed));

    public static void Subscribe(Subscribed subscribed) =>
        Recording.TryAdd("message", new KeyValuePair<string, Subscribed>("Subscribe", subscribed));
}