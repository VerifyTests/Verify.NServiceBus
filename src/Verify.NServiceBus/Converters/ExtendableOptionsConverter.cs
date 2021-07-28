using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NServiceBus;
using NServiceBus.Extensibility;
using VerifyTests;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

class ExtendableOptionsConverter :
    WriteOnlyJsonConverter<ExtendableOptions>
{
    public override void WriteJson(JsonWriter writer, ExtendableOptions options, JsonSerializer serializer, IReadOnlyDictionary<string, object> context)
    {
        writer.WriteStartObject();
        WriteBaseMembers(writer, serializer, options);
        writer.WriteEndObject();
    }

    public static void WriteBaseMembers(JsonWriter writer, JsonSerializer serializer, ExtendableOptions options)
    {
        var messageId = options.GetMessageId();
        if (messageId != null)
        {
            writer.WritePropertyName("MessageId");
            serializer.Serialize(writer, messageId);
        }

        var headers = options.GetCleanedHeaders();
        if (headers.Any())
        {
            writer.WritePropertyName("Headers");
            serializer.Serialize(writer, headers);
        }

        var extensions = options.GetExtensions();
        if (extensions != null)
        {
            if (ContextBagHelper.HasContent(extensions))
            {
                writer.WritePropertyName("Extensions");
                serializer.Serialize(writer, extensions);
            }
        }
    }
}