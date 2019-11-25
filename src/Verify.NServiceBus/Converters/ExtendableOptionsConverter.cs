using System;
using System.Linq;
using Newtonsoft.Json;
using NServiceBus;
using NServiceBus.Extensibility;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

class ExtendableOptionsConverter :
    JsonConverter
{
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            return;
        }
        var options = (ExtendableOptions) value;
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

    public override bool CanConvert(Type type)
    {
        return typeof(ExtendableOptions).IsAssignableFrom(type);
    }
}