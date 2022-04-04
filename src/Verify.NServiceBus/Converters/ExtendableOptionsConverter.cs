using NServiceBus.Extensibility;

class ExtendableOptionsConverter :
    WriteOnlyJsonConverter<ExtendableOptions>
{
    public override void Write(VerifyJsonWriter writer, ExtendableOptions options)
    {
        writer.WriteStartObject();
        WriteBaseMembers(writer, options);
        writer.WriteEndObject();
    }

    public static void WriteBaseMembers(VerifyJsonWriter writer, ExtendableOptions options)
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