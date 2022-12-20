class IncomingLogicalMessageContextConverter :
    WriteOnlyJsonConverter<TestableIncomingLogicalMessageContext>
{
    public override void Write(VerifyJsonWriter writer, TestableIncomingLogicalMessageContext context)
    {
        writer.WriteStartObject();

        WriteMembers(writer, context);

        writer.WriteEndObject();
    }

    public static void WriteMembers(VerifyJsonWriter writer, TestableIncomingLogicalMessageContext context)
    {
        IncomingContextConverter.WriteMembers(writer, context);
        var headers = ExtendableOptionsHelper.CleanedHeaders(context.Headers);
        writer.WriteListOrSingleMember(context, headers, "Headers");
        writer.WriteMember(context, context.Message, "Message");
    }
}