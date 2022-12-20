class IncomingPhysicalMessageContextConverter :
    WriteOnlyJsonConverter<TestableIncomingPhysicalMessageContext>
{
    public override void Write(VerifyJsonWriter writer, TestableIncomingPhysicalMessageContext context)
    {
        writer.WriteStartObject();

        WriteMembers(writer, context);

        writer.WriteEndObject();
    }

    public static void WriteMembers(VerifyJsonWriter writer, TestableIncomingPhysicalMessageContext context)
    {
        IncomingContextConverter.WriteMembers(writer, context);
        writer.WriteMember(context, context.Message, "Message");
    }
}