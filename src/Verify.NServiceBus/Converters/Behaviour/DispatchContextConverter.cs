class DispatchContextConverter :
    WriteOnlyJsonConverter<TestableDispatchContext>
{
    public override void Write(VerifyJsonWriter writer, TestableDispatchContext context)
    {
        writer.WriteStartObject();
        writer.WriteMember(context, context.Operations, "Operations");
        BehaviorContextConverter.WriteMembers(writer, context);
        writer.WriteEndObject();
    }
}