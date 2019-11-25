using System;
using System.Collections.Generic;
using System.Linq;
using NServiceBus;
using NServiceBus.Extensibility;

static class ExtendableOptionsHelper
{
    public static Dictionary<string, string> GetCleanedHeaders(this ExtendableOptions options)
    {
        var dictionary = new Dictionary<string, string>();
        foreach (var header in options.GetHeaders())
        {
            var key = header.Key;
            if (key.StartsWith("NServiceBus."))
            {
                key = key.Substring(12);
            }
            if (header.Key == Headers.SagaType)
            {
                dictionary.Add(key, Type.GetType(header.Value, throwOnError: true).FullName);
                continue;
            }

            dictionary.Add(key, header.Value);
        }

        return dictionary;
    }

    public static bool HasValue(this ExtendableOptions options)
    {
        var messageId = options.GetMessageId();
        if (messageId != null)
        {
            return true;
        }

        if (options is SendOptions sendOptions)
        {
            if (sendOptions.GetDeliveryDate().HasValue || sendOptions.GetDeliveryDelay().HasValue)
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
        if (extensions != null)
        {
            return ContextBagHelper.HasContent(extensions);
        }

        return false;
    }
}