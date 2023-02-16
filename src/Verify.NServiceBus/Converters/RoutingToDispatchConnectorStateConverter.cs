class RoutingToDispatchConnectorStateConverter :
    WriteOnlyJsonConverter
{
    public override bool CanConvert(Type type) =>
        RoutingToDispatchConnectorHelper.IsRoutingToDispatchConnector(type);

    public override void Write(VerifyJsonWriter writer, object value)
    {
        writer.WriteStartObject();

        writer.WriteMember(value, RoutingToDispatchConnectorHelper.GetImmediateDispatch(value), "ImmediateDispatch");

        writer.WriteEndObject();
    }
}