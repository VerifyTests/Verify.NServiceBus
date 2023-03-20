class ExtendableOptionsConverter :
    WriteOnlyJsonConverter<ExtendableOptions>
{
    public override void Write(VerifyJsonWriter writer, ExtendableOptions options)
    {
        writer.WriteStartObject();
        writer.WriteMember(options, options.GetMessageId(), "MessageId");
        writer.WriteMember(options, options.GetCleanedHeaders(), "Headers");
        WriteExtensions(writer, options);
        writer.WriteEndObject();
    }

    public static void WriteExtensions(VerifyJsonWriter writer, ExtendableOptions options)
    {
        var bag = options.GetExtensions();
        if (bag is null)
        {
            return;
        }

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

            if (RoutingToDispatchConnectorHelper.TryWriteRoute(writer, key, value))
            {
                return;
            }

            writer.WriteMember(options, value, key);
            return;
        }

        writer.WriteMember(options, bag, "Extensions");
    }
}