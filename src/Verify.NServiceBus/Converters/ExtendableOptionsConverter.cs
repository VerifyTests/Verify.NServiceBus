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
        writer.WriteMember(options, headers, "Headers");

        var bag = options.GetExtensions();
        if (bag is not null)
        {
            var bagValues = bag.GetValues().ToArray();
            if (bagValues.Length == 0)
            {
                return;
            }

            if (bagValues.Length == 1)
            {
                var (key, value) = bagValues[0];
                if (UnicastRouterHelper.TryWriteRoute(writer, key, value))
                {
                    return;
                }

                writer.WriteMember(options, value, key);
                return;
            }

            writer.WriteMember(options, bag, "Extensions");
        }
    }
}