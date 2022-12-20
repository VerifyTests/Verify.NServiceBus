class OutgoingPhysicalMessageContextConverter :
    WriteOnlyJsonConverter<TestableOutgoingPhysicalMessageContext>
{
    public override void Write(VerifyJsonWriter writer, TestableOutgoingPhysicalMessageContext context)
    {
        writer.WriteStartObject();

        writer.WriteMember(context, context.MessageId, "MessageId");
        writer.WriteMember(context, context.Body, "Body");
        var headers = ExtendableOptionsHelper.CleanedHeaders(context.Headers);
        writer.WriteListOrSingleMember(context, headers, "Headers");
        writer.WriteListOrSingleMember(context, context.PublishedMessages, "Published");
        writer.WriteListOrSingleMember(context, context.SentMessages, "Sent");
        writer.WriteListOrSingleMember(context, context.TimeoutMessages, "Timeouts");

        writer.WriteMember(context, context.Extensions, "Extensions");

        writer.WriteEndObject();
    }
}