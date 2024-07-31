namespace VerifyTests;

public static partial class VerifyNServiceBus
{
    public static string DefaultMessageIdString = "c5e12a59-e424-44c8-8875-e7822e534966";
    public static Guid DefaultMessageId { get; } = new(DefaultMessageIdString);

    public static string DefaultConversationIdString = "cd154c38-ab73-4a83-a66b-7404b9514080";
    public static Guid DefaultConversationId { get; } = new(DefaultConversationIdString);

    public static string DefaultCorrelationIdString = "87027093-7b35-4125-aa36-b5c15b9ea478";
    public static Guid DefaultCorrelationId { get; } = new(DefaultCorrelationIdString);

    public static string DefaultOriginatingEndpoint = "DefaultOriginatingEndpoint";

    public static string DefaultReplyToAddress = "ReplyToAddress";

    internal static FrozenDictionary<string, string> DefaultHeaders { get; private set; }
        = FrozenDictionary
            .ToFrozenDictionary<string, string>(
            [
                new(Headers.MessageId, DefaultMessageIdString),
                new(Headers.ConversationId, DefaultConversationIdString),
                new(Headers.CorrelationId, DefaultCorrelationIdString),
                new(Headers.ReplyToAddress, DefaultReplyToAddress),
                new(Headers.OriginatingEndpoint, DefaultOriginatingEndpoint),
                new("NServiceBus.TimeSent", "2000-01-01 13:00:00:000000 Z")
            ]);

    public static void AddSharedHeader(string key, string value)
    {
        var dictionary = new Dictionary<string, string>(DefaultHeaders)
        {
            [key] = value
        };
        DefaultHeaders = dictionary.ToFrozenDictionary();
    }

    public static void AddSharedHeaders(IEnumerable<KeyValuePair<string, string>> headers)
    {
        var dictionary = new Dictionary<string, string>(DefaultHeaders);
        foreach (var pair in headers)
        {
            dictionary[pair.Key] = pair.Value;
        }

        DefaultHeaders = dictionary.ToFrozenDictionary();
    }

    internal static Dictionary<string, string> MergeHeaders(IEnumerable<KeyValuePair<string, string>> headers)
    {
        var messageHeaders = new Dictionary<string, string>(headers);
        messageHeaders.TryAdd(Headers.MessageId, DefaultMessageIdString);
        messageHeaders.TryAdd(Headers.ConversationId, DefaultConversationIdString);
        messageHeaders.TryAdd(Headers.CorrelationId, DefaultCorrelationIdString);
        messageHeaders.TryAdd(Headers.ReplyToAddress, DefaultReplyToAddress);
        messageHeaders.TryAdd(Headers.OriginatingEndpoint, DefaultOriginatingEndpoint);
        messageHeaders.TryAdd("NServiceBus.TimeSent", "2000-01-01 13:00:00:000000 Z");
        return messageHeaders;
    }
}