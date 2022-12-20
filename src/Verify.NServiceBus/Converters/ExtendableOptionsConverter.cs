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
        writer.WriteMember(options, messageId, "MessageId");

        var headers = options.GetCleanedHeaders();
        writer.WriteListOrSingleMember(options, headers, "Headers");

        var extensions = options.GetExtensions();
        if (extensions is not null)
        {
            if (ContextBagHelper.HasContent(extensions))
            {
                writer.WriteMember(options, extensions, "Extensions");
            }
        }
    }
}