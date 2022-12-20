class MessageHandlerContextConverter :
    WriteOnlyJsonConverter<TestableMessageHandlerContext>
{
    public override void Write(VerifyJsonWriter writer, TestableMessageHandlerContext context)
    {
        writer.WriteStartObject();

        WriteMembers(writer, context);

        writer.WriteEndObject();
    }

    public static void WriteMembers(VerifyJsonWriter writer, TestableMessageHandlerContext context)
    {

        var messageHeaders = ExtendableOptionsHelper.CleanedHeaders(context.MessageHeaders);
        if (context is TestHandlerContext)
        {
            messageHeaders.Remove("ConversationId");
            messageHeaders.Remove("TimeSent");
        }

        writer.WriteListOrSingleMember(context, messageHeaders, "MessageHeaders");
        writer.WriteListOrSingleMember(context, context.PublishedMessages, "Published");
        writer.WriteListOrSingleMember(context, context.RepliedMessages, "Replied");
        writer.WriteListOrSingleMember(context, context.SentMessages, "Sent");
        writer.WriteListOrSingleMember(context, context.ForwardedMessages, "Forwarded");
        writer.WriteListOrSingleMember(context, context.TimeoutMessages, "Timeouts");

        writer.WriteMember(context, context.Extensions, "Extensions");
        InvokeHandlerContextConverter.WriteMembers(writer, context);
    }
}