class IncomingContextConverter :
    WriteOnlyJsonConverter<TestableIncomingContext>
{
    public override void Write(VerifyJsonWriter writer, TestableIncomingContext context)
    {
        writer.WriteStartObject();
        WriteMembers(writer, context);
        writer.WriteEndObject();
    }

    public static void WriteMembers(VerifyJsonWriter writer, TestableIncomingContext context) =>
        MessageProcessingContextConverter.WriteMembers(writer, context);
}