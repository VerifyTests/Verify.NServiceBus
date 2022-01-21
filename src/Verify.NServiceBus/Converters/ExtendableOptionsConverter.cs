using NServiceBus;
using NServiceBus.Extensibility;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

class ExtendableOptionsConverter :
    WriteOnlyJsonConverter<ExtendableOptions>
{
    public override void Write(VerifyJsonWriter writer, ExtendableOptions options, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        WriteBaseMembers(writer, serializer, options);
        writer.WriteEndObject();
    }

    public static void WriteBaseMembers(VerifyJsonWriter writer, JsonSerializer serializer, ExtendableOptions options)
    {
        var messageId = options.GetMessageId();
        writer.WriteProperty(options, messageId, "MessageId");

        var headers = options.GetCleanedHeaders();
        writer.WriteProperty(options, headers, "Headers");

        var extensions = options.GetExtensions();
        if (extensions is not null)
        {
            if (ContextBagHelper.HasContent(extensions))
            {
                writer.WriteProperty(options, extensions, "Extensions");
            }
        }
    }
}