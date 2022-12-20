class OutgoingLogicalMessageContextConverter :
    WriteOnlyJsonConverter<TestableOutgoingLogicalMessageContext>
{
    public override void Write(VerifyJsonWriter writer, TestableOutgoingLogicalMessageContext context)
    {
        writer.WriteStartObject();

        var headers = ExtendableOptionsHelper.CleanedHeaders(context.Headers);
        writer.WriteListOrSingleMember(context, headers, "Headers");
        writer.WriteMember(context, context.Message, "Message");
        writer.WriteListOrSingleMember(context, context.PublishedMessages, "Published");
        writer.WriteListOrSingleMember(context, context.SentMessages, "Sent");
        writer.WriteListOrSingleMember(context, context.TimeoutMessages, "Timeouts");

        writer.WriteMember(context, context.Extensions, "Extensions");

        writer.WriteEndObject();
    }
}