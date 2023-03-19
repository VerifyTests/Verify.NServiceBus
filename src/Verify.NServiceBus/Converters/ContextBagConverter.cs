class ContextBagConverter :
    WriteOnlyJsonConverter<ContextBag>
{
    public override void Write(VerifyJsonWriter writer, ContextBag bag)
    {
        writer.WriteStartObject();
        foreach (var (key, value) in bag.GetValues())
        {
            if (UnicastRouterHelper.TryWriteRoute(writer, key, value))
            {
                continue;
            }

            if (RoutingToDispatchConnectorHelper.TryWriteRoute(writer, key, value))
            {
                continue;
            }

            writer.WriteMember(bag, value, key);
        }

        writer.WriteEndObject();
    }
}

