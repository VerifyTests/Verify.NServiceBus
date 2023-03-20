static class ExtendableOptionsHelper
{
    public static Dictionary<string, string> GetCleanedHeaders(this ExtendableOptions options) =>
        CleanedHeaders(options.GetHeaders());

    public static Dictionary<string, string> CleanedHeaders(this IReadOnlyDictionary<string, string> headers)
    {
        var dictionary = new Dictionary<string, string>();
        foreach (var header in headers)
        {
            var key = header.Key;

            if (key.StartsWith("NServiceBus."))
            {
                key = key[12..];
            }

            if (header.Key == Headers.SagaType)
            {
                dictionary.Add(key, Type.GetType(header.Value, throwOnError: true)!.FullName!);
                continue;
            }

            dictionary.Add(key, header.Value);
        }

        return dictionary;
    }

    public static bool HasValue(this ExtendableOptions options)
    {
        var messageId = options.GetMessageId();
        if (messageId is not null)
        {
            return true;
        }

        if (options is SendOptions sendOptions)
        {
            if (sendOptions.GetDeliveryDate().HasValue ||
                sendOptions.GetDeliveryDelay().HasValue)
            {
                return true;
            }
        }

        var headers = options.GetHeaders();
        if (headers.Any())
        {
            return true;
        }

        var extensions = options.GetExtensions();
        if (extensions is not null)
        {
            return ContextBagHelper.HasContent(extensions);
        }

        return false;
    }
}