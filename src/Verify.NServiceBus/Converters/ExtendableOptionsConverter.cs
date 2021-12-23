using Newtonsoft.Json;
using NServiceBus;
using NServiceBus.Extensibility;
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
        if (messageId is not null)
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
        if (extensions is not null)
        {
            if (ContextBagHelper.HasContent(extensions))
            {
                writer.WritePropertyName("Extensions");
                serializer.Serialize(writer, extensions);
            }
        }
    }
}