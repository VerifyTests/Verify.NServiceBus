class RoutingContextConverter :
    WriteOnlyJsonConverter<TestableRoutingContext>
{
    public override void Write(VerifyJsonWriter writer, TestableRoutingContext context)
    {
        writer.WriteStartObject();
        writer.WriteMember(context, context.Message, "Message");
        BehaviorContextConverter.WriteMembers(writer, context);
        writer.WriteEndObject();
    }
}