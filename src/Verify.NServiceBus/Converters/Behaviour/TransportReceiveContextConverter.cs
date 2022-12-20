class TransportReceiveContextConverter :
    WriteOnlyJsonConverter<TestableTransportReceiveContext>
{
    public override void Write(VerifyJsonWriter writer, TestableTransportReceiveContext context)
    {
        writer.WriteStartObject();
        writer.WriteMember(context, context.Message, "Message");
        BehaviorContextConverter.WriteMembers(writer, context);
        writer.WriteEndObject();
    }
}