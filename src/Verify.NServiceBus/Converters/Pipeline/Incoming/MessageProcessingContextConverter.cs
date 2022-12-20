class MessageProcessingContextConverter :
    WriteOnlyJsonConverter<TestableMessageProcessingContext>
{
    public override void Write(VerifyJsonWriter writer, TestableMessageProcessingContext context)
    {
        writer.WriteStartObject();

        WriteMembers(writer, context);

        writer.WriteEndObject();
    }

    public static void WriteMembers(VerifyJsonWriter writer, TestableMessageProcessingContext context)
    {
        var messageHeaders = ExtendableOptionsHelper.CleanedHeaders(context.MessageHeaders);
        writer.WriteListOrSingleMember(context, messageHeaders, "MessageHeaders");
        writer.WriteListOrSingleMember(context, context.RepliedMessages, "Replied");
        writer.WriteListOrSingleMember(context, context.ForwardedMessages, "Forwarded");
        PipelineContextConverter.WriteMembers(writer, context);
    }
}