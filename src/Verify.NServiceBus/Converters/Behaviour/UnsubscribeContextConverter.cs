class UnsubscribeContextConverter :
    WriteOnlyJsonConverter<TestableUnsubscribeContext>
{
    public override void Write(VerifyJsonWriter writer, TestableUnsubscribeContext context)
    {
        writer.WriteStartObject();
        writer.WriteMember(context, context.EventType, "EventType");
        BehaviorContextConverter.WriteMembers(writer, context);
        writer.WriteEndObject();
    }
}