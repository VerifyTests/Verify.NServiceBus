class SubscribeContextConverter :
    WriteOnlyJsonConverter<TestableSubscribeContext>
{
    public override void Write(VerifyJsonWriter writer, TestableSubscribeContext context)
    {
        writer.WriteStartObject();
        writer.WriteMember(context, context.EventTypes, "EventTypes");
        BehaviorContextConverter.WriteMembers(writer, context);
        writer.WriteEndObject();
    }
}