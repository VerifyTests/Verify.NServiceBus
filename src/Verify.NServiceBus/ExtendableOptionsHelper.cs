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
                var indexOf = header.Value.IndexOf(',');
                if (indexOf == -1)
                {
                    dictionary.Add(key, header.Value);
                }
                else
                {
                    dictionary.Add(key, header.Value[..indexOf]);
                }

                continue;
            }

            dictionary.Add(key, header.Value);
        }

        return dictionary;
    }

    public static bool HasValue(this ExtendableOptions options)
    {
        if (HasMessageId(options))
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
        return headers.Any() ||
               options.HasContent();
    }

    static bool HasContent(this ExtendableOptions options)
    {
        var extensions = options.GetExtensions();
        if (extensions is not null)
        {
            return ContextBagHelper.HasContent(extensions);
        }

        return false;
    }

    static bool HasMessageId(this ExtendableOptions options) =>
        options.GetMessageId() is not null;
}